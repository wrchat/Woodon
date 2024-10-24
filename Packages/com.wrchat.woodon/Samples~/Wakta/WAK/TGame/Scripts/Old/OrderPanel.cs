using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class OrderPanel : MBase
    {
        [SerializeField] private OrderBox[] orderBoxes;
        public OrderBox[] OrderBoxes => orderBoxes;

        public void BoxUp(int boxIndex)
        {
			int curSiblingIndex = orderBoxes[boxIndex].transform.GetSiblingIndex();
            MDebugLog(curSiblingIndex.ToString());

            if (curSiblingIndex == 0)
                return;

            orderBoxes[boxIndex].transform.SetSiblingIndex(curSiblingIndex - 1);
        }

        public void BoxDown(int boxIndex)
        {
			int curSiblingIndex = orderBoxes[boxIndex].transform.GetSiblingIndex();
            MDebugLog(curSiblingIndex.ToString());

            if (curSiblingIndex == orderBoxes.Length - 1)
                return;

            orderBoxes[boxIndex].transform.SetSiblingIndex(curSiblingIndex + 1);
        }

        public string GetOrder()
        {
			string order = string.Empty;
			Transform parent = transform.GetChild(0);

            for (int i = 0; i < orderBoxes.Length; i++)
            {
				string childName = parent.GetChild(i).gameObject.name;
                foreach (OrderBox orderBox in orderBoxes)
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