using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WPlayerUdonIndex : WBase
	{
		// https://cafe.naver.com/steamindiegame/14065241
		private const char NICK_SEPARATOR = '#';

		[Header("_" + nameof(WPlayerUdonIndex))]
		[SerializeField] private TextMeshProUGUI debugText;

		[UdonSynced, FieldChangeCallback(nameof(PlayerUdonIndexDataPack))] private string _playerUdonIndexDataPack = string.Empty;
		public string PlayerUdonIndexDataPack
		{
			get => _playerUdonIndexDataPack;
			private set
			{
				_playerUdonIndexDataPack = value;
				WDebugLog($"{nameof(_playerUdonIndexDataPack)}, = {_playerUdonIndexDataPack}");

				if ((debugMode == WLogMode.None) && (debugText != null))
				{
					string debugS = $"{_playerUdonIndexDataPack}\n" +
						$"LOCAL = {Networking.LocalPlayer.displayName} - {Networking.LocalPlayer.playerId}, {Networking.IsMaster}\n" +
						$"PlayerCount = {VRCPlayerApi.GetPlayerCount()},\n" +
						$"{nameof(enableUdonCount)} = {enableUdonCount}, {CanUpdateNow}\n";
					
					string[] data = _playerUdonIndexDataPack.Split(DATA_SEPARATOR);
					for (int i = 0; i < data.Length; i++)
						debugS += data[i] + '\n';
						
					debugText.text = debugS;
				}
			}
		}

		public VRCPlayerApi[] PlayerApis { get; private set; }
		public bool CanUpdateNow =>
			(PlayerApis != null) &&
			(PlayerApis.Length == VRCPlayerApi.GetPlayerCount()) &&
			(enableUdonCount == VRCPlayerApi.GetPlayerCount());

		private string[] playerNameByUdonIndex = new string[80];
		private int enableUdonCount = 0;

		public int GetUdonIndex(VRCPlayerApi targetPlayer = null)
		{
			WDebugLog($"{nameof(GetUdonIndex)} : {targetPlayer}");

			if (IsNotOnline())
				return NONE_INT;

			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			for (int i = 0; i < playerNameByUdonIndex.Length; i++)
			{
				if (playerNameByUdonIndex[i] == (targetPlayer.displayName + NICK_SEPARATOR + targetPlayer.playerId))
					return i;
			}

			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(ReqUpdatePlayerList));
			return NONE_INT;
		}

		public VRCPlayerApi GetPlayerApi(int udonIndex)
		{
			WDebugLog($"{nameof(GetPlayerApi)} : {udonIndex}");

			if (IsNotOnline())
				return null;

			if (udonIndex < 0 || udonIndex >= playerNameByUdonIndex.Length)
				return null;

			if (string.IsNullOrEmpty(playerNameByUdonIndex[udonIndex]))
				return null;

			string[] data = playerNameByUdonIndex[udonIndex].Split(NICK_SEPARATOR);
			if (data == null || data.Length != 2)
				return null;

			int playerId = int.Parse(data[1]);
			return VRCPlayerApi.GetPlayerById(playerId);
		}

		public void ReqUpdatePlayerList()
		{
			WDebugLog(nameof(ReqUpdatePlayerList));
			UpdatePlayerList();
		}

		private void Update()
		{
			if (CanUpdateNow == false)
				UpdatePlayerList();
		}

		private void UpdatePlayerList()
		{
			WDebugLog($"{nameof(UpdatePlayerList)}, PlayerCount = {VRCPlayerApi.GetPlayerCount()}");

			PlayerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VRCPlayerApi.GetPlayers(PlayerApis);

			string[] data = PlayerUdonIndexDataPack.Split(DATA_SEPARATOR);
			playerNameByUdonIndex = new string[80];
			enableUdonCount = 0;
			WDebugLog($"PlayerUdonIndexDataPack = {PlayerUdonIndexDataPack}");
			WDebugLog($"dataLength = {data.Length}");
			for (int i = 0; i < data.Length; i++)
				playerNameByUdonIndex[i] = data[i];

			// 존재하지 않는 플레이어는 제거
			for (int i = 0; i < playerNameByUdonIndex.Length; i++)
			{
				if (string.IsNullOrEmpty(playerNameByUdonIndex[i]))
					continue;

				VRCPlayerApi targetPlayerAPI = VRCPlayerApi.GetPlayerById(int.Parse(playerNameByUdonIndex[i].Split(NICK_SEPARATOR)[1]));

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

					if (playerNameByUdonIndex[i] == player.displayName + NICK_SEPARATOR + player.playerId)
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
						playerNameByUdonIndex[i] = player.displayName + NICK_SEPARATOR + player.playerId;
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