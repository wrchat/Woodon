using TMPro;
using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class FCFS : MBase
    {
        [SerializeField] private string textFormat;
        [SerializeField] private TextMeshProUGUI resultText;
        private FCFS_LocalData[] fcfsDatas;
        private int[][] sortedFCFS_Datas;

        private void Start()
        {
            fcfsDatas = GameObject.Find(nameof(PlayerOwnUdonManager)).GetComponentsInChildren<FCFS_LocalData>();

            sortedFCFS_Datas = new int[fcfsDatas.Length][];
            for (int i = 0; i < fcfsDatas.Length; i++)
                sortedFCFS_Datas[i] = new int[2];

            textFormat += "\n";
        }

        private void Update()
        {
            if (fcfsDatas == null || fcfsDatas.Length == 0)
                return;

            UpdateText();
        }

        private void UpdateText()
        {
            for (int i = 0; i < fcfsDatas.Length; i++)
            {
                sortedFCFS_Datas[i][0] = i;
                sortedFCFS_Datas[i][1] = fcfsDatas[i].MyTimeByMilliseconds;
            }

            // FCFS_LocalData[] sortedFCFS_Datas = fcfsDatas;
            QuickSort(ref sortedFCFS_Datas, 0, fcfsDatas.Length - 1);

			string newText = string.Empty;
            // newText += Networking.GetServerTimeInMilliseconds() + "\n";

            for (int i = 0; i < fcfsDatas.Length; i++)
            {
                if (fcfsDatas[sortedFCFS_Datas[i][0]].MyTimeByMilliseconds == NONE_INT)
                    continue;

                newText += string.Format(textFormat, sortedFCFS_Datas[i][0],
                    fcfsDatas[sortedFCFS_Datas[i][0]].MyTimeByMilliseconds);
            }

            newText.TrimEnd('\n', ' ');
            resultText.text = newText;
        }

        private void QuickSort(ref int[][] list, int start, int end)
        {
            // 원소가 1개인 경우 그대로 리턴
            if (start >= end)
                return;

			// 피봇은 첫 번째 원소
			int pivot = start;
			int i = start + 1;
			int j = end;

            // 엇갈릴 때까지 반복
            while (i <= j)
            {
                // 피봇 값보다 큰 값을 만날 때까지
                while (i <= end && list[i][1] <= list[pivot][1])
                    i++;

                // 피봇 값보다 작은 값을 만날 떄까지
                while (j > start && list[j][1] >= list[pivot][1])
                    j--;

                if (i > j)
                {
					int[] temp = list[j];
                    list[j] = list[pivot];
                    list[pivot] = temp;
                }
                else
                {
					int[] temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }

            QuickSort(ref list, start, j - 1);
            QuickSort(ref list, j + 1, end);
        }
    }
}