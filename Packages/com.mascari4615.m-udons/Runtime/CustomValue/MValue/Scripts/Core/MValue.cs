using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MValue : MEventSender
	{
		[field:Header("_" + nameof(MValue))]
		[field: SerializeField] public int MinValue{ get; private set; } = 0;
		[field: SerializeField] public int MaxValue { get; private set; } = 1000;
		[field: SerializeField] public int IncreaseAmount { get; private set; } = 1;
		[field: SerializeField] public int DecreaseAmount { get; private set; }= 1;
		[SerializeField] private int defaultValue = 0;
		[SerializeField] private MValueStyle style = MValueStyle.Clamp;

		[SerializeField] private bool useSync = true;
		[SerializeField] private MBool isMaxValue;
		[SerializeField] private MBool isMinValue;

		public int SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private int _syncedValue;

		public int Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}
		private int _value;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (useSync)
			{
				if (Networking.IsMaster)
				{
					SyncedValue = defaultValue;
					RequestSerialization();
				}
			}
			else
			{
				SetValue(defaultValue);
			}

			OnValueChange();
		}

		public void SetMinMaxValue(int min, int max, bool recalcValue = true)
		{
			MDebugLog($"{nameof(SetMinMaxValue)}");

			MinValue = min;
			MaxValue = max;

			if (recalcValue)
				SetValue(Value);
		}

		public void SetValue(int newValue, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetValue)}");

			int actualValue = newValue;

			switch (style)
			{
				case MValueStyle.None:
					if (actualValue > MaxValue)
						return;
					if (actualValue < MinValue)
						return;
					break;
				// Clamp
				case MValueStyle.Clamp:
					actualValue = Mathf.Clamp(actualValue, MinValue, MaxValue);
					break;
				// LoopA : 초과/미만 시 반대쪽으로 이동 (이때 MinValue, MaxValue는 포함되지 않음)
				// { ..., Max - 1, Max == Min, Min + 1, ... }
				// ex) MinValue : 0, MaxValue : 100, Value : 101 -> 1
				// ex) MinValue : 0, MaxValue : 100, Value : -1 -> 99
				case MValueStyle.LoopA:
					if (actualValue > MaxValue)
						actualValue = MinValue + (actualValue - MaxValue);
					else if (actualValue < MinValue)
						actualValue = MaxValue - (MinValue - actualValue);
					break;
				// LoopB : 초과/미만 시 반대쪽으로 이동 (이때 MinValue, MaxValue는 포함됨)
				// { ..., Max - 1, Max, Min, Min + 1, ... }
				// ex) MinValue : 0, MaxValue : 100, Value : 101 -> 0
				// ex) MinValue : 0, MaxValue : 100, Value : -1 -> 100
				case MValueStyle.LoopB:
					if (actualValue > MaxValue)
						actualValue = MinValue + (actualValue - MaxValue) - 1;
					else if (actualValue < MinValue)
						actualValue = MaxValue - (MinValue - actualValue) + 1;
					break;
			}

			if (useSync)
			{
				if (SyncedValue != actualValue)
				{
					if (isReciever == false)
					{
						SetOwner();
						SyncedValue = actualValue;
						RequestSerialization();
					}
				}
			}

			Value = actualValue;
		}

		private void OnValueChange()
		{
			MDebugLog($"{nameof(OnValueChange)} : {Value}");

			if (isMaxValue != null)
				isMaxValue.SetValue(Value == MaxValue);
			
			if (isMinValue != null)
				isMinValue.SetValue(Value == MinValue);

			SendEvents();
		}

		public void IncreaseValue() => SetValue(Value + IncreaseAmount);
		public void AddValue(int amount) => SetValue(Value + amount);
		public void DecreaseValue() => SetValue(Value - DecreaseAmount);
		public void SubValue(int amount) => SetValue(Value - amount);
		public void ResetValue() => SetValue(defaultValue);
	}
}