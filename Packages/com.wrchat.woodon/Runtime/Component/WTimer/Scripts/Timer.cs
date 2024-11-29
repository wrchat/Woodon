using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Timer : WEventPublisher
	{
		[field: Header("_" + nameof(Timer))]
		// 서버 시간은 밀리초 단위지만, 계산은 데시초 단위로 할 것
		// 데시초 = 1/10초
		[field: SerializeField] public int TimeByDecisecond { get; private set; } = 50;
		[SerializeField] private MValue mValueForSetTime;
		[SerializeField] private MValue mValueForAddTime;
		[SerializeField] private MBool isCounting;

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

		// Idea By 'Listing'
		// 서버 시간이 음수가 되는 경우를 방지하기 위해 (관측된 최솟값 -20.4억), 서버 시간에 int.MaxValue(2147483647)을 더함
		public const int TIME_ADJUSTMENT = int.MaxValue;
		public int CalcedCurTime =>
			Networking.GetServerTimeInMilliseconds() + (Networking.GetServerTimeInMilliseconds() < 0 ? TIME_ADJUSTMENT : 0);
		public bool IsExpiredOrStoped => ExpireTime == NONE_INT;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mValueForSetTime != null)
			{
				mValueForSetTime.RegisterListener(this, nameof(SetTimerByMValue));
				SetTimerByMValue();
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

			if (CalcedCurTime >= ExpireTime)
			{
				MDebugLog("Expired!");
				ResetTimer();
			}
		}

		private void OnExpireTimeChange(int origin)
		{
			MDebugLog($"{nameof(OnExpireTimeChange)} : ChangeTo = {ExpireTime}");

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
			MDebugLog(nameof(ResetTimer));
			SetExpireTime(NONE_INT);
		}

		public void SetTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(SetTimer)} : {timeByDecisecond} Decisecond");
			TimeByDecisecond = timeByDecisecond;
		}
		public void SetTimerByMValue()
		{
			MDebugLog(nameof(SetTimerByMValue));
			SetTimer(mValueForSetTime.Value);
		}

		public void StartTimer()
		{
			if (mValueForSetTime != null)
				StartTimer(mValueForSetTime.Value);
			else
				StartTimer(TimeByDecisecond);
		}
		public void StartTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(StartTimer)} : {timeByDecisecond} Decisecond");
			SetExpireTime(CalcedCurTime + (timeByDecisecond * 100));
		}
		public void StartTimerByMValue()
		{
			MDebugLog(nameof(StartTimerByMValue));

			if (mValueForSetTime != null)
				SetExpireTime(CalcedCurTime + (mValueForSetTime.Value * 100));
		}

		public void AddTime()
		{
			MDebugLog(nameof(AddTime));

			if (ExpireTime != NONE_INT)
				SetExpireTime(ExpireTime + TimeByDecisecond * 100);
		}

		public void AddTimeByMValue()
		{
			MDebugLog(nameof(AddTimeByMValue));

			if (mValueForAddTime == null)
				return;

			if (ExpireTime == NONE_INT)
				return;

			SetExpireTime(ExpireTime + mValueForAddTime.Value * 100);
		}

		public void ToggleTimer()
		{
			MDebugLog(nameof(ToggleTimer));

			if (ExpireTime == NONE_INT)
				StartTimer();
			else
				ResetTimer();
		}
	}
}