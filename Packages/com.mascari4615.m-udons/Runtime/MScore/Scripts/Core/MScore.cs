using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MScore : MEventSender
	{
		[field:Header("_" + nameof(MScore))]
		[field: SerializeField] public int MinScore{ get; private set; } = 0;
		[field: SerializeField] public int MaxScore { get; private set; } = 1000;
		[field: SerializeField] public int IncreaseAmount { get; private set; } = 1;
		[field: SerializeField] public int DecreaseAmount { get; private set; }= 1;
		[SerializeField] private int defaultScore = 0;
		[SerializeField] private MScoreStyle style = MScoreStyle.Clamp;

		[SerializeField] private bool useSync = true;
		[SerializeField] private CustomBool isMaxScore;
		[SerializeField] private CustomBool isMinScore;

		[UdonSynced(), FieldChangeCallback(nameof(SyncedValue))] private int _syncedValue;
		public int SyncedValue
		{
			get => _syncedValue;
			set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}
		private int _value;
		public int Value
		{
			get => _value;
			set
			{
				_value = value;
				OnValueChange();
			}
		}

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
					SyncedValue = defaultScore;
					RequestSerialization();
				}
			}
			else
			{
				SetValue(defaultScore);
			}

			OnValueChange();
		}

		public void SetMinMaxValue(int min, int max, bool recalcValue = true)
		{
			MDebugLog($"{nameof(SetMinMaxValue)}");

			MinScore = min;
			MaxScore = max;

			if (recalcValue)
				SetValue(Value);
		}

		public void SetValue(int newValue, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetValue)}");

			int actualValue = newValue;

			switch (style)
			{
				case MScoreStyle.None:
					if (actualValue > MaxScore)
						return;
					if (actualValue < MinScore)
						return;
					break;
				// Clamp
				case MScoreStyle.Clamp:
					actualValue = Mathf.Clamp(actualValue, MinScore, MaxScore);
					break;
				// LoopA : 초과/미만 시 반대쪽으로 이동 (이때 MinScore, MaxScore는 포함되지 않음)
				// { ..., Max - 1, Max == Min, Min + 1, ... }
				// ex) MinScore : 0, MaxScore : 100, Score : 101 -> 1
				// ex) MinScore : 0, MaxScore : 100, Score : -1 -> 99
				case MScoreStyle.LoopA:
					if (actualValue > MaxScore)
						actualValue = MinScore + (actualValue - MaxScore);
					else if (actualValue < MinScore)
						actualValue = MaxScore - (MinScore - actualValue);
					break;
				// LoopB : 초과/미만 시 반대쪽으로 이동 (이때 MinScore, MaxScore는 포함됨)
				// { ..., Max - 1, Max, Min, Min + 1, ... }
				// ex) MinScore : 0, MaxScore : 100, Score : 101 -> 0
				// ex) MinScore : 0, MaxScore : 100, Score : -1 -> 100
				case MScoreStyle.LoopB:
					if (actualValue > MaxScore)
						actualValue = MinScore + (actualValue - MaxScore) - 1;
					else if (actualValue < MinScore)
						actualValue = MaxScore - (MinScore - actualValue) + 1;
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
			MDebugLog(nameof(OnValueChange));

			if (isMaxScore != null)
				isMaxScore.SetValue(Value == MaxScore);
			
			if (isMinScore != null)
				isMinScore.SetValue(Value == MinScore);

			SendEvents();
		}

		public void IncreaseScore() => SetValue(Value + IncreaseAmount);
		public void AddValue(int amount) => SetValue(Value + amount);
		public void DecreaseScore() => SetValue(Value - DecreaseAmount);
		public void SubValue(int amount) => SetValue(Value - amount);
		public void ResetScore() => SetValue(defaultScore);
	}
}