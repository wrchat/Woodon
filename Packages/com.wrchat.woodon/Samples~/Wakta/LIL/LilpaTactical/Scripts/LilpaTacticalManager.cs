using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.LIL.LilpaTactical
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class LilpaTacticalManager : MBase
	{
		[Header("_" + nameof(LilpaTacticalManager))]
		[SerializeField] private TextMeshProUGUI gameStateText;
		[SerializeField] private MTeamManager mTeamManager;
		[SerializeField] private Transform[] teamATpPos;
		[SerializeField] private Transform[] teamBTpPos;

		[SerializeField] private Transform[] teamARespawnTpPos;
		[SerializeField] private Transform[] teamBRespawnTpPos;

		[SerializeField] private GameObject[] hits;
		[SerializeField] private VRC_Pickup[] pickups;

		[SerializeField] private MBool[] hitActives;

		[SerializeField] private Image hitsActiveButtonImage;

		[SerializeField] private Image gameActiveStateUIImage;

		private VRCObjectSync[] objectSyncs;

		[SerializeField] private MBool hitsActive;
		[SerializeField] private MBool gameState;

		private void Start()
		{
			Init();

			OnHitsActiveChange();
			OnGameStateChange();
		}

		private void Init()
		{
			objectSyncs = new VRCObjectSync[pickups.Length];
			for (int i = 0; i < pickups.Length; i++)
				objectSyncs[i] = pickups[i].GetComponent<VRCObjectSync>();

			hitsActive.RegisterListener(this, nameof(OnHitsActiveChange));
			gameState.RegisterListener(this, nameof(OnGameStateChange));
		}

		public void OnHitsActiveChange()
		{
			MDebugLog(nameof(OnHitsActiveChange));

			hitsActiveButtonImage.color = MColorUtil.GetGreenOrRed(hitsActive);

			foreach (GameObject hit in hits)
				hit.SetActive(hitsActive);
		}

		public void ToggleHitsActive()
		{
			MDebugLog(nameof(ToggleHitsActive));

			hitsActive.ToggleValue();
		}

		public void OnGameStateChange()
		{
			MDebugLog(nameof(OnGameStateChange));

			gameActiveStateUIImage.color = MColorUtil.GetGreenOrRed(!gameState);
			gameStateText.text = gameState ? "RESET" : "START";
		}

		public void ToggleGameState()
		{
			MDebugLog(nameof(ToggleGameState));

			SetOwner();
			ResetGame();
			gameState.ToggleValue();

			if (gameState.Value)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TryTeleport));
			else
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TryRespawn));

			RequestSerialization();
		}

		public void ResetGame()
		{
			MDebugLog(nameof(ResetGame));

			for (int i = 0; i < pickups.Length; i++)
			{
				SetOwner(pickups[i].gameObject);
				pickups[i].Drop();
				objectSyncs[i].Respawn();
			}

			for (int i = 0; i < hitActives.Length; i++) hitActives[i].SetValue(false);
		}

		public void TryTeleport()
		{
			MDebugLog(nameof(TryTeleport));

			TeamType localTeamType = mTeamManager.GetTargetPlayerTeamType();

			if (localTeamType == TeamType.None)
				return;

			if (localTeamType == TeamType.A)
			{
				int playerIndex = mTeamManager.MTeams[0].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamATpPos[playerIndex].position, teamATpPos[playerIndex].rotation);
			}
			else if (localTeamType == TeamType.B)
			{
				int playerIndex = mTeamManager.MTeams[1].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamBTpPos[playerIndex].position, teamBTpPos[playerIndex].rotation);
			}
		}

		public void TryRespawn()
		{
			MDebugLog(nameof(TryRespawn));

			TeamType localTeamType = mTeamManager.GetTargetPlayerTeamType();

			if (localTeamType == TeamType.None)
				return;

			if (localTeamType == TeamType.A)
			{
				int playerIndex = mTeamManager.MTeams[0].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamARespawnTpPos[playerIndex].position,
					teamARespawnTpPos[playerIndex].rotation);
			}
			else if (localTeamType == TeamType.B)
			{
				int playerIndex = mTeamManager.MTeams[1].GetTargetPlayerIndex();

				if (playerIndex == NONE_INT)
					return;

				Networking.LocalPlayer.TeleportTo(teamBRespawnTpPos[playerIndex].position,
					teamBRespawnTpPos[playerIndex].rotation);
			}
		}
	}
}