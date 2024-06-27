using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TimeEvent : MEventSender
	{
		[Header("_" + nameof(TimeEvent))]
		[SerializeField] private TextMeshProUGUI[] timeTexts;
		[SerializeField] private TimeEventBarUI[] timeEventBarUIs;
		[SerializeField] private Image[] buttonUIImages;
		[SerializeField] private string format = "{0:mm\\:ss.ff}";
		[field: SerializeField] public int TimeByDecisecond { get; set; } = 50;
		[SerializeField] private MScore mScore;
		[SerializeField] private CustomBool isCounting;

		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(ExpireTime))]
		private int _expireTime = NONE_INT;

		public int ExpireTime
		{
			get => _expireTime;
			set
			{
				_expireTime = value;
				OnExpireTimeChange();
			}
		}

		public bool IsExpired => (ExpireTime == NONE_INT);

		private void Start()
		{
			foreach (TimeEventBarUI timeEventBarUI in timeEventBarUIs)
				timeEventBarUI.Init(this);
		}

		private void Update()
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(0);

			if (ExpireTime == NONE_INT)
			{
			}
			else
			{
				if (Networking.GetServerTimeInMilliseconds() >= ExpireTime)
				{
				}
				else
				{
					int diff = ExpireTime - Networking.GetServerTimeInMilliseconds();
					timeSpan = TimeSpan.FromMilliseconds(diff);
				}
			}

			foreach (TextMeshProUGUI timeText in timeTexts)
				timeText.text = string.Format(format, timeSpan);

			foreach (TimeEventBarUI timeEventBarUI in timeEventBarUIs)
				timeEventBarUI.UpdateUI();

			if (IsOwner() == false)
				return;

			if (ExpireTime == NONE_INT)
				return;

			if (Networking.GetServerTimeInMilliseconds() >= ExpireTime)
			{
				MDebugLog("Expired!");
				SendEvents();
				ResetTime();
			}
		}

		private void OnExpireTimeChange()
		{
			MDebugLog($"{nameof(OnExpireTimeChange)} : ChangeTo = {ExpireTime}");

			if (isCounting)
				isCounting.SetValue(ExpireTime != NONE_INT);

			foreach (Image buttonUIImage in buttonUIImages)
				buttonUIImage.color = MColorUtil.GetGreenOrRed(isCounting);
		}

		public void ResetTime()
		{
			MDebugLog(nameof(ResetTime));

			SetOwner();
			ExpireTime = NONE_INT;
			RequestSerialization();
		}

		public void SetTime()
		{
			MDebugLog(nameof(SetTime));

			SetOwner();
			ExpireTime = Networking.GetServerTimeInMilliseconds() + (TimeByDecisecond * 100);
			RequestSerialization();
		}

		public void SetTimeByMScore()
		{
			MDebugLog(nameof(SetTimeByMScore));

			if (mScore == null)
				return;

			SetOwner();
			ExpireTime = Networking.GetServerTimeInMilliseconds() + (mScore.Score * 100);
			RequestSerialization();
		}

		public void AddTime()
		{
			MDebugLog(nameof(AddTime));

			if (ExpireTime == NONE_INT)
				return;

			SetOwner();
			ExpireTime += TimeByDecisecond * 100;
			RequestSerialization();
		}

		public void AddTimeByMScore()
		{
			MDebugLog(nameof(AddTimeByMScore));

			if (ExpireTime == NONE_INT)
				return;

			if (mScore == null)
				return;

			SetOwner();
			ExpireTime += mScore.Score * 100;
			RequestSerialization();
		}

		public void ToggleTime()
		{
			MDebugLog(nameof(ToggleTime));

			SetOwner();
			ExpireTime = ExpireTime == NONE_INT ? Networking.GetServerTimeInMilliseconds() + (TimeByDecisecond * 100) : NONE_INT;
			RequestSerialization();
		}
	}
}