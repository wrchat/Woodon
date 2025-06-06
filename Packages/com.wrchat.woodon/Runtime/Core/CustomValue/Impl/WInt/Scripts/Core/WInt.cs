﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WInt : WEventPublisher
	{
		[field: Header("_" + nameof(WInt))]
		[field: SerializeField] public int MinValue { get; private set; } = 0;
		[field: SerializeField] public int MaxValue { get; private set; } = int.MaxValue;
		[field: SerializeField] public int IncreaseAmount { get; private set; } = 1;
		[field: SerializeField] public int DecreaseAmount { get; private set; } = 1;
		[field: SerializeField] public int DefaultValue { get; private set; } = 0;
		[field: SerializeField] public WIntStyle Style { get; private set; } = WIntStyle.Clamp;
		[field: SerializeField] public bool UseSync { get; private set; } = true;
		[SerializeField] private WBool isMaxValue;
		[SerializeField] private WBool isMinValue;

		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private int _syncedValue;
		public int SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (UseSync)
					SetValue(_syncedValue, isReceiverContext: true);
			}
		}

		private int _value;
		public int Value
		{
			get => _value;
			private set
			{
				int origin = _value;
				_value = value;
				OnValueChange(DataChangeStateUtil.GetChangeState(origin, _value));
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			WDebugLog($"{nameof(Init)}");

			if (UseSync)
			{
				if (Networking.IsMaster)
					SetValue(DefaultValue);
			}
			else
			{
				SetValue(DefaultValue);
			}

			OnValueChange(DataChangeState.None);
		}

		public void SetMinMaxValue(int min, int max, bool shouldRecalculate = true)
		{
			WDebugLog($"{nameof(SetMinMaxValue)}");

			MinValue = min;
			MaxValue = max;

			if (shouldRecalculate)
				SetValue(Value);
		}

		public void SetValue(int newValue, bool isReceiverContext = false)
		{
			WDebugLog($"{nameof(SetValue)}");

			int actualValue = newValue;

			switch (Style)
			{
				case WIntStyle.None:
					if (actualValue > MaxValue)
						return;
					if (actualValue < MinValue)
						return;
					break;
				// Clamp
				case WIntStyle.Clamp:
					actualValue = Mathf.Clamp(actualValue, MinValue, MaxValue);
					break;
				// LoopA : 초과/미만 시 반대쪽으로 이동 (이때 MinValue, MaxValue는 포함되지 않음)
				// { ..., Max - 1, Max == Min, Min + 1, ... }
				// ex) MinValue : 0, MaxValue : 100, Value : 101 -> 1
				// ex) MinValue : 0, MaxValue : 100, Value : -1 -> 99
				case WIntStyle.LoopA:
					if (actualValue > MaxValue)
						actualValue = MinValue + (actualValue - MaxValue);
					else if (actualValue < MinValue)
						actualValue = MaxValue - (MinValue - actualValue);
					break;
				// LoopB : 초과/미만 시 반대쪽으로 이동 (이때 MinValue, MaxValue는 포함됨)
				// { ..., Max - 1, Max, Min, Min + 1, ... }
				// ex) MinValue : 0, MaxValue : 100, Value : 101 -> 0
				// ex) MinValue : 0, MaxValue : 100, Value : -1 -> 100
				case WIntStyle.LoopB:
					if (actualValue > MaxValue)
						actualValue = MinValue + (actualValue - MaxValue) - 1;
					else if (actualValue < MinValue)
						actualValue = MaxValue - (MinValue - actualValue) + 1;
					break;
			}

			if (isReceiverContext == false)
			{
				if (UseSync && SyncedValue != newValue)
				{
					SetOwner();
					SyncedValue = actualValue;
					RequestSerialization();

					return;
				}
			}

			Value = actualValue;
		}

		private void OnValueChange(DataChangeState dataChangeState)
		{
			WDebugLog($"{nameof(OnValueChange)} : {Value}");

			if (isMaxValue != null)
				isMaxValue.SetValue(Value == MaxValue);

			if (isMinValue != null)
				isMinValue.SetValue(Value == MinValue);

			SendEvents();

			if (dataChangeState == DataChangeState.Greater)
				SendEvents(WIntEvent.OnValueIncreased);
			else if (dataChangeState == DataChangeState.Less)
				SendEvents(WIntEvent.OnValueDecreased);
		}

		[ContextMenu(nameof(IncreaseValue))]
		public void IncreaseValue() => SetValue(Value + IncreaseAmount);
		public void AddValue(int amount) => SetValue(Value + amount);

		[ContextMenu(nameof(DecreaseValue))]
		public void DecreaseValue() => SetValue(Value - DecreaseAmount);
		public void SubValue(int amount) => SetValue(Value - amount);

		[ContextMenu(nameof(ResetValue))]
		public void ResetValue() => SetValue(DefaultValue);
	}
}