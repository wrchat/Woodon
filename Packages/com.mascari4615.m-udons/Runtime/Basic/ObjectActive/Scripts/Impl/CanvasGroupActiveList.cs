using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActiveList : ActiveList
	{
		[Header("_" + nameof(CanvasGroupActiveList))]
		[SerializeField] private CanvasGroup[] canvasGroups;

		protected override void InitMValueMinMax()
		{
			mValue.SetMinMaxValue(0, canvasGroups.Length - 1);
		}

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}({Value})");

			switch (option)
			{
				case ActiveListOption.UseMValueAsListIndex:
					for (int i = 0; i < canvasGroups.Length; i++)
					{
						canvasGroups[i].alpha = i == Value ? 1 : 0;
						canvasGroups[i].blocksRaycasts = i == Value;
						canvasGroups[i].interactable = i == Value;
					}

					break;
				case ActiveListOption.UseMValueAsTargetIndex:
					bool isTargetIndex = Value == targetIndex;
					foreach (CanvasGroup canvasGroup in canvasGroups)
					{
						canvasGroup.alpha = isTargetIndex ? 1 : 0;
						canvasGroup.blocksRaycasts = isTargetIndex;
						canvasGroup.interactable = isTargetIndex;
					}
					break;
				default:
					MDebugLog($"{nameof(UpdateActive)}({Value}) - {option}, Invalid Option");
					break;
			}
		}
	}
}