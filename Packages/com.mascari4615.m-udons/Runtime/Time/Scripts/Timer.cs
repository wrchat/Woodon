using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Timer : MEventSender
	{
		[field: Header("_" + nameof(Timer))]
		[field: SerializeField] public int TimeByDecisecond { get; set; } = 50;
		[SerializeField] private MValue mValue;
		[SerializeField] private MBool isCounting;

		public int ExpireTime
		{
			get => _expireTime;
			private set
			{
				_expireTime = value;
				OnExpireTimeChange();
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(ExpireTime))] private int _expireTime = NONE_INT;

		// Idea By 'Listing'
		// 서버 시간이 음수가 되는 경우를 방지하기 위해 (최초값 -10억), 서버 시간에 15억을 더함
		public const int TIME_ADJUSTMENT = 1_500_000_000;
		public int CalcedCurTime => Networking.GetServerTimeInMilliseconds() + TIME_ADJUSTMENT;
		public bool IsExpiredOrStoped => ExpireTime == NONE_INT;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mValue != null)
				mValue.RegisterListener(this, nameof(SetTimeByMValue));
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

		public void SetTimer() => SetTimer(TimeByDecisecond);

		public void SetTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(SetTimer)} : {timeByDecisecond}");
			SetExpireTime(CalcedCurTime + (timeByDecisecond * 100));
		}

		public void SetTimeByMValue()
		{
			MDebugLog(nameof(SetTimeByMValue));

			if (mValue != null)
				SetExpireTime(CalcedCurTime + (mValue.Value * 100));
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

			if (mValue == null)
				return;

			if (ExpireTime == NONE_INT)
				return;

			SetExpireTime(ExpireTime + mValue.Value * 100);
		}

		public void ToggleTimer()
		{
			MDebugLog(nameof(ToggleTimer));

			if (ExpireTime == NONE_INT)
				SetTimer();
			else
				ResetTimer();
		}
	}
}