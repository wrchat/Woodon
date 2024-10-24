using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class FCFS_Team : MBase
    {
        [SerializeField] private MTeamManager teamManager;
        [SerializeField] private PlayerOwnUdonManager playerOwnUdonManager;
        [SerializeField] private TextMeshProUGUI[] teamTimeTexts;
        [SerializeField] private Image[] teamTimeTextBackgrounds;

        [SerializeField] private Color black;
        [SerializeField] private Color white;
        [SerializeField] private Color firstColor;

        [SerializeField] private string noneComment = "-";
        [SerializeField] private string firstComment = "First !";
        [SerializeField] private string elseFormat = "+ {0:0.00}초";
        private FCFS_LocalData[] fcfsDatas;

        private int[] teamTimes;

        private void Start()
        {
            fcfsDatas = playerOwnUdonManager.GetComponentsInChildren<FCFS_LocalData>();
            teamTimes = new int[teamManager != null ? teamManager.MTeams.Length : 10];
            InitTeamTimes();
        }

        private void Update()
        {
            CalcTime();
        }

        private void InitTeamTimes()
        {
            if (DEBUG)
                Debug.Log(nameof(InitTeamTimes));

            for (int i = 0; i < teamTimes.Length; i++)
                teamTimes[i] = NONE_INT;
        }

        public void RecordTime0()
        {
            RecordTime(0);
        }

        public void RecordTime1()
        {
            RecordTime(1);
        }

        public void RecordTime2()
        {
            RecordTime(2);
        }

        public void RecordTime3()
        {
            RecordTime(3);
        }

        public void RecordTime4()
        {
            RecordTime(4);
        }

        public void RecordTime5()
        {
            RecordTime(5);
        }

        public void RecordTime6()
        {
            RecordTime(6);
        }

        public void RecordTime7()
        {
            RecordTime(7);
        }

        public void RecordTime8()
        {
            RecordTime(8);
        }

        public void RecordTime9()
        {
            RecordTime(9);
        }

        public void RecordTime(int teamIndex)
        {
            if (DEBUG)
                Debug.Log($"{nameof(RecordTime)} : {nameof(teamIndex)} = {teamIndex}");

			PlayerOwnUdon localUdon = playerOwnUdonManager.LocalUdon;

            if (localUdon == null)
            {
                if (DEBUG)
                    Debug.Log($"{nameof(RecordTime)} : No Udon");
                return;
            }

            fcfsDatas[localUdon.Index].RecordCurTime_(true, teamIndex.ToString());
        }

        public void RecordTime_()
        {
            if (DEBUG)
                Debug.Log($"{nameof(RecordTime_)}");

			PlayerOwnUdon localUdon = playerOwnUdonManager.LocalUdon;

            if (localUdon == null)
            {
                if (DEBUG)
                    Debug.Log($"{nameof(RecordTime_)} : No Udon");
                return;
            }

            localUdon.SendEvnetToChildUdons(nameof(FCFS_LocalData.RecordCurTimeOnce));
        }

        public void ClearTime_Global()
        {
            if (DEBUG)
                Debug.Log(nameof(ClearTime_Global));
            InitTeamTimes();
        }

        public void ClearTime()
        {
            if (DEBUG)
                Debug.Log(nameof(ClearTime));
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ClearTime_Global));

			PlayerOwnUdon localUdon = playerOwnUdonManager.LocalUdon;

            if (localUdon == null)
            {
                if (DEBUG)
                    Debug.Log($"{nameof(ClearTime)} : No Udon");
                return;
            }

            localUdon.SendEvnetToChildUdons(nameof(FCFS_LocalData.ClearTime));

            foreach (FCFS_LocalData i in fcfsDatas)
                i.ForceClearTime();
        }

        private void CalcTime()
        {
            for (int i = 0; i < fcfsDatas.Length; i++)
            {
				int time = fcfsDatas[i].MyTimeByMilliseconds;

                if (time == NONE_INT)
                    //if (debug)
                    //	Debug.Log("A");
                    continue;

                if (fcfsDatas[i].PlayerOwnUdon == null)
                    //if (debug)
                    //	Debug.Log("B");
                    continue;

				int playerID = fcfsDatas[i].PlayerOwnUdon.OwnerID;

                if (playerID == NONE_INT)
                    //if (debug)
                    //	Debug.Log("C");
                    continue;

				VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(playerID);

                if (targetPlayer == null)
                    //if (debug)
                    //	Debug.Log("D");
                    continue;

                if (teamManager != null)
                {
					TeamType teamType = teamManager.GetTargetPlayerTeamType(targetPlayer);

                    if (teamType == TeamType.None)
                        //if (debug)
                        //	Debug.Log("E");
                        continue;
                    ;

                    teamTimes[(int)teamType] = time;
                }
                else
                {
                    teamTimes[int.Parse(fcfsDatas[i].SubData)] = time;
                }
            }

			int minTime = int.MaxValue;

            for (int i = 0; i < teamTimes.Length; i++)
            {
                if (teamTimes[i] == NONE_INT)
                    continue;

                if (teamTimes[i] < minTime)
                    minTime = teamTimes[i];
            }

            // Debug.Log($"{nameof(Update)} : {nameof(minTime)} = {minTime}");

            for (int i = 0; i < teamTimeTexts.Length; i++)
            {
                teamTimeTexts[i].text =
                    teamTimes[i] == NONE_INT ? noneComment :
                    teamTimes[i] - minTime == 0 ? firstComment :
                    string.Format(elseFormat, (teamTimes[i] - minTime) * .001f);

                teamTimeTexts[i].color =
                    teamTimes[i] == NONE_INT ? white :
                    teamTimes[i] - minTime == 0 ? white :
                    black;

                teamTimeTextBackgrounds[i].color =
                    teamTimes[i] == NONE_INT ? black :
                    teamTimes[i] - minTime == 0 ? firstColor :
                    white;
            }
        }
    }
}