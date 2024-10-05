using UnityEngine;

namespace WakVRC
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActive : ActiveToggle
	{
		[Header("_" + nameof(CanvasGroupActive))]
		[SerializeField] private CanvasGroup[] activeCanvasGroups;
		[SerializeField] private CanvasGroup[] disableCanvasGroups;

		[Header("_" + nameof(CanvasGroupActive) + " - Options")]
		[SerializeField] private bool toggleOnlyInteractable;

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}");

			if (toggleOnlyInteractable)
			{
				foreach (CanvasGroup c in activeCanvasGroups)
					c.interactable = Active;

				foreach (CanvasGroup c in disableCanvasGroups)
					c.interactable = !Active;
			}
			else
			{
				foreach (CanvasGroup c in activeCanvasGroups)
					MUtil.SetCanvasGroupActive(c, Active);

				foreach (CanvasGroup c in disableCanvasGroups)
					MUtil.SetCanvasGroupActive(c, !Active);
			}
		}
	}
}