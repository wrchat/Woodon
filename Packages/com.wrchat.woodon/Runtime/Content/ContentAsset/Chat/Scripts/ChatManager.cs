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
	public class ChatManager : MBase
	{
		[Header("_" + nameof(ChatManager))]
		[SerializeField] private MPlayerUdonIndex mPlayerUdonIndex;
		[SerializeField] private WJson[] chatDatas;

		[SerializeField] private MSFXManager mSFXManager;

		[SerializeField] private MString nickname;
		[SerializeField] private UIChat[] chatUIs;

		[SerializeField] private int chatSaveCount = 10; // 각 채팅방에 저장할 메시지 수
		[SerializeField] private bool useInitChat = true;
		private DataDictionary curChatData = new DataDictionary();

		private void Start()
		{
			Init();
			
			for (int i = (int)TeamType.A; i <= (int)TeamType.Z; i++)
				ProcessChat((TeamType)i);
		}

		private void Init()
		{
			// DataDictionary chatData = new DataDictionary();
			// for (int i = (int)TeamType.A; i <= (int)TeamType.Z; i++)
			// {
			// 	string key = i.ToString();
			// 	chatData.Add(key, new DataList());
			// }

			// curChattingRoomValue.SetMinMaxValue((int)TeamType.A, (int)TeamType.Z);
			// curChattingRoomValue.RegisterListener(this, nameof(ProcessChat));

			for (int i = 0; i < chatDatas.Length; i++)
				chatDatas[i].RegisterListener(this, nameof(ReceieveChat) + i, WJsonEvent.OnDeserialization);
		}

		#region SendChat
		public void SendChatMessage(string message, TeamType chatRoom, string additionalData = "")
		{
			MDebugLog($"{nameof(SendChatMessage)} : {message}");

			if (string.IsNullOrEmpty(message))
				return;

			string name = string.IsNullOrEmpty(nickname.Value) ?
				Networking.LocalPlayer.displayName : nickname.Value;
			string formattedMessage = message.Trim();
			int udonIndex = mPlayerUdonIndex.GetUdonIndex();
			// int curChattingRoom = (int)this.curChattingRoom;
			int curChattingRoom = (int)chatRoom;

			// 클라이언트가 호출
			if (udonIndex == NONE_INT)
			{
				MDebugLog("Udon Index is None.");
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

		#region ReceieveChat
		public void ReceieveChat(int udonIndex)
		{
			MDebugLog($"{nameof(ReceieveChat)} : {udonIndex}");

			WJson wJson = chatDatas[udonIndex];

			if (wJson.Value == string.Empty)
			{
				MDebugLog("Value is Empty.");
				return;
			}

			DataDictionary chatData = wJson.DataDictionary.DeepClone();
			MDebugLog($"ChatData : {wJson.Value} || {chatData}");
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
				MDebugLog($"Sort : {curDataList.Count}");
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

			mSFXManager.PlaySFX_L((int)chatRoom);
			ProcessChat(chatRoom);

			// if (udonIndex == mPlayerUdonIndex.GetUdonIndex())
			// {
			// 	// chatInputField.Select();
			// 	// chatInputField.ActivateInputField();
			// }
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

			string debugText;
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < curDataList.Count; i++)
				{
					DataDictionary curChatData = curDataList[i].DataDictionary;
					int time = curChatData.GetChatTime();
					string name = curChatData.GetChatName();
					string message = curChatData.GetChatMessage();
					int udonIndex = curChatData.GetChatUdonIndex();

					stringBuilder.Append($"{time}\t\t| {udonIndex}-{name} : {message}\n");
				}

				debugText = stringBuilder.ToString();
			}

			UpdateUI(chatRoom, curDataList, debugText);
		}
		#endregion

		private void UpdateUI(TeamType chatRoom, DataList chatDataList, string debugText)
		{
			foreach (UIChat chatUI in chatUIs)
			{
				if (chatUI.ChatRoom == chatRoom)
					chatUI.SetChatText(chatDataList, debugText);
			}
		}

		public void ClearChatData_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ClearChatData));
		public void ClearChatData()
		{
			curChatData.Clear();
			for (int i = (int)TeamType.A; i <= (int)TeamType.Z; i++)
				ProcessChat((TeamType)i);
		}

		#region HorribleEvents
		public void ReceieveChat0() => ReceieveChat(0);
		public void ReceieveChat1() => ReceieveChat(1);
		public void ReceieveChat2() => ReceieveChat(2);
		public void ReceieveChat3() => ReceieveChat(3);
		public void ReceieveChat4() => ReceieveChat(4);
		public void ReceieveChat5() => ReceieveChat(5);
		public void ReceieveChat6() => ReceieveChat(6);
		public void ReceieveChat7() => ReceieveChat(7);
		public void ReceieveChat8() => ReceieveChat(8);
		public void ReceieveChat9() => ReceieveChat(9);
		public void ReceieveChat10() => ReceieveChat(10);
		public void ReceieveChat11() => ReceieveChat(11);
		public void ReceieveChat12() => ReceieveChat(12);
		public void ReceieveChat13() => ReceieveChat(13);
		public void ReceieveChat14() => ReceieveChat(14);
		public void ReceieveChat15() => ReceieveChat(15);
		public void ReceieveChat16() => ReceieveChat(16);
		public void ReceieveChat17() => ReceieveChat(17);
		public void ReceieveChat18() => ReceieveChat(18);
		public void ReceieveChat19() => ReceieveChat(19);
		public void ReceieveChat20() => ReceieveChat(20);
		public void ReceieveChat21() => ReceieveChat(21);
		public void ReceieveChat22() => ReceieveChat(22);
		public void ReceieveChat23() => ReceieveChat(23);
		public void ReceieveChat24() => ReceieveChat(24);
		public void ReceieveChat25() => ReceieveChat(25);
		public void ReceieveChat26() => ReceieveChat(26);
		public void ReceieveChat27() => ReceieveChat(27);
		public void ReceieveChat28() => ReceieveChat(28);
		public void ReceieveChat29() => ReceieveChat(29);
		public void ReceieveChat30() => ReceieveChat(30);
		public void ReceieveChat31() => ReceieveChat(31);
		public void ReceieveChat32() => ReceieveChat(32);
		public void ReceieveChat33() => ReceieveChat(33);
		public void ReceieveChat34() => ReceieveChat(34);
		public void ReceieveChat35() => ReceieveChat(35);
		public void ReceieveChat36() => ReceieveChat(36);
		public void ReceieveChat37() => ReceieveChat(37);
		public void ReceieveChat38() => ReceieveChat(38);
		public void ReceieveChat39() => ReceieveChat(39);
		public void ReceieveChat40() => ReceieveChat(40);
		public void ReceieveChat41() => ReceieveChat(41);
		public void ReceieveChat42() => ReceieveChat(42);
		public void ReceieveChat43() => ReceieveChat(43);
		public void ReceieveChat44() => ReceieveChat(44);
		public void ReceieveChat45() => ReceieveChat(45);
		public void ReceieveChat46() => ReceieveChat(46);
		public void ReceieveChat47() => ReceieveChat(47);
		public void ReceieveChat48() => ReceieveChat(48);
		public void ReceieveChat49() => ReceieveChat(49);
		public void ReceieveChat50() => ReceieveChat(50);
		public void ReceieveChat51() => ReceieveChat(51);
		public void ReceieveChat52() => ReceieveChat(52);
		public void ReceieveChat53() => ReceieveChat(53);
		public void ReceieveChat54() => ReceieveChat(54);
		public void ReceieveChat55() => ReceieveChat(55);
		public void ReceieveChat56() => ReceieveChat(56);
		public void ReceieveChat57() => ReceieveChat(57);
		public void ReceieveChat58() => ReceieveChat(58);
		public void ReceieveChat59() => ReceieveChat(59);
		public void ReceieveChat60() => ReceieveChat(60);
		public void ReceieveChat61() => ReceieveChat(61);
		public void ReceieveChat62() => ReceieveChat(62);
		public void ReceieveChat63() => ReceieveChat(63);
		public void ReceieveChat64() => ReceieveChat(64);
		public void ReceieveChat65() => ReceieveChat(65);
		public void ReceieveChat66() => ReceieveChat(66);
		public void ReceieveChat67() => ReceieveChat(67);
		public void ReceieveChat68() => ReceieveChat(68);
		public void ReceieveChat69() => ReceieveChat(69);
		public void ReceieveChat70() => ReceieveChat(70);
		public void ReceieveChat71() => ReceieveChat(71);
		public void ReceieveChat72() => ReceieveChat(72);
		public void ReceieveChat73() => ReceieveChat(73);
		public void ReceieveChat74() => ReceieveChat(74);
		public void ReceieveChat75() => ReceieveChat(75);
		public void ReceieveChat76() => ReceieveChat(76);
		public void ReceieveChat77() => ReceieveChat(77);
		public void ReceieveChat78() => ReceieveChat(78);
		public void ReceieveChat79() => ReceieveChat(79);
		public void ReceieveChat80() => ReceieveChat(80);
		#endregion
	}
}