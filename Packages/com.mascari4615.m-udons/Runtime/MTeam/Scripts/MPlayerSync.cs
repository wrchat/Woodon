using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MPlayerSync : MEventSender
	{
		[Header("_" + nameof(MPlayerSync))]
		[SerializeField] private TextMeshProUGUI[] nicknameTexts;
		[SerializeField] private bool toggleIt;
		[SerializeField] private bool toggleOthers;
		[SerializeField] private bool convertWaktaNickname = true;
		[SerializeField] private string defaultText = "-";

		public string playerNickname = string.Empty;
		public VRCPlayerApi playerAPI;

		[UdonSynced()]
		[FieldChangeCallback(nameof(PlayerID))]
		private int playerID = NONE_INT;

		public int PlayerID
		{
			get => playerID;
			set
			{
				playerID = value;
				OnPlayerIDChange();
			}
		}

		private void Start()
		{
			OnPlayerIDChange();
		}

		private void OnPlayerIDChange()
		{
			MDebugLog($"{nameof(OnPlayerIDChange)}");

			playerAPI = VRCPlayerApi.GetPlayerById(playerID);
			playerNickname = playerAPI == null ? defaultText : playerAPI.displayName;

			if (playerAPI != null)
				if (convertWaktaNickname)
					playerNickname = Waktaverse.GetNickname(playerAPI.displayName);

			foreach (TextMeshProUGUI nicknameText in nicknameTexts)
				nicknameText.text = playerNickname;

			if (PlayerID != NONE_INT && playerAPI == null && IsOwner())
			{
				PlayerID = NONE_INT;
				RequestSerialization();
			}

			SendEvents();
		}

		public void SyncLocalPlayer()
		{
			MDebugLog(nameof(SyncLocalPlayer));

			if (PlayerID == NONE_INT)
			{
				if (PlayerID != Networking.LocalPlayer.playerId)
				{
					SetOwner();
					PlayerID = Networking.LocalPlayer.playerId;
					RequestSerialization();
				}
			}
			else
			{
				if (toggleOthers || PlayerID == Networking.LocalPlayer.playerId)
					if (toggleIt)
						ClearPlayer();
			}
		}

		public void ClearPlayer()
		{
			MDebugLog(nameof(ClearPlayer));

			if (PlayerID != NONE_INT)
			{
				SetOwner();
				PlayerID = NONE_INT;
				RequestSerialization();
			}
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			MDebugLog(nameof(OnPlayerLeft));

			if (IsLocalPlayer(player))
			{
				MDebugLog("ClearPlayer Before Exit");
				ClearPlayer();
			}
		}
	}
}