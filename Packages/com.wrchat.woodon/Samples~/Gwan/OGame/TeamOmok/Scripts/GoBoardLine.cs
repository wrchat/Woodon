using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class GoBoardLine : MBase
    {
        [SerializeField] private GoBoardButton[] goBoardButtons;

        [UdonSynced, FieldChangeCallback(nameof(StoneDataPack))] private string stoneDataPack = "0000000000000000000";
        private string StoneDataPack
        {
            get => stoneDataPack;
            set
            {
                stoneDataPack = value;
                
                if (stoneDataPack.Length == 19)
                    for (int i = 0; i < 19; i++)
                        stoneData[i] = int.Parse(stoneDataPack[i].ToString());
            }
        }
        
        private int[] stoneData = new int[19];

        private void Start()
        {
            MDebugLog($"{nameof(goBoardButtons)} {goBoardButtons.Length}");

            for (int i = 0; i < goBoardButtons.Length; i++)
                goBoardButtons[i].transform.localPosition = Vector3.right * i * .5f;

            if (Networking.IsMaster)
                ResetStoneData();
        }

        private void Update()
        {
            if (stoneData == null)
                return;

            if (stoneData.Length == 0)
                return;

            if (goBoardButtons.Length != stoneData.Length)
                return;

            for (int i = 0; i < goBoardButtons.Length; i++)
                goBoardButtons[i].SetState(stoneData[i]);
        }

        public void SetStoneData(int index, int color)
        {
            MDebugLog($"{nameof(SetStoneData)}, ButtonIndex = {index}, Color = {color}");

            stoneData[index] = color;

            string newDataPack = string.Empty;
            for (int i = 0; i < 19; i++)
                newDataPack += stoneData[i].ToString();
            
            SetOwner();
            StoneDataPack = newDataPack;
            RequestSerialization();
        }

        public void ResetStoneData()
        {
            MDebugLog(nameof(ResetStoneData));
            
            SetOwner();
            stoneDataPack = "0000000000000000000";
            RequestSerialization();
        }
    }
}