using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActiveList : ActiveList
	{
		[Header("_" + nameof(ObjectActiveList))]
		[SerializeField] private GameObject[] objectList;
		// [SerializeField] private Behaviour[] behaviours; // 240819 : U#에서 .enabled set을 지원하지 않음

		protected override void InitMValueMinMax()
		{
			int maxLen = Mathf.Max(mValue.MaxValue, objectList.Length - 1);
			mValue.SetMinMaxValue(mValue.MinValue, maxLen);
		}

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}({Value})");

			switch (option)
			{
				// Value를 리스트의 인덱스로 사용할 때
				// 예를 들어, Value가 0일 때 리스트의 0번째 오브젝트만 활성화, 나머지는 비활성화
				case ActiveListOption.UseValueAsListIndex:
					for (int i = 0; i < objectList.Length; i++)
					{
						if (objectList[i])
							objectList[i].SetActive(i == Value);
					}
					break;

				// Value를 타겟 인덱스로 사용할 때
				// 예를 들어, Value가 0일 때 타겟 인덱스가 0이면 리스트의 모든 오브젝트 활성화, 아니면 비활성화
				case ActiveListOption.UseValueAsTargetIndex:
					bool isTargetIndex = Value == targetIndex;

					foreach (GameObject obj in objectList)
					{
						if (obj)
							obj.SetActive(isTargetIndex);
					}
					break;
				
				default:
					MDebugLog($"{nameof(UpdateActive)}({Value}) - {option}, Invalid Option");
					break;
			}
		}
	}
}