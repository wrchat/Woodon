﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Timer : WEventPublisher
	{
		[field: Header("_" + nameof(Timer))]
		// 서버 시간은 밀리초 단위지만, 계산은 데시초 단위로 할 것
		// 데시초 = 1/10초
		[field: SerializeField] public int TimeByDecisecond { get; private set; } = 50;
		[SerializeField] private WInt wIntForSetTime;
		[SerializeField] private WInt wIntForAddTime;
		[SerializeField] private WBool isCounting;

		[UdonSynced, FieldChangeCallback(nameof(ExpireTime))] private int _expireTime = NONE_INT;
		public int ExpireTime
		{
			get => _expireTime;
			private set
			{
				int origin = _expireTime;
				_expireTime = value;
				OnExpireTimeChange(origin);
			}
		}

		public bool IsTimerStopped => ExpireTime == NONE_INT;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (wIntForSetTime != null)
			{
				wIntForSetTime.RegisterListener(this, nameof(SetTimerByWInt));
				SetTimerByWInt();
			}

			SendEvents();
			SendEvents(TimerEvent.ExpireTimeChanged);
		}

		private void Update()
		{
			// UpdateUI();

			if (IsOwner() == false)
				return;

			if (ExpireTime == NONE_INT)
				return;

			if (ServerTimeAdjusted() >= ExpireTime)
			{
				WDebugLog("Expired!");
				ResetTimer();
			}
		}

		private void OnExpireTimeChange(int origin)
		{
			WDebugLog($"{nameof(OnExpireTimeChange)} : ChangeTo = {ExpireTime}");

			SendEvents();
			SendEvents(TimerEvent.ExpireTimeChanged);

			if (origin == NONE_INT && ExpireTime != NONE_INT)
				SendEvents(TimerEvent.TimerStarted);

			if (origin != NONE_INT && ExpireTime == NONE_INT)
				SendEvents(TimerEvent.TimeExpired);

			if (isCounting)
				isCounting.SetValue(ExpireTime != NONE_INT);
		}

		public void SetExpireTime(int newExpireTime)
		{
			SetOwner();
			ExpireTime = newExpireTime;
			RequestSerialization();
		}

		public void StopTimer() => ResetTimer();
		public void ResetTimer()
		{
			WDebugLog(nameof(ResetTimer));
			SetExpireTime(NONE_INT);
		}

		public void SetTimer(int timeByDecisecond)
		{
			WDebugLog($"{nameof(SetTimer)} : {timeByDecisecond} Decisecond");
			TimeByDecisecond = timeByDecisecond;
		}
		public void SetTimerByWInt()
		{
			WDebugLog(nameof(SetTimerByWInt));
			SetTimer(wIntForSetTime.Value);
		}

		public void StartTimer()
		{
			if (wIntForSetTime != null)
				StartTimer(wIntForSetTime.Value);
			else
				StartTimer(TimeByDecisecond);
		}
		public void StartTimer(int timeByDecisecond)
		{
			WDebugLog($"{nameof(StartTimer)} : {timeByDecisecond} Decisecond");
			SetExpireTime(ServerTimeAdjusted() + (timeByDecisecond * 100));
		}
		public void StartTimerByWInt()
		{
			WDebugLog(nameof(StartTimerByWInt));

			if (wIntForSetTime != null)
				SetExpireTime(ServerTimeAdjusted() + (wIntForSetTime.Value * 100));
		}

		public void AddTime()
		{
			WDebugLog(nameof(AddTime));

			if (ExpireTime != NONE_INT)
				SetExpireTime(ExpireTime + TimeByDecisecond * 100);
		}

		public void AddTimeByWInt()
		{
			WDebugLog(nameof(AddTimeByWInt));

			if (wIntForAddTime == null)
				return;

			if (ExpireTime == NONE_INT)
				return;

			SetExpireTime(ExpireTime + wIntForAddTime.Value * 100);
		}

		public void ToggleTimer()
		{
			WDebugLog(nameof(ToggleTimer));

			if (ExpireTime == NONE_INT)
				StartTimer();
			else
				ResetTimer();
		}
	}
}