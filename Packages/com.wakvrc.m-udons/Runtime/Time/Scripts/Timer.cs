using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Timer : MEventSender
	{
		[field: Header("_" + nameof(Timer))]
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
				_expireTime = value;
				OnExpireTimeChange();
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
				SendEvents((int)TimerEvent.TimeExpired);
			}
		}

		private void OnExpireTimeChange()
		{
			MDebugLog($"{nameof(OnExpireTimeChange)} : ChangeTo = {ExpireTime}");

			if (isCounting)
				isCounting.SetValue(ExpireTime != NONE_INT);

			SendEvents((int)TimerEvent.ExpireTimeChanged);
		}

		public void SetExpireTime(int newExpireTime)
		{
			SetOwner();
			ExpireTime = newExpireTime;
			RequestSerialization();
		}

		public void ResetTimer()
		{
			MDebugLog(nameof(ResetTimer));
			SetExpireTime(NONE_INT);
		}

		public void SetTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(SetTimer)} : {timeByDecisecond}");
			TimeByDecisecond = timeByDecisecond;
		}
		public void SetTimerByMValue()
		{
			MDebugLog(nameof(SetTimerByMValue));

			if (mValueForSetTime != null)
				SetTimer(mValueForSetTime.Value * 100);
		}

		public void StartTimer() => StartTimer(TimeByDecisecond);
		public void StartTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(StartTimer)} : {timeByDecisecond}");
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