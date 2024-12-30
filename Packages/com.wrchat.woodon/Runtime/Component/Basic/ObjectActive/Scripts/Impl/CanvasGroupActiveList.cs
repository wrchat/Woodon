using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActiveList : ActiveList
	{
		[Header("_" + nameof(CanvasGroupActiveList))]
		[SerializeField] private CanvasGroup[] canvasGroups;

		protected override void InitMValueMinMax()
		{
			// mValue.SetMinMaxValue(0, canvasGroups.Length - 1);
		}

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}({Value})");

			switch (option)
			{
				case ActiveListOption.UseValueAsListIndex:
					for (int i = 0; i < canvasGroups.Length; i++)
					{
						if (canvasGroups[i] == null)
							continue;

						canvasGroups[i].alpha = i == Value ? 1 : 0;
						canvasGroups[i].blocksRaycasts = i == Value;
						canvasGroups[i].interactable = i == Value;
					}

					break;
				case ActiveListOption.UseValueAsTargetIndex:
					bool isTargetIndex = Value == targetIndex;
					foreach (CanvasGroup canvasGroup in canvasGroups)
					{
						if (canvasGroup == null)
							continue;

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