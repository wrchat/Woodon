using System;
using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UITimer : MBase
	{
		[Header("_" + nameof(UITimer))]
		[SerializeField] private Timer timer;
		[SerializeField] private TextMeshProUGUI[] remainTimeTexts;
		[SerializeField] private string format = "{0:mm\\:ss.ff}";
		[SerializeField] private float[] remainTimeFlags;
		[SerializeField] private Color[] remainTimeColors;
		private Color[] originColors;

		// Lerp
		[Header("_ Lerp")]
		[SerializeField] private Color lerpColor = Color.red;
		[SerializeField] private bool useLerp;
		[SerializeField] private float lerpTime = 2;
		private float curLerpTime = 0;
		private int lastSavedExpireTime = NONE_INT;
		private int changedTimeDiff = 0;

		// Fields
		private TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);
		public bool IsLerping =>
		(timer != null) &&
		(useLerp == true) &&
		(lastSavedExpireTime != NONE_INT) &&
		(timer.ExpireTime != NONE_INT) &&
		(curLerpTime > 0);

		private void Start()
		{
			if (timer == null)
			{
				Debug.LogError($"{nameof(timer)} is null!");
				return;
			}

			Init();
		}

		private void Init()
		{
			originColors = new Color[remainTimeTexts.Length];
			for (int i = 0; i < remainTimeTexts.Length; i++)
				originColors[i] = remainTimeTexts[i].color;
		}

		private void Update()
		{
			if (timer == null)
				return;

			CalcTimeSpan();
			UpdateUI();
		}

		private void UpdateUI()
		{
			// Text String
			string formatedString = string.Format(format, timeSpan);

			// Text Color
			Color color = default;
			if (timer.IsExpiredOrStoped)
			{
				color = default;
			}
			else if (IsLerping)
			{
				color = lerpColor;
			}
			else
			{
				for (int i = 0; i < remainTimeFlags.Length; i++)
				{
					if (timeSpan.TotalSeconds <= remainTimeFlags[i])
						color = remainTimeColors[i];
				}
			}

			// Update Texts
			for (int i = 0; i < remainTimeTexts.Length; i++)
			{
				remainTimeTexts[i].text = formatedString;
				remainTimeTexts[i].color = color == default ? originColors[i] : color;
			}
		}

		private void CalcTimeSpan()
		{
			// Time Span
			if (useLerp)
			{
				CalcLerpTime();

				if (lastSavedExpireTime != NONE_INT)
				{
					int diff = (int)(lastSavedExpireTime - timer.CalcedCurTime + (int)(changedTimeDiff * (curLerpTime / lerpTime)));
					timeSpan = TimeSpan.FromMilliseconds(diff);
					MDebugLog($"curLerpTime: {curLerpTime}, ||| ((curLerpTime / lerpTime) : {curLerpTime / lerpTime}");
				}
			}
			else
			{
				int diff = (int)(timer.ExpireTime - timer.CalcedCurTime);
				diff = Mathf.Max(diff, 0);
				timeSpan = TimeSpan.FromMilliseconds(diff);
			}
		}

		private void CalcLerpTime()
		{
			if (timer.IsExpiredOrStoped)
			{
				lastSavedExpireTime = NONE_INT;
				changedTimeDiff = 0;
				curLerpTime = 0;
				return;
			}

			// Lerp
			curLerpTime = Mathf.Max(curLerpTime - Time.deltaTime, 0);

			// If : Expire Time Changed
			if (lastSavedExpireTime != timer.ExpireTime)
			{
				// If : First Time (Init)
				if (lastSavedExpireTime == NONE_INT)
				{
					lastSavedExpireTime = (int)timer.ExpireTime;
					changedTimeDiff = 0;
					curLerpTime = 0;
					return;
				}
				// If : Remain Time Changed (Reset Lerp)
				else
				{
					changedTimeDiff = (int)Mathf.Abs(lastSavedExpireTime - timer.ExpireTime);
					MDebugLog($"remainChangeTime: {changedTimeDiff} = {lastSavedExpireTime} - {timer.ExpireTime}");

					lastSavedExpireTime = (int)timer.ExpireTime;
					curLerpTime = lerpTime;
				}
			}
		}

		#region HorribleEvents
		public void ResetTimer() => timer.ResetTimer();
		public void SetTimer() => timer.StartTimer();
		public void SetTimeByMValue() => timer.StartTimerByMValue();
		public void AddTime() => timer.AddTime();
		public void AddTimeByMValue() => timer.AddTimeByMValue();
		public void ToggleTimer() => timer.ToggleTimer();
		#endregion
	}
}