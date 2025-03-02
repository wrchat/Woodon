using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WTeamManager : WEventPublisher
	{
		[field: Header("_" + nameof(WTeamManager))]
		[field: SerializeField] public WTeam[] WTeams { get; private set; }

		[SerializeField] private bool onlyOneTeam = true;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			foreach (WTeam wTeam in WTeams)
				wTeam.Init(this);
		}

		public void PlayerChanged(TeamType teamType, UIWTeamButton targetTeamButton)
		{
			WDebugLog(
				$"{nameof(PlayerChanged)} : {nameof(TeamType)} = {teamType}, {nameof(UIWTeamButton)} = {targetTeamButton}");

			if (targetTeamButton.WPlayer.TargetPlayerID == NONE_INT ||
				targetTeamButton.WPlayer.TargetPlayerID == Networking.LocalPlayer.playerId)
			{
				WDebugLog($"Invalid ID");
				return;
			}

			if (onlyOneTeam)
				foreach (WTeam mTeam in WTeams)
				{
					if (mTeam.TeamType == teamType)
						continue;

					foreach (UIWTeamButton teamButton in mTeam.TeamButtons)
						if (teamButton.IsPlayer())
							teamButton.WPlayer.SetTargetNone();
				}

			SendEvents();
		}

		public TeamType GetTargetPlayerTeamType(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			foreach (WTeam mTeam in WTeams)
				if (mTeam.IsTargetPlayerTeam(targetPlayer))
					return mTeam.TeamType;

			return TeamType.None;
		}
	}
}