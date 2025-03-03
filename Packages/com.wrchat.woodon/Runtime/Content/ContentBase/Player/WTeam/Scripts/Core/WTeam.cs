using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WTeam : WEventPublisher
	{
		[field: Header("_" + nameof(WTeam))]
		[field: SerializeField] public TeamType TeamType { get; private set; }
		[field: SerializeField] public UIWTeamButton[] TeamButtons { get; private set; }

		[SerializeField] private WTeamManager wTeamManager;
		
		public void Init(WTeamManager wTeamManager)
		{
			this.wTeamManager = wTeamManager;

			for (int i = 0; i < TeamButtons.Length; ++i)
				TeamButtons[i].Init(this);
		}

		public void PlayerChanged(UIWTeamButton teamButton)
		{
			WDebugLog($"{nameof(PlayerChanged)} : {teamButton}");

			SendEvents();
			wTeamManager.PlayerChanged(TeamType, teamButton);
		}

		public bool IsTargetPlayerTeam(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			foreach (UIWTeamButton teamButton in TeamButtons)
				if (teamButton.IsPlayer(targetPlayer))
					return true;

			return false;
		}

		public int GetTargetPlayerIndex(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			for (int i = 0; i < TeamButtons.Length; ++i)
				if (TeamButtons[i].IsPlayer(targetPlayer))
					return i;

			return NONE_INT;
		}
	}
}