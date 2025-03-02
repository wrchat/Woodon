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

			if (CalcedCurTime >= ExpireTime)
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
			SetExpireTime(CalcedCurTime + (timeByDecisecond * 100));
		}
		public void StartTimerByWInt()
		{
			WDebugLog(nameof(StartTimerByWInt));

			if (wIntForSetTime != null)
				SetExpireTime(CalcedCurTime + (wIntForSetTime.Value * 100));
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