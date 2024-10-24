using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class OrderBox : MBase
    {
        [SerializeField] private int index;
        [SerializeField] private OrderPanel orderPanel;
        [SerializeField] private TextMeshProUGUI boxText;
        [SerializeField] private Image playerImage;

        public int Index => index;

        public void SetBoxText(string newText, Sprite sprite)
        {
            boxText.text = newText;
            playerImage.sprite = sprite;
        }

        public void Up()
        {
            orderPanel.BoxUp(index);
        }

        public void Down()
        {
            orderPanel.BoxDown(index);
        }
    }
}