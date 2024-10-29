using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MFullscreen : MBase
	{
		[SerializeField] private MBool screenActive;
		private CanvasGroup canvasGroup;

		private void Start()
		{
			canvasGroup = GetComponent<CanvasGroup>();

			if (!screenActive)
				SetActiveFalse();
		}

		public void SetActive(bool value)
		{
			MDebugLog($"{nameof(SetActive)}({value})");
			canvasGroup.alpha = value ? 1 : 0;
		}

		[ContextMenu(nameof(ToggleActive))]
		public void ToggleActive() => SetActive(canvasGroup.alpha != 1);

		[ContextMenu(nameof(SetActiveTrue))]
		public void SetActiveTrue() => SetActive(true);

		[ContextMenu(nameof(SetActiveTrue_Global))]
		public void SetActiveTrue_Global() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetActiveTrue));

		[ContextMenu(nameof(SetActiveFalse))]
		public void SetActiveFalse() => SetActive(false);

		[ContextMenu(nameof(SetActiveFalse_Global))]
		public void SetActiveFalse_Global() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetActiveFalse));

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (screenActive)
				SetActive(screenActive.Value);
		}
	}
}