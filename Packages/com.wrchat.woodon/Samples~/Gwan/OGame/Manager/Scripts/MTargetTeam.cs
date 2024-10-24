using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MTargetTeam : MEventSender
    {
        [Header("_" + nameof(MTargetTeam))]
        public TeamType TeamType;
        public MTarget[] mTargets;

        [SerializeField] private MTargetTeamManager mTargetTeamManager;

        public void PlayerChanged(int index)
        {
            MDebugLog($"{nameof(PlayerChanged)} : Index = {index}");

            SendEvents();
            mTargetTeamManager.PlayerChanged(TeamType, index, mTargets[index].TargetPlayerID);
        }

        public bool IsTargetPlayerTeam(VRCPlayerApi targetPlayer = null)
        {
            if (targetPlayer == null)
                targetPlayer = Networking.LocalPlayer;

            foreach (MTarget mPlayerSync in mTargets)
                if (mPlayerSync.TargetPlayerID == targetPlayer.playerId)
                    return true;

            return false;
        }

        public int GetTargetPlayerIndex(VRCPlayerApi targetPlayer = null)
        {
            if (targetPlayer == null)
                targetPlayer = Networking.LocalPlayer;

            for (int i = 0; i < mTargets.Length; i++)
                if (mTargets[i].TargetPlayerID == targetPlayer.playerId)
                    return i;

            return NONE_INT;
        }

        public void PlayerChanged0()
        {
            PlayerChanged(0);
        }

        public void PlayerChanged1()
        {
            PlayerChanged(1);
        }

        public void PlayerChanged2()
        {
            PlayerChanged(2);
        }

        public void PlayerChanged3()
        {
            PlayerChanged(3);
        }

        public void PlayerChanged4()
        {
            PlayerChanged(4);
        }

        public void PlayerChanged5()
        {
            PlayerChanged(5);
        }

        public void PlayerChanged6()
        {
            PlayerChanged(6);
        }

        public void PlayerChanged7()
        {
            PlayerChanged(7);
        }

        public void PlayerChanged8()
        {
            PlayerChanged(8);
        }

        public void PlayerChanged9()
        {
            PlayerChanged(9);
        }
    }
}