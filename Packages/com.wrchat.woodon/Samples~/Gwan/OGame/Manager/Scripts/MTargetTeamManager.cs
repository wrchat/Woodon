using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615
{
    public class MTargetTeamManager : MEventSender
    {
        [Header("_" + nameof(MTeamManager))] [SerializeField]
        private MTargetTeam[] mTargetTeams;

        public MTargetTeam[] MTargetTeams => mTargetTeams;

        [SerializeField] private bool onlyOneTeam = true;

        public void PlayerChanged(TeamType teamType, int playerIndex, int playerID)
        {
            MDebugLog(
                $"{gameObject.name}, {nameof(PlayerChanged)} : {nameof(TeamType)} = {teamType}, {nameof(playerIndex)} = {playerIndex}, {nameof(playerID)} = {playerID}, {Networking.LocalPlayer.playerId}");

            if (playerID == NONE_INT)
            {
                MDebugLog("mTarget.CurTargetPlayerID == None");
                return;
            }

            if (playerID != Networking.LocalPlayer.playerId)
            {
                MDebugLog("mTarget.CurTargetPlayerID != Networking.LocalPlayer.playerId");
                return;
            }

            if (onlyOneTeam)
                foreach (MTargetTeam mTeam in mTargetTeams)
                    for (int i = 0; i < mTeam.mTargets.Length; i++)
                    {
                        if (mTeam.TeamType == teamType && i == playerIndex)
                            continue;

                        if (mTeam.mTargets[i].TargetPlayerID == playerID)
                            mTeam.mTargets[i].SetTargetNone();
                    }

            SendEvents();
        }

        public TeamType GetTargetPlayerTeamType(VRCPlayerApi targetPlayer = null)
        {
            if (targetPlayer == null)
                targetPlayer = Networking.LocalPlayer;

            foreach (MTargetTeam mTeam in mTargetTeams)
                if (mTeam.IsTargetPlayerTeam(targetPlayer))
                    return mTeam.TeamType;

            return TeamType.None;
        }
    }
}