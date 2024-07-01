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

		[SerializeField] private bool useSync;
		[SerializeField] private CustomBool isMaxScore;
		[SerializeField] private CustomBool isMinScore;

		[UdonSynced(), FieldChangeCallback(nameof(SyncedScore))] private int _syncedScore;
		public int SyncedScore
		{
			get => _syncedScore;
			set
			{
				_syncedScore = value;

				if (useSync)
					SetScore(_syncedScore, isReciever: true);
			}
		}
		private int _score;
		public int Score
		{
			get => _score;
			set
			{
				_score = value;
				OnScoreChange();
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
					SyncedScore = defaultScore;
					RequestSerialization();
				}
			}
			else
			{
				SetScore(defaultScore);
			}

			OnScoreChange();
		}

		public void SetMinMaxScore(int min, int max, bool recalcScore = true)
		{
			MDebugLog($"{nameof(SetMinMaxScore)}");

			MinScore = min;
			MaxScore = max;

			if (recalcScore)
				SetScore(Score);
		}

		public void SetScore(int newScore, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetScore)}");

			int actualScore = newScore;

			switch (style)
			{
				case MScoreStyle.None:
					if (actualScore > MaxScore)
						return;
					if (actualScore < MinScore)
						return;
					break;
				// Clamp
				case MScoreStyle.Clamp:
					actualScore = Mathf.Clamp(actualScore, MinScore, MaxScore);
					break;
				// LoopA : 초과/미만 시 반대쪽으로 이동 (이때 MinScore, MaxScore는 포함되지 않음)
				// { ..., Max - 1, Max == Min, Min + 1, ... }
				// ex) MinScore : 0, MaxScore : 100, Score : 101 -> 1
				// ex) MinScore : 0, MaxScore : 100, Score : -1 -> 99
				case MScoreStyle.LoopA:
					if (actualScore > MaxScore)
						actualScore = MinScore + (actualScore - MaxScore);
					else if (actualScore < MinScore)
						actualScore = MaxScore - (MinScore - actualScore);
					break;
				// LoopB : 초과/미만 시 반대쪽으로 이동 (이때 MinScore, MaxScore는 포함됨)
				// { ..., Max - 1, Max, Min, Min + 1, ... }
				// ex) MinScore : 0, MaxScore : 100, Score : 101 -> 0
				// ex) MinScore : 0, MaxScore : 100, Score : -1 -> 100
				case MScoreStyle.LoopB:
					if (actualScore > MaxScore)
						actualScore = MinScore + (actualScore - MaxScore) - 1;
					else if (actualScore < MinScore)
						actualScore = MaxScore - (MinScore - actualScore) + 1;
					break;
			}

			if (useSync)
			{
				if (SyncedScore != actualScore)
				{
					if (isReciever == false)
					{
						SetOwner();
						SyncedScore = actualScore;
						RequestSerialization();
					}
				}
			}

			Score = actualScore;
		}

		private void OnScoreChange()
		{
			MDebugLog(nameof(OnScoreChange));

			if (isMaxScore != null)
				isMaxScore.SetValue(Score == MaxScore);
			
			if (isMinScore != null)
				isMinScore.SetValue(Score == MinScore);

			SendEvents();
		}

		public void IncreaseScore() => SetScore(Score + IncreaseAmount);
		public void AddScore(int amount) => SetScore(Score + amount);
		public void DecreaseScore() => SetScore(Score - DecreaseAmount);
		public void SubScore(int amount) => SetScore(Score - amount);
		public void ResetScore() => SetScore(defaultScore);
	}
}