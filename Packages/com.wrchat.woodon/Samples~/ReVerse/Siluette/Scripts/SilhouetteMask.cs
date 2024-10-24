using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.ReVerse
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SilhouetteMask : MBase
    {
        private const int SILHOUETTE_COUNT = 4;
        [SerializeField] private Image[] toggleButtonUIImages;
        [SerializeField] private Image[] silhouetteMaskImages;
        [SerializeField] private Sprite[] silhouetteMaskSprites;

        [UdonSynced] [FieldChangeCallback(nameof(SilhouetteMaskData))]
        private int silhouetteMaskData;

        private int SilhouetteMaskData
        {
            get => silhouetteMaskData;
            set
            {
                silhouetteMaskData = value;
                OnSilhouetteMaskDataChange();
            }
        }

        private void Start()
        {
            OnSilhouetteMaskDataChange();
        }

        private void OnSilhouetteMaskDataChange()
        {
            for (int bitNum = 0; bitNum < SILHOUETTE_COUNT; bitNum++)
            {
				bool silhouetteOn = (SilhouetteMaskData & (1 << bitNum)) == 1 << bitNum;

                toggleButtonUIImages[bitNum].color = silhouetteOn ? Color.green : Color.red;
                silhouetteMaskImages[bitNum].sprite = silhouetteOn ? null : silhouetteMaskSprites[bitNum];
            }
        }

        private void ToggleBit(int bitNum)
        {
            SetOwner();
            SilhouetteMaskData ^= 1 << bitNum;
            RequestSerialization();
        }

        public void ToggleBit0()
        {
            ToggleBit(0);
        }

        public void ToggleBit1()
        {
            ToggleBit(1);
        }

        public void ToggleBit2()
        {
            ToggleBit(2);
        }

        public void ToggleBit3()
        {
            ToggleBit(3);
        }

        public void OffAllBit()
        {
            SetOwner();
            SilhouetteMaskData = 0;
            RequestSerialization();
        }
    }
}