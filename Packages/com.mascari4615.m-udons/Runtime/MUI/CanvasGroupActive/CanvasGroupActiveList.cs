using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActiveList : MBase
	{
		[Header("_" + nameof(CanvasGroupActiveList))]
		[SerializeField] private CanvasGroup[] canvasGroups;
		[SerializeField] private MScore mScore;

		private void Start()
		{
			mScore.SetMinMaxValue(0, canvasGroups.Length - 1);
		}

		[ContextMenu(nameof(UpdateCanvasGroupByScore))]
		public void UpdateCanvasGroupByScore()
		{
			if (mScore)
				SetCanvasGroup(mScore.Value);
		}

		public void SetCanvasGroup(int targetIndex)
		{
			for (int i = 0; i < canvasGroups.Length; i++)
			{
				canvasGroups[i].alpha = i == targetIndex ? 1 : 0;
				canvasGroups[i].blocksRaycasts = i == targetIndex;
				canvasGroups[i].interactable = i == targetIndex;
			}
		}

		[ContextMenu(nameof(SetCanvasGroup0))]
		public void SetCanvasGroup0() => SetCanvasGroup(0);

		[ContextMenu(nameof(SetCanvasGroup1))]
		public void SetCanvasGroup1() => SetCanvasGroup(1);

		[ContextMenu(nameof(SetCanvasGroup2))]
		public void SetCanvasGroup2() => SetCanvasGroup(2);
	}
}