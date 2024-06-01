using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class LilpaTacticalManager : MBase
	{
		[SerializeField] private TextMeshProUGUI gameStateText;
		[SerializeField] private MTeamManager mTeamManager;
		[SerializeField] private Transform[] teamATpPos;
		[SerializeField] private Transform[] teamBTpPos;

		[SerializeField] private Transform[] teamARespawnTpPos;
		[SerializeField] private Transform[] teamBRespawnTpPos;

		[SerializeField] private GameObject[] hits;
		[SerializeField] private VRC_Pickup[] pickups;

		[SerializeField] private SyncedBool[] hitActives;

		[SerializeField] private Image hitsActiveButtonImage;

		[SerializeField] private Image gameActiveStateUIImage;

		[UdonSynced()]
		[FieldChangeCallback(nameof(GameState))]
		private bool gameState = true;

		[UdonSynced()]
		[FieldChangeCallback(nameof(HitsActive))]
		private bool hitsActive = true;

		private VRCObjectSync[] objectSyncs;

		private bool HitsActive
		{
			get => hitsActive;
			set
			{
				hitsActive = value;
				OnHitsActiveChange();
			}
		}

		private bool GameState
		{
			get => gameState;
			set
			{
				gameState = value;
				OnGameStateChange();
			}
		}

		private void Start()
		{
			objectSyncs = new VRCObjectSync[pickups.Length];
			for (var i = 0; i < pickups.Length; i++)
				objectSyncs[i] = pickups[i].GetComponent<VRCObjectSync>();

			OnHitsActiveChange();
			OnGameStateChange();
		}

		private void OnHitsActiveChange()
		{
			MDebugLog(nameof(OnHitsActiveChange));

			hitsActiveButtonImage.color = MColorUtil.GetGreenOrRed(hitsActive);

			foreach (var hit in hits)
				hit.SetActive(hitsActive);
		}

		public void ToggleHitsActive()
		{
			MDebugLog(nameof(ToggleHitsActive));

			SetOwner();
			HitsActive = !HitsActive;
			RequestSerialization();
		}

		private void OnGameStateChange()
		{
			MDebugLog(nameof(OnGameStateChange));

			gameActiveStateUIImage.color = MColorUtil.GetGreenOrRed(!GameState);
			gameStateText.text = GameState ? "RESET" : "START";
		}

		public void ToggleGameState()
		{
			MDebugLog(nameof(ToggleGameState));

			SetOwner();
			ResetGame();
			GameState = !GameState;

			if (GameState)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TryTeleport));
			else
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TryRespawn));

			RequestSerialization();
		}

		public void ResetGame()
		{
			MDebugLog(nameof(ResetGame));

			for (var i = 0; i < pickups.Length; i++)
			{
				SetOwner(pickups[i].gameObject);
				pickups[i].Drop();
				objectSyncs[i].Respawn();
			}

			for (var i = 0; i < hitActives.Length; i++) hitActives[i].SetValue(false);
		}

		public void TryTeleport()
		{
			MDebugLog(nameof(TryTeleport));

			var localTeamType = mTeamManager.GetTargetPlayerTeamType();

			if (localTeamType == TeamType.None)
				return;

			if (localTeamType == TeamType.A)
			{
				var playerIndex = mTeamManager.MTeams[0].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamATpPos[playerIndex].position, teamATpPos[playerIndex].rotation);
			}
			else if (localTeamType == TeamType.B)
			{
				var playerIndex = mTeamManager.MTeams[1].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamBTpPos[playerIndex].position, teamBTpPos[playerIndex].rotation);
			}
		}

		public void TryRespawn()
		{
			MDebugLog(nameof(TryRespawn));

			var localTeamType = mTeamManager.GetTargetPlayerTeamType();

			if (localTeamType == TeamType.None)
				return;

			if (localTeamType == TeamType.A)
			{
				var playerIndex = mTeamManager.MTeams[0].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamARespawnTpPos[playerIndex].position,
					teamARespawnTpPos[playerIndex].rotation);
			}
			else if (localTeamType == TeamType.B)
			{
				var playerIndex = mTeamManager.MTeams[1].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamBRespawnTpPos[playerIndex].position,
					teamBRespawnTpPos[playerIndex].rotation);
			}
		}
	}
}