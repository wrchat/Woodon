
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UITimeEventBar : MBase
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private Image bar;
		private TimeEvent timeEvent;

		public void Init(TimeEvent timeEvent)
		{
			this.timeEvent = timeEvent;
		}

		public void UpdateUI()
		{
			float fillAmount = ((timeEvent.TimeByDecisecond * 100f) - (timeEvent.ExpireTime - Networking.GetServerTimeInMilliseconds())) / (timeEvent.TimeByDecisecond * 100f);
			bar.fillAmount = fillAmount;

			if (timeEvent.IsExpired)
				canvasGroup.alpha = 0;
			else
				canvasGroup.alpha = 1;

			// MDebugLog($"{nameof(fillAmount)} = {fillAmount} = {}");
		}
	}
}