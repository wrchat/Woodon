using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

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
		private Color[] defaultColors;

		[Header("_ Lerp")]
		[SerializeField] private Color lerpColor = Color.red;
		[SerializeField] private bool lerp;
		[SerializeField] private float lerpTime = 2;
		private float curLerpTime = 0;

		private int lastExpireTime = NONE_INT;
		private int changedTimeDiff = 0;

		public bool IsLerping =>
		(timer != null) &&
		(lerp == true) &&
		(lastExpireTime != NONE_INT) &&
		(timer.ExpireTime != NONE_INT) &&
		(curLerpTime > 0);

		private void Start()
		{
			if (timer == null)
			{
				MDebugLog($"{nameof(timer)} is null!");
				return;
			}

			Init();
		}

		private void Init()
		{
			defaultColors = new Color[remainTimeTexts.Length];
			for (int i = 0; i < remainTimeTexts.Length; i++)
				defaultColors[i] = remainTimeTexts[i].color;
		}

		private void Update()
		{
			if (timer == null)
				return;

			CalcTime();
			UpdateUI();
		}

		public void UpdateUI()
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);

			if (lastExpireTime != NONE_INT)
			{
				int diff = lastExpireTime - Networking.GetServerTimeInMilliseconds() + (int)(changedTimeDiff * ((curLerpTime / lerpTime)));
				MDebugLog($"curLerpTime: {curLerpTime}, ||| ((curLerpTime / lerpTime) : {(curLerpTime / lerpTime)}");
				timeSpan = TimeSpan.FromMilliseconds(diff);
			}

			string formatedString = string.Format(format, timeSpan);

			Color color = default;

			if (timer.ExpireTime == NONE_INT)
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

			for (int i = 0; i < remainTimeTexts.Length; i++)
			{
				remainTimeTexts[i].text = formatedString;
				remainTimeTexts[i].color = color == default ? defaultColors[i] : color;
			}
		}

		private void CalcTime()
		{
			curLerpTime = Mathf.Max(curLerpTime - Time.deltaTime, 0);

			// 타이머가 멈춰있거나,
			if (timer.ExpireTime == NONE_INT)
			{
				lastExpireTime = NONE_INT;
				changedTimeDiff = 0;
				curLerpTime = 0;
				return;
			}

			// Lerp 하지 않는 경우
			if (lerp == false)
			{
				lastExpireTime = timer.ExpireTime;
				changedTimeDiff = 0;
				curLerpTime = 0;
				return;
			}

			if (lastExpireTime == timer.ExpireTime)
				return;

			// 1. Lerp

			// 1-1. Init
			if (lastExpireTime == NONE_INT)
			{
				lastExpireTime = timer.ExpireTime;
				changedTimeDiff = 0;
				curLerpTime = 0;
				return;
			}

			// 1-2. Lerp
			if (lastExpireTime != timer.ExpireTime)
			{
				int Max = Mathf.Max(lastExpireTime, timer.ExpireTime);
				int Min = Mathf.Min(lastExpireTime, timer.ExpireTime);

				changedTimeDiff = Max - Min;
				MDebugLog($"remainChangeTime: {changedTimeDiff} = {Max} - {Min}");
				lastExpireTime = timer.ExpireTime;

				curLerpTime = lerpTime;
			}
		}

		public void ResetTime() => timer.ResetTime();
		public void SetTimer() => timer.SetTimer();
		public void SetTimeByMValue() => timer.SetTimeByMValue();
		public void AddTime() => timer.AddTime();
		public void AddTimeByMValue() => timer.AddTimeByMValue();
		public void ToggleTime() => timer.ToggleTime();
	}
}