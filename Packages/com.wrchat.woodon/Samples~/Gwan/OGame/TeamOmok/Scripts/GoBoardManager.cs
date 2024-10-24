using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class GoBoardManager : MBase
    {
        [Header("_" + nameof(GoBoardManager))]
        [SerializeField] private Camera screenCamera;
        [SerializeField] private GameObject[] blackBlinds;
        [SerializeField] private GameObject[] whiteBlinds;
        [SerializeField] private Transform[] blackRoomPos;
        [SerializeField] private Transform[] whiteRoomPos;
        [SerializeField] private Transform stagePos;

        [SerializeField] private MTargetTeamManager teamManager;
        [SerializeField] private Timer timeEvent;

        [SerializeField] private TextMeshProUGUI curPlayingPlayerNameText;


        [SerializeField] private TextMeshProUGUI curPlayerIDText;
        [UdonSynced] [FieldChangeCallback(nameof(CurPlayingPlayerID))]
        private int curPlayingPlayerID = NONE_INT;

        [SerializeField] private GoBoardLine[] goBoardLines;

        private int CurPlayingPlayerID
        {
            get => curPlayingPlayerID;
            set
            {
                curPlayingPlayerID = value;
                OnCurPlayingPlayerChange();
            }
        }

        private void Start()
        {
            for (int i = 0; i < goBoardLines.Length; i++)
                goBoardLines[i].transform.localPosition = Vector3.up * .25f + .5f * i * Vector3.forward;
            screenCamera.enabled = true;
            screenCamera.gameObject.SetActive(true);
            OnCurPlayingPlayerChange();
            SetBlind();
        }

        private void OnCurPlayingPlayerChange()
        {
            MDebugLog(nameof(OnCurPlayingPlayerChange));

            curPlayerIDText.text = CurPlayingPlayerID.ToString();
			VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(curPlayingPlayerID);
            curPlayingPlayerNameText.text = targetPlayer == null ? "-" : targetPlayer.displayName;
        }

        public void SetBlind()
        {
			TeamType teamType = teamManager.GetTargetPlayerTeamType();
            MDebugLog($"{nameof(SetBlind)} : teamType = {teamType}");

            if (teamType == TeamType.None)
            {
                for (int i = 0; i < blackBlinds.Length; i++)
                    blackBlinds[i].SetActive(false);

                for (int i = 0; i < whiteBlinds.Length; i++)
                    whiteBlinds[i].SetActive(false);
            }
            else if (teamType == TeamType.A)
            {
                for (int i = 0; i < blackBlinds.Length; i++)
                    blackBlinds[i].SetActive(teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex() == i);

                for (int i = 0; i < whiteBlinds.Length; i++)
                    whiteBlinds[i].SetActive(false);
            }
            else
            {
                for (int i = 0; i < blackBlinds.Length; i++)
                    blackBlinds[i].SetActive(false);

                for (int i = 0; i < whiteBlinds.Length; i++)
                    whiteBlinds[i].SetActive(teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex() == i);
            }
        }

        public void PlayerToStage(TeamType teamType, int index)
        {
            int targetPlayerID = teamManager.MTargetTeams[(int)teamType].mTargets[index].TargetPlayerID;
            VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(targetPlayerID);

            if (targetPlayer == null)
                // if (targetPlayer == null)
                return;

            if (targetPlayer != Networking.LocalPlayer)
                return;

            SetOwner();

            CurPlayingPlayerID = targetPlayerID;
            timeEvent.StartTimer();

            targetPlayer.TeleportTo(stagePos.position, stagePos.rotation);
            //targetPlayer.TeleportTo(stagePos.position, stagePos.rotation);

            RequestSerialization();
        }

        public void ReturnCurPlayingPlayer()
        {
            MDebugLog(nameof(ReturnCurPlayingPlayer));

            if (curPlayingPlayerID != Networking.LocalPlayer.playerId)
                return;

			TeamType teamType = teamManager.GetTargetPlayerTeamType();

            if (teamType == TeamType.None)
                return;

			Transform tpPos = teamType == TeamType.A
                ? blackRoomPos[teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex()]
                : whiteRoomPos[teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex()];

            SetOwner();
            if (timeEvent.ExpireTime != NONE_INT)
                timeEvent.ResetTimer();
            CurPlayingPlayerID = NONE_INT;
            RequestSerialization();
            
            Networking.LocalPlayer.TeleportTo(tpPos.position, tpPos.rotation);
        }

        public void ReturnCurPlayingPlayer_Global()
        {
            MDebugLog(nameof(ReturnCurPlayingPlayer_Global));
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ReturnCurPlayingPlayer));
        }
        
        public void ReturnAllPlayer_Global()
        {
            MDebugLog(nameof(ReturnAllPlayer_Global));
            
            SetOwner();
            if (timeEvent.ExpireTime != NONE_INT)
                timeEvent.ResetTimer();
            CurPlayingPlayerID = NONE_INT;
            RequestSerialization();

            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ReturnAllPlayer));
        }

        public void ReturnAllPlayer()
        {
            MDebugLog(nameof(ReturnCurPlayingPlayer));

			TeamType teamType = teamManager.GetTargetPlayerTeamType();
            if (teamType == TeamType.None)
                return;
            MDebugLog(teamType.ToString());
            MDebugLog(teamManager.MTargetTeams[(int)teamType].ToString());
            MDebugLog(teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex().ToString());

			Transform tpPos = (teamType == TeamType.A
                ? blackRoomPos
                : whiteRoomPos)[teamManager.MTargetTeams[(int)teamType].GetTargetPlayerIndex()];

            Networking.LocalPlayer.TeleportTo(tpPos.position, tpPos.rotation);
        }

        public void Black0_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Black0));
        }

        public void Black0()
        {
            PlayerToStage(TeamType.A, 0);
        }

        public void Black1_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Black1));
        }

        public void Black1()
        {
            PlayerToStage(TeamType.A, 1);
        }

        public void Black2_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Black2));
        }

        public void Black2()
        {
            PlayerToStage(TeamType.A, 2);
        }

        public void White0_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(White0));
        }

        public void White0()
        {
            PlayerToStage(TeamType.B, 0);
        }

        public void White1_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(White1));
        }

        public void White1()
        {
            PlayerToStage(TeamType.B, 1);
        }

        public void White2_Global()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(White2));
        }

        public void White2()
        {
            PlayerToStage(TeamType.B, 2);
        }

        public void ResetBoard()
        {
            foreach (GoBoardLine goBoardLine in goBoardLines)
                goBoardLine.ResetStoneData();
        }
    }
}