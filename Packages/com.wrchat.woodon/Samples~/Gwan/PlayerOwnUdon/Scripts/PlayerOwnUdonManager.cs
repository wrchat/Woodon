using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class PlayerOwnUdonManager : MBase
	{
		[SerializeField] private PlayerOwnUdon[] playerOwnUdons;

		public PlayerOwnUdon[] PlayerOwnUdons => playerOwnUdons;
		public bool AllSynced { get; private set; }

		public PlayerOwnUdon LocalUdon { get; private set; }

		private void Start()
		{
			ForceClearRepeat();

			if (Networking.IsMaster)
				foreach (PlayerOwnUdon playerOwnUdon in playerOwnUdons)
					playerOwnUdon.Init();
		}

		private void Update()
		{
			if (AllSynced)
				return;

			for (int i = 0; i < playerOwnUdons.Length; i++)
				if (playerOwnUdons[i].isSynced == false)
					return;

			AllSynced = true;
			LocalUdon = SetLocalUdon();
		}

		private PlayerOwnUdon SetLocalUdon()
		{
			if (!AllSynced)
				return null;

			foreach (PlayerOwnUdon playerOwnUdon in playerOwnUdons)
				if (playerOwnUdon.OwnerID == NONE_INT)
				{
					playerOwnUdon.SetStatus(Networking.LocalPlayer.playerId, NONE_STRING);
					return playerOwnUdon;
				}

			return null;
		}

		public void ForceClearRepeat()
		{
			// SendCustomEventDelayedSeconds(nameof(ForceClearRepeat), 30f);
		}
	}
}