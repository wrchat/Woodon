using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTeam : MEventSender
	{
		[field: Header("_" + nameof(MTeam))]
		[field: SerializeField] public TeamType TeamType { get; private set; }
		[field: SerializeField] public UIMTeamButton[] TeamButtons { get; private set; }

		[SerializeField] private MTeamManager mTeamManager;
		
		public void Init(MTeamManager mTeamManager)
		{
			this.mTeamManager = mTeamManager;

			for (int i = 0; i < TeamButtons.Length; ++i)
				TeamButtons[i].Init(this);
		}

		public void PlayerChanged(UIMTeamButton teamButton)
		{
			MDebugLog($"{nameof(PlayerChanged)} : {teamButton}");

			SendEvents();
			mTeamManager.PlayerChanged(TeamType, teamButton);
		}

		public bool IsTargetPlayerTeam(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			foreach (UIMTeamButton teamButton in TeamButtons)
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