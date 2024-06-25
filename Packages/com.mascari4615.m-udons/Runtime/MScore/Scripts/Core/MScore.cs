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
		[SerializeField] private bool useClamp;
		[SerializeField] private bool useLoop;
		[SerializeField] private bool loopOption;

		[SerializeField] private bool useSync;
		[SerializeField] private CustomBool isMaxScore;
		[SerializeField] private CustomBool isMinScore;

		[SerializeField] private UIMScore[] uis;

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

			foreach (UIMScore ui in uis)
				ui.Init(this);

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

		public void SetMinMaxScore(int min, int max)
		{
			MDebugLog($"{nameof(SetMinMaxScore)}");

			MinScore = min;
			MaxScore = max;

			if (Score < MinScore || Score > MaxScore)
				SetScore(Mathf.Clamp(Score, MinScore, MaxScore));
		}

		public void SetScore(int newScore, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetScore)}");

			int actualScore = newScore;

			if (useClamp)
			{
				actualScore = Mathf.Clamp(actualScore, MinScore, MaxScore);
			}
			else if (useLoop)
			{
				if (actualScore > MaxScore)
					actualScore = MinScore + (actualScore - MaxScore) + (loopOption ? -1 : 0);
				else if (actualScore < MinScore)
					actualScore = MaxScore - (MinScore - actualScore) + (loopOption ? 1 : 0);
			}
			else
			{
				if (actualScore > MaxScore)
					return;

				if (actualScore < MinScore)
					return;
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