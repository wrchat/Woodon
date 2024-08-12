using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MPlayerUdonIndex : MBase
	{
		// https://cafe.naver.com/steamindiegame/14065241

		[UdonSynced, FieldChangeCallback(nameof(PlayerUdonIndexDataPack))] private string _playerUdonIndexDataPack = string.Empty;
		public string PlayerUdonIndexDataPack
		{
			get => _playerUdonIndexDataPack;
			set
			{
				_playerUdonIndexDataPack = value;
				MDebugLog($"{nameof(_playerUdonIndexDataPack)}, = {_playerUdonIndexDataPack}");

				if (DEBUG && debugText != null)
				{
					string debugS = $"{_playerUdonIndexDataPack}\n" +
						$"LOCAL = {Networking.LocalPlayer.displayName} - {Networking.LocalPlayer.playerId}, {Networking.IsMaster}\n" +
						$"PlayerCount = {VRCPlayerApi.GetPlayerCount()},\n" +
						$"{nameof(enableUdonCount)} = {enableUdonCount}, {CanUpdateNow}\n";
					string[] datas = _playerUdonIndexDataPack.Split(DATA_SEPARATOR);
					for (int i = 0; i < datas.Length; i++)
						debugS += datas[i] + '\n';
					debugText.text = debugS;
				}
			}
		}

		public int GetUdonIndex(VRCPlayerApi targetPlayer = null)
		{
			MDebugLog($"{nameof(GetUdonIndex)} : {targetPlayer}");

			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			for (int i = 0; i < playerNameByUdonIndex.Length; i++)
			{
				if (playerNameByUdonIndex[i] == (targetPlayer.displayName + nickSeparater + targetPlayer.playerId))
					return i;
			}

			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(ReqUpdatePlayerList));
			return NONE_INT;
		}

		public void ReqUpdatePlayerList()
		{
			MDebugLog(nameof(ReqUpdatePlayerList));
			UpdatePlayerList();
		}

		public VRCPlayerApi[] PlayerApis { get; private set; }
		public bool CanUpdateNow =>
			(PlayerApis != null) &&
			(PlayerApis.Length == VRCPlayerApi.GetPlayerCount()) &&
			(enableUdonCount == VRCPlayerApi.GetPlayerCount());

		private string[] playerNameByUdonIndex = new string[80];
		private int enableUdonCount = 0;
		[SerializeField] private TextMeshProUGUI debugText;

		private void Update()
		{
			if (!CanUpdateNow)
				UpdatePlayerList();
		}

		private char nickSeparater = '#';

		private void UpdatePlayerList()
		{
			MDebugLog($"{nameof(UpdatePlayerList)}, PlayerCount = {VRCPlayerApi.GetPlayerCount()}");

			PlayerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VRCPlayerApi.GetPlayers(PlayerApis);

			string[] datas = PlayerUdonIndexDataPack.Split(DATA_SEPARATOR);
			playerNameByUdonIndex = new string[80];
			enableUdonCount = 0;
			MDebugLog($"PlayerUdonIndexDataPack = {PlayerUdonIndexDataPack}");
			MDebugLog($"datasLength = {datas.Length}");
			for (int i = 0; i < datas.Length; i++)
				playerNameByUdonIndex[i] = datas[i];

			// 존재하지 않는 플레이어는 제거
			for (int i = 0; i < playerNameByUdonIndex.Length; i++)
			{
				if (string.IsNullOrEmpty(playerNameByUdonIndex[i]))
					continue;

				VRCPlayerApi targetPlayerAPI = VRCPlayerApi.GetPlayerById(int.Parse(playerNameByUdonIndex[i].Split(nickSeparater)[1]));

				if (targetPlayerAPI == null)
				{
					playerNameByUdonIndex[i] = string.Empty;
					break;
				}
			}

			// 각 플레이어에 대해
			foreach (VRCPlayerApi player in PlayerApis)
			{
				bool hasUdon = false;

				// 있는지 없는지 확인하고
				for (int i = 0; i < playerNameByUdonIndex.Length; i++)
				{
					if (string.IsNullOrEmpty(playerNameByUdonIndex[i]))
						continue;

					if (playerNameByUdonIndex[i] == player.displayName + nickSeparater + player.playerId)
					{
						hasUdon = true;
						enableUdonCount++;
						break;
					}
				}

				if (hasUdon)
					continue;

				// 없으면 빈 곳을 찾아 대입
				for (int i = 0; i < playerNameByUdonIndex.Length; i++)
				{
					if (string.IsNullOrEmpty(playerNameByUdonIndex[i]))
					{
						playerNameByUdonIndex[i] = player.displayName + nickSeparater + player.playerId;
						enableUdonCount++;
						break;
					}
				}
			}

			string newDataPack = string.Empty;

			for (int i = 0; i < playerNameByUdonIndex.Length; i++)
				newDataPack += playerNameByUdonIndex[i] + DATA_SEPARATOR;

			newDataPack = newDataPack.Trim(new char[] { DATA_SEPARATOR });

			if (PlayerUdonIndexDataPack != newDataPack)
			{
				if (!Networking.IsMaster)
					return;

				SetOwner();
				PlayerUdonIndexDataPack = newDataPack;
				RequestSerialization();
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			if (IsOwner())
				UpdatePlayerList();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner())
				UpdatePlayerList();
		}
	}
}