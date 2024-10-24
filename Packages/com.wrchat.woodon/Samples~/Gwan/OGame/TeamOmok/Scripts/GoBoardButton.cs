using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GoBoardButton : MBase
    {
        [SerializeField] private int buttonIndex;
        [SerializeField] private GoBoardLine goBoardLine;

        [SerializeField] private GameObject blackStone;
        [SerializeField] private GameObject whiteStone;
        private int curState;

        public override void Interact()
        {
            /*switch (Networking.LocalPlayer.GetPlayerTag("LOCALPLAYER_TEAM"))
            {
                case "BLACK":
                    goBoardLine.SetStoneData(buttonIndex, (curState != TeamColor.None) ? TeamColor.Black);
                    break;
                case "WHITE":
                    goBoardLine.SetStoneData(buttonIndex, TeamColor.White);
                    break;
                default:
                    goBoardLine.SetStoneData(buttonIndex, TeamColor.None);
                    break;
            }*/

            switch (curState)
            {
                case 0:
                    goBoardLine.SetStoneData(buttonIndex, 1);
                    break;
                case 1:
                    goBoardLine.SetStoneData(buttonIndex, 2);
                    break;
                case 2:
                    goBoardLine.SetStoneData(buttonIndex, 0);
                    break;
            }
        }

        public void SetState(int state)
        {
            curState = state;
            blackStone.SetActive(state == 1);
            whiteStone.SetActive(state == 2);
        }
    }
}