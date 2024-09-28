using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTarget : MEventSender
	{
		[Header("_" + nameof(MTarget))]
		[SerializeField] private string autoTargetName = "-";

		// If You Use UdonGraph, Don't Change The '_curTargetPlayerID' Variable's Access Modifier To Private.
		// If You Don't, You Can Just Change It If You Want.
		[UdonSynced, FieldChangeCallback(nameof(CurTargetPlayerID))] private int _curTargetPlayerID = NONE_INT;
		public int CurTargetPlayerID
		{
			get => _curTargetPlayerID;
			set
			{
				_curTargetPlayerID = value;
				SendEvents();
			}
		}

		public int[] PlayerIDBuffer { get; private set; } = new int[80];

		[field: Header("_" + nameof(MTarget) + " - Options")]
		[field: SerializeField] public bool UseNone { get; private set; } = true;

		// ---- ---- ---- ----
		
		public bool IsTargetPlayer(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;
			
			return targetPlayer.playerId == CurTargetPlayerID;
		}

		public VRCPlayerApi GetTargetPlayerAPI() => VRCPlayerApi.GetPlayerById(CurTargetPlayerID);

		public void SetPlayerID(int id)
		{
			SetOwner();
			CurTargetPlayerID = id;
			RequestSerialization();
		}

		public void SetLocalPlayer() => SetPlayerID(Networking.LocalPlayer.playerId);
		public void SetNone() => SetPlayerID(NONE_INT);

		public void ToggleLocalPlayer()
		{
			if (IsTargetPlayer(Networking.LocalPlayer))
				SetNone();
			else
				SetLocalPlayer();
		}

		public void SelectPlayer(int index) => SetPlayerID(PlayerIDBuffer[index]);

		// ---- ---- ---- ----

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (Networking.IsMaster)
			{
				SetOwner();
				CurTargetPlayerID = UseNone ? NONE_INT : Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			if (IsOwner() && (Networking.LocalPlayer.displayName == autoTargetName))
			{
				CurTargetPlayerID = Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
			else
			{
				UpdatePlayerIDBuffer();
			}
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner() && (player.playerId == CurTargetPlayerID))
			{
				CurTargetPlayerID = UseNone ? NONE_INT : Networking.LocalPlayer.playerId;
				RequestSerialization();
			}
			else
			{
				UpdatePlayerIDBuffer();
			}
		}

		public void UpdatePlayerIDBuffer()
		{
			VRCPlayerApi[] players = GetPlayers();

			if (players.Length != VRCPlayerApi.GetPlayerCount())
			{
				SendCustomEventDelayedSeconds(nameof(UpdatePlayerIDBuffer), .3f);
				return;
			}

			for (int i = 0; i < PlayerIDBuffer.Length; i++)
			{
				if (i >= players.Length)
				{
					PlayerIDBuffer[i] = -1;
				}
				else
				{
					PlayerIDBuffer[i] = players[i].playerId;
				}
			}

			SendEvents();
		}
	}
}