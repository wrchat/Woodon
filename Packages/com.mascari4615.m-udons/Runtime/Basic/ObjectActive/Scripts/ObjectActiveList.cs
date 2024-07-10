using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActiveList : MBase
	{
		[Header("_" + nameof(ObjectActiveList))]
		[SerializeField] private MScore mScore;
		
		[SerializeField] private GameObject[] objectList;
		[SerializeField] private CanvasGroup[] canvasGroups;
		[SerializeField] private MPickup[] pickups;

		private void Start()
		{
			int maxLen = objectList.Length;
			maxLen = Mathf.Max(maxLen, canvasGroups.Length);
			maxLen = Mathf.Max(maxLen, pickups.Length);

			mScore.SetMinMaxValue(0, maxLen - 1);
		}

		[ContextMenu(nameof(UpdateObjectByScore))]
		public void UpdateObjectByScore()
		{
			if (mScore)
				SetObject(mScore.Value);
		}

		public void SetObject(int targetIndex)
		{
			for (int i = 0; i < objectList.Length; i++)
			{
				if (objectList[i])
					objectList[i].SetActive(i == targetIndex);
			}

			for (int i = 0; i < canvasGroups.Length; i++)
			{
				if (canvasGroups[i])
				{
					canvasGroups[i].alpha = i == targetIndex ? 1 : 0;
					canvasGroups[i].blocksRaycasts = i == targetIndex;
					canvasGroups[i].interactable = i == targetIndex;
				}
			}

			for (int i = 0; i < pickups.Length; i++)
			{
				if (pickups[i])
					pickups[i].SetEnabled(i == targetIndex);
			}
		}

		#region HorribleEvents
		[ContextMenu(nameof(SetObject0))]
		public void SetObject0() => SetObject(0);
		public void SetObject1() => SetObject(1);
		public void SetObject2() => SetObject(2);
		#endregion
	}
}