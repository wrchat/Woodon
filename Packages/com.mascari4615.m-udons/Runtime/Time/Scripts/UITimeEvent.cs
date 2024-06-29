using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UITimeEvent : MBase
	{
		[field: Header("_" + nameof(UITimeEvent))]
		[SerializeField] private TextMeshProUGUI[] remainTimeTexts;
		[SerializeField] private string format = "{0:mm\\:ss.ff}";
		[SerializeField] private float[] remainTimeFlags;
		[SerializeField] private Color[] remainTimeColors;

		[Header("_ Lerp")]
		[SerializeField] private Color lerpColor;
		[SerializeField] private bool lerp;
		[SerializeField] private int lerpSpeed = 1;

		private int lastExpireTime = NONE_INT;
		private TimeEvent timeEvent;

		public bool IsLerping =>
		(timeEvent != null) &&
		(lerp == true) &&
		(lastExpireTime != timeEvent.ExpireTime) &&
		(lastExpireTime != NONE_INT) &&
		(timeEvent.ExpireTime != NONE_INT);

		public void Init(TimeEvent timeEvent)
		{
			this.timeEvent = timeEvent;
		}

		private void Update()
		{
			if (timeEvent == null)
				return;

			CalcTime();
			UpdateUI();
		}

		public void UpdateUI()
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);

			if (lastExpireTime != NONE_INT)
			{
				int diff = lastExpireTime - Networking.GetServerTimeInMilliseconds();
				timeSpan = TimeSpan.FromMilliseconds(diff);
			}

			string formatedString = string.Format(format, timeSpan);

			Color color = Color.white;

			if (IsLerping)
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

			foreach (TextMeshProUGUI remainTimeText in remainTimeTexts)
			{
				remainTimeText.text = formatedString;
				remainTimeText.color = color;
			}
		}

		private void CalcTime()
		{
			// 타이머가 멈춰있거나,
			if (timeEvent.ExpireTime == NONE_INT)
			{
				lastExpireTime = NONE_INT;
				return;
			}

			// Lerp 하지 않는 경우
			if (lerp == false)
			{
				lastExpireTime = timeEvent.ExpireTime;
				return;
			}

			if (lastExpireTime == timeEvent.ExpireTime)
				return;

			// 1. Lerp

			// 1-1. Init
			if (lastExpireTime == NONE_INT)
			{
				lastExpireTime = timeEvent.ExpireTime;
				return;
			}

			// 1-2. Lerp
			if (lastExpireTime != timeEvent.ExpireTime)
			{
				// 강제 보정 (.5초)
				if (Mathf.Abs(lastExpireTime - timeEvent.ExpireTime) <= 500)
				{
					lastExpireTime = timeEvent.ExpireTime;
				}
				else
				{
					lastExpireTime = (int)Mathf.Lerp(lastExpireTime, timeEvent.ExpireTime, Time.deltaTime * lerpSpeed);
				}
			}
		}

		public void ResetTime() => timeEvent.ResetTime();
		public void SetTimer() => timeEvent.SetTimer();
		public void SetTimeByMScore() => timeEvent.SetTimeByMScore();
		public void AddTime() => timeEvent.AddTime();
		public void AddTimeByMScore() => timeEvent.AddTimeByMScore();
		public void ToggleTime() => timeEvent.ToggleTime();
	}
}