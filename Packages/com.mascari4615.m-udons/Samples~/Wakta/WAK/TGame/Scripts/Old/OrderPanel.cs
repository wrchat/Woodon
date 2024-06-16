
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class OrderPanel : MBase
    {
        [SerializeField] private OrderBox[] orderBoxes;
        public OrderBox[] OrderBoxes => orderBoxes;

        public void BoxUp(int boxIndex)
        {
            var curSiblingIndex = orderBoxes[boxIndex].transform.GetSiblingIndex();
            MDebugLog(curSiblingIndex.ToString());

            if (curSiblingIndex == 0)
                return;

            orderBoxes[boxIndex].transform.SetSiblingIndex(curSiblingIndex - 1);
        }

        public void BoxDown(int boxIndex)
        {
            var curSiblingIndex = orderBoxes[boxIndex].transform.GetSiblingIndex();
            MDebugLog(curSiblingIndex.ToString());

            if (curSiblingIndex == orderBoxes.Length - 1)
                return;

            orderBoxes[boxIndex].transform.SetSiblingIndex(curSiblingIndex + 1);
        }

        public string GetOrder()
        {
            var order = string.Empty;
            var parent = transform.GetChild(0);

            for (int i = 0; i < orderBoxes.Length; i++)
            {
                var childName = parent.GetChild(i).gameObject.name;
                foreach (var orderBox in orderBoxes)
                {
                    if (orderBox.name == childName)
                    {
                        order += orderBox.Index.ToString();
                        break;
                    }
                }
            }

            return order;
        }

        public void ResetOrder()
        {
            for (int i = 0; i < orderBoxes.Length; i++)
                orderBoxes[i].transform.SetSiblingIndex(i);
        }
    }
}