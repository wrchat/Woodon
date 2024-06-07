using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTeamManager : MEventSender
	{
		[Header("_" + nameof(MTeamManager))]
		[SerializeField]
		private MTeam[] mTeams;
		public MTeam[] MTeams => mTeams;

		[SerializeField] private bool onlyOneTeam = true;

		public void PlayerChanged(TeamType teamType, int playerIndex, int playerID)
		{
			MDebugLog(
				$"{gameObject.name}, {nameof(PlayerChanged)} : {nameof(TeamType)} = {teamType}, {nameof(playerIndex)} = {playerIndex}, {nameof(playerID)} = {playerID}, {Networking.LocalPlayer.playerId}");

			if (playerID == NONE_INT)
			{
				MDebugLog("playerID == MPlayerSync.NOT_SELECTED");
				return;
			}

			if (playerID != Networking.LocalPlayer.playerId)
			{
				MDebugLog("playerID != Networking.LocalPlayer.playerId");
				return;
			}

			if (onlyOneTeam)
				foreach (var mTeam in mTeams)
					for (var i = 0; i < mTeam.mPlayerSyncs.Length; i++)
					{
						if (mTeam.TeamType == teamType && i == playerIndex)
							continue;

						if (mTeam.mPlayerSyncs[i].PlayerID == playerID)
							mTeam.mPlayerSyncs[i].ClearPlayer();
					}

			SendEvents();
		}

		public TeamType GetTargetPlayerTeamType(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			foreach (var mTeam in mTeams)
				if (mTeam.IsTargetPlayerTeam(targetPlayer))
					return mTeam.TeamType;

			return TeamType.None;
		}
	}
}