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

		[UdonSynced, FieldChangeCallback(nameof(ExpireTime))] private int _expireTime = NONE_INT;
		public int ExpireTime
		{
			get => _expireTime;
			set
			{
				_expireTime = value;
				OnExpireTimeChange();
			}
		}

		[UdonSynced, FieldChangeCallback(nameof(TimeSpeed))] private int _timeSpeed = 1;
		public int TimeSpeed
		{
			get => _timeSpeed;
			set
			{
				_timeSpeed = value;
				// OnExpireTimeChange();
			}
		}

		public bool IsExpired => ExpireTime == NONE_INT;

		private void Update()
		{
			// UpdateUI();

			if (IsOwner() == false)
				return;

			if (ExpireTime == NONE_INT)
				return;

			if (Networking.GetServerTimeInMilliseconds() >= ExpireTime)
			{
				MDebugLog("Expired!");
				ResetTime();
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

		public void ResetTime()
		{
			MDebugLog(nameof(ResetTime));
			SetExpireTime(NONE_INT);
		}

		public void SetTimer() => SetTimer(TimeByDecisecond);

		public void SetTimer(int timeByDecisecond)
		{
			MDebugLog($"{nameof(SetTimer)} : {timeByDecisecond}");
			SetExpireTime(Networking.GetServerTimeInMilliseconds() + (timeByDecisecond * 100));
		}

		public void SetTimeByMValue()
		{
			MDebugLog(nameof(SetTimeByMValue));

			if (mValue != null)
				SetExpireTime(Networking.GetServerTimeInMilliseconds() + (mValue.Value * 100));
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

		public void ToggleTime()
		{
			MDebugLog(nameof(ToggleTime));

			if (ExpireTime == NONE_INT)
				SetTimer();
			else
				ResetTime();
		}
	}
}