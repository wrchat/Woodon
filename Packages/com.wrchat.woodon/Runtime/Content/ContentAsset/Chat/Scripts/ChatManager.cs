using System.Text;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon.Chat;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ChatManager : WEventPublisher
	{
		[Header("_" + nameof(ChatManager))]
		[SerializeField] private WPlayerUdonIndex mPlayerUdonIndex;
		[SerializeField] private WJson[] chatDatas;

		[SerializeField] private WString nickname;

		[SerializeField] private int chatSaveCount = 10; // 각 채팅방에 저장할 메시지 수
		[SerializeField] private bool useInitChat = true;

		private DataDictionary curChatData = new DataDictionary();

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			for (int i = 0; i < chatDatas.Length; i++)
				chatDatas[i].RegisterListener(this, nameof(ReceiveChat) + i, WJsonEvent.OnDeserialization);

			for (int i = (int)TeamType.A; i <= (int)TeamType.Z; i++)
				ProcessChat((TeamType)i);
		}

		#region SendChat
		public void SendChatMessage(string message, TeamType chatRoom, string additionalData = "")
		{
			WDebugLog($"{nameof(SendChatMessage)} : {message}");

			if (string.IsNullOrEmpty(message))
				return;

			string name = string.IsNullOrEmpty(nickname.Value) ?
				Networking.LocalPlayer.displayName : nickname.Value;
			string formattedMessage = message.Trim();
			int udonIndex = mPlayerUdonIndex.GetUdonIndex();
			int curChattingRoom = (int)chatRoom;

			// 클라이언트가 호출
			if (udonIndex == NONE_INT)
			{
				WDebugLog("Udon Index is None.");
				return;
			}

			WJson wJson = chatDatas[udonIndex];
			wJson.SetData("Name", name);
			wJson.SetData("Time", Networking.GetServerTimeInMilliseconds());
			wJson.SetData("Message", formattedMessage);
			wJson.SetData("UdonIndex", udonIndex);
			wJson.SetData("ChatRoom", curChattingRoom);
			wJson.SetData("AdditionalData", additionalData);
			wJson.SerializeData();
		}
		#endregion

		#region ReceiveChat
		public void ReceiveChat(int udonIndex)
		{
			WDebugLog($"{nameof(ReceiveChat)} : {udonIndex}");

			WJson wJson = chatDatas[udonIndex];

			if (wJson.Value == string.Empty)
			{
				WDebugLog("Value is Empty.");
				return;
			}

			DataDictionary chatData = wJson.DataDictionary.DeepClone();
			WDebugLog($"ChatData : {wJson.Value} || {chatData}");
			TeamType chatRoom = chatData.GetChatRoom();
			string chatRoomString = chatRoom.ToString();

			// 데이터 추가
			{
				if (curChatData.TryGetValue(chatRoomString, out DataToken dataToken))
				{
					dataToken.DataList.Add(chatData);
				}
				else
				{
					DataList chatDataList = new DataList();
					chatDataList.Add(chatData);
					curChatData.Add(chatRoomString, chatDataList);
				}
			}

			DataList curDataList = curChatData[chatRoomString].DataList;

			// 정렬 (시간순)
			{
				WDebugLog($"Sort : {curDataList.Count}");
				for (int i = 0; i < curDataList.Count; i++)
				{
					DataDictionary curChatData = curDataList[i].DataDictionary;
					int curTime = curChatData.GetChatTime();

					for (int j = i + 1; j < curDataList.Count; j++)
					{
						DataDictionary nextChatData = curDataList[j].DataDictionary;
						int nextTime = nextChatData.GetChatTime();

						if (curTime > nextTime)
						{
							curDataList[i] = nextChatData;
							curDataList[j] = curChatData;
						}
					}
				}
			}

			// 가장 오래된 메시지 제거 (chatSaveCount개 이상일 경우)
			{
				if (curDataList.Count > chatSaveCount)
				{
					curDataList.RemoveAt(0);
				}
			}

			ProcessChat(chatRoom);

			SendEvents(chatRoom);
			SendEvents();
		}

		public void ProcessChat(TeamType chatRoom)
		{
			DataList curDataList;
			{
				string curChatRoomString = chatRoom.ToString();
				if (curChatData.TryGetValue(curChatRoomString, out DataToken dataToken))
				{
					curDataList = dataToken.DataList;

					// remove InitChat
					for (int i = 0; i < curDataList.Count; i++)
					{
						DataDictionary curChatData = curDataList[i].DataDictionary;
						int chatTime = curChatData.GetChatTime();

						if (chatTime == 0)
						{
							curDataList.RemoveAt(i);
							break;
						}
					}
				}
				else
				{
					curDataList = new DataList();

					// Init DataList
					if (useInitChat)
					{
						// InitChat
						DataDictionary chatData = new DataDictionary();
						chatData.UpdateChatData(0, "System", $"{chatRoom} 채팅방입니다.", NONE_INT, chatRoom);
						curDataList.Add(chatData);
					}
					else
					{
						DataDictionary tempChatData = new DataDictionary();
						curDataList.Add(tempChatData);
						curDataList.RemoveAt(0);
					}

					curChatData.Add(curChatRoomString, curDataList);
				}
			}
		}

		public DataList GetChatDataList(TeamType chatRoom)
		{
			string curChatRoomString = chatRoom.ToString();
			if (curChatData.TryGetValue(curChatRoomString, out DataToken dataToken))
			{
				return dataToken.DataList;
			}
			else
			{
				WDebugLog($"No chat data found for {curChatRoomString}. Returning empty DataList.");
				return new DataList();
			}
		}
		#endregion

		public string GetDebugText(DataList chatDataList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < chatDataList.Count; i++)
			{
				DataDictionary curChatData = chatDataList[i].DataDictionary;
				int time = curChatData.GetChatTime();
				string name = curChatData.GetChatName();
				string message = curChatData.GetChatMessage();
				int udonIndex = curChatData.GetChatUdonIndex();

				stringBuilder.Append($"{time}\t\t| {udonIndex}-{name} : {message}\n");
			}
			return stringBuilder.ToString();
		}

		public void ClearChatData_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ClearChatData));
		public void ClearChatData()
		{
			curChatData.Clear();
			for (int i = (int)TeamType.A; i <= (int)TeamType.Z; i++)
			{
				ProcessChat((TeamType)i);
				SendEvents((TeamType)i);
			}
			SendEvents();
		}

		#region HorribleEvents
		public void ReceiveChat0() => ReceiveChat(0);
		public void ReceiveChat1() => ReceiveChat(1);
		public void ReceiveChat2() => ReceiveChat(2);
		public void ReceiveChat3() => ReceiveChat(3);
		public void ReceiveChat4() => ReceiveChat(4);
		public void ReceiveChat5() => ReceiveChat(5);
		public void ReceiveChat6() => ReceiveChat(6);
		public void ReceiveChat7() => ReceiveChat(7);
		public void ReceiveChat8() => ReceiveChat(8);
		public void ReceiveChat9() => ReceiveChat(9);
		public void ReceiveChat10() => ReceiveChat(10);
		public void ReceiveChat11() => ReceiveChat(11);
		public void ReceiveChat12() => ReceiveChat(12);
		public void ReceiveChat13() => ReceiveChat(13);
		public void ReceiveChat14() => ReceiveChat(14);
		public void ReceiveChat15() => ReceiveChat(15);
		public void ReceiveChat16() => ReceiveChat(16);
		public void ReceiveChat17() => ReceiveChat(17);
		public void ReceiveChat18() => ReceiveChat(18);
		public void ReceiveChat19() => ReceiveChat(19);
		public void ReceiveChat20() => ReceiveChat(20);
		public void ReceiveChat21() => ReceiveChat(21);
		public void ReceiveChat22() => ReceiveChat(22);
		public void ReceiveChat23() => ReceiveChat(23);
		public void ReceiveChat24() => ReceiveChat(24);
		public void ReceiveChat25() => ReceiveChat(25);
		public void ReceiveChat26() => ReceiveChat(26);
		public void ReceiveChat27() => ReceiveChat(27);
		public void ReceiveChat28() => ReceiveChat(28);
		public void ReceiveChat29() => ReceiveChat(29);
		public void ReceiveChat30() => ReceiveChat(30);
		public void ReceiveChat31() => ReceiveChat(31);
		public void ReceiveChat32() => ReceiveChat(32);
		public void ReceiveChat33() => ReceiveChat(33);
		public void ReceiveChat34() => ReceiveChat(34);
		public void ReceiveChat35() => ReceiveChat(35);
		public void ReceiveChat36() => ReceiveChat(36);
		public void ReceiveChat37() => ReceiveChat(37);
		public void ReceiveChat38() => ReceiveChat(38);
		public void ReceiveChat39() => ReceiveChat(39);
		public void ReceiveChat40() => ReceiveChat(40);
		public void ReceiveChat41() => ReceiveChat(41);
		public void ReceiveChat42() => ReceiveChat(42);
		public void ReceiveChat43() => ReceiveChat(43);
		public void ReceiveChat44() => ReceiveChat(44);
		public void ReceiveChat45() => ReceiveChat(45);
		public void ReceiveChat46() => ReceiveChat(46);
		public void ReceiveChat47() => ReceiveChat(47);
		public void ReceiveChat48() => ReceiveChat(48);
		public void ReceiveChat49() => ReceiveChat(49);
		public void ReceiveChat50() => ReceiveChat(50);
		public void ReceiveChat51() => ReceiveChat(51);
		public void ReceiveChat52() => ReceiveChat(52);
		public void ReceiveChat53() => ReceiveChat(53);
		public void ReceiveChat54() => ReceiveChat(54);
		public void ReceiveChat55() => ReceiveChat(55);
		public void ReceiveChat56() => ReceiveChat(56);
		public void ReceiveChat57() => ReceiveChat(57);
		public void ReceiveChat58() => ReceiveChat(58);
		public void ReceiveChat59() => ReceiveChat(59);
		public void ReceiveChat60() => ReceiveChat(60);
		public void ReceiveChat61() => ReceiveChat(61);
		public void ReceiveChat62() => ReceiveChat(62);
		public void ReceiveChat63() => ReceiveChat(63);
		public void ReceiveChat64() => ReceiveChat(64);
		public void ReceiveChat65() => ReceiveChat(65);
		public void ReceiveChat66() => ReceiveChat(66);
		public void ReceiveChat67() => ReceiveChat(67);
		public void ReceiveChat68() => ReceiveChat(68);
		public void ReceiveChat69() => ReceiveChat(69);
		public void ReceiveChat70() => ReceiveChat(70);
		public void ReceiveChat71() => ReceiveChat(71);
		public void ReceiveChat72() => ReceiveChat(72);
		public void ReceiveChat73() => ReceiveChat(73);
		public void ReceiveChat74() => ReceiveChat(74);
		public void ReceiveChat75() => ReceiveChat(75);
		public void ReceiveChat76() => ReceiveChat(76);
		public void ReceiveChat77() => ReceiveChat(77);
		public void ReceiveChat78() => ReceiveChat(78);
		public void ReceiveChat79() => ReceiveChat(79);
		public void ReceiveChat80() => ReceiveChat(80);
		#endregion
	}
}