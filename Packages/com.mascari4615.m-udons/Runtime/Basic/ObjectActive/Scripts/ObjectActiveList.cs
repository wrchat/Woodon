using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActiveList : MBase
	{
		[Header("_" + nameof(ObjectActiveList))]
		[SerializeField] private MValue mValue;

		[SerializeField] private GameObject[] objectList;
		[SerializeField] private CanvasGroup[] canvasGroups;
		[SerializeField] private MPickup[] pickups;
		// [SerializeField] private Behaviour[] behaviours; // 240819 : U#에서 .enabled set을 지원하지 않음

		[Header("_" + nameof(ObjectActiveList) + " - Options")]
		[SerializeField] private ObjectActiveListOption option;
		[SerializeField] private int targetIndex = NONE_INT;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mValue != null)
			{
				int maxLen = objectList.Length;
				maxLen = Mathf.Max(maxLen, canvasGroups.Length);
				maxLen = Mathf.Max(maxLen, pickups.Length);

				mValue.SetMinMaxValue(0, maxLen - 1);
				mValue.RegisterListener(this, nameof(UpdateObjectByValue));
			}

			UpdateObjectByValue();
		}

		[ContextMenu(nameof(UpdateObjectByValue))]
		public void UpdateObjectByValue()
		{
			if (mValue)
				SetObject(mValue.Value);
		}

		public void SetObject(int value)
		{
			// MValue를 리스트의 인덱스로 사용할 때
			// 예를 들어, MValue가 0일 때 리스트의 0번째 오브젝트만 활성화, 나머지는 비활성화
			if (option == ObjectActiveListOption.UseMValueAsListIndex)
			{
				for (int i = 0; i < objectList.Length; i++)
				{
					if (objectList[i])
						objectList[i].SetActive(i == value);
				}

				for (int i = 0; i < canvasGroups.Length; i++)
				{
					if (canvasGroups[i])
					{
						canvasGroups[i].alpha = i == value ? 1 : 0;
						canvasGroups[i].blocksRaycasts = i == value;
						canvasGroups[i].interactable = i == value;
					}
				}

				for (int i = 0; i < pickups.Length; i++)
				{
					if (pickups[i])
						pickups[i].SetEnabled(i == value);
				}
			}
			// MValue를 타겟 인덱스로 사용할 때
			// 예를 들어, MValue가 0일 때 타겟 인덱스가 0이면 리스트의 모든 오브젝트 활성화, 아니면 비활성화
			else if (option == ObjectActiveListOption.UseMValueAsTargetIndex)
			{
				bool isTargetIndex = value == targetIndex;

				foreach (GameObject obj in objectList)
				{
					if (obj)
						obj.SetActive(isTargetIndex);
				}

				foreach (CanvasGroup canvasGroup in canvasGroups)
				{
					if (canvasGroup)
					{
						canvasGroup.alpha = isTargetIndex ? 1 : 0;
						canvasGroup.blocksRaycasts = isTargetIndex;
						canvasGroup.interactable = isTargetIndex;
					}
				}

				foreach (MPickup pickup in pickups)
				{
					if (pickup)
						pickup.SetEnabled(isTargetIndex);
				}
			}
		}

		#region HorribleEvents
		[ContextMenu(nameof(SetObject0))]
		public void SetObject0() => SetObject(0);
		public void SetObject1() => SetObject(1);
		public void SetObject2() => SetObject(2);
		public void SetObject3() => SetObject(3);
		public void SetObject4() => SetObject(4);
		public void SetObject5() => SetObject(5);
		public void SetObject6() => SetObject(6);
		public void SetObject7() => SetObject(7);
		public void SetObject8() => SetObject(8);
		public void SetObject9() => SetObject(9);
		#endregion
	}
}