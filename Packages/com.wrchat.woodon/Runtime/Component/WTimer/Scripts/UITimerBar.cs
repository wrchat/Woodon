using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UITimerBar : WBase
	{
		[Header("_" + nameof(UITimerBar))]
		[SerializeField] private Timer timer;
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private Image bar;

		private void Start()
		{
			if (timer == null)
			{
				WDebugLog($"{nameof(timer)} is null!");
				return;
			}
		}

		private void Update()
		{
			if (timer == null)
				return;

			UpdateUI();
		}

		public void UpdateUI()
		{
			float fillAmount = ((timer.TimeByDecisecond * 100f) - (timer.ExpireTime - timer.CalcedCurTime)) / (timer.TimeByDecisecond * 100f);
			bar.fillAmount = fillAmount;

			if (timer.IsExpiredOrStoped)
				canvasGroup.alpha = 0;
			else
				canvasGroup.alpha = 1;

			// WDebugLog($"{nameof(fillAmount)} = {fillAmount} = {}");
		}
	}
}