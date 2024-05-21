using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MScore : MEventSender
	{
		[Header("_" + nameof(MScore))]
		[SerializeField] private Image uiActiveButtonImage;
		[SerializeField] private GameObject uiParent;
		[SerializeField] private TextMeshProUGUI[] scoreTexts;

		[SerializeField] private int minScore;
		[field: SerializeField] public int maxScore = 1000;
		[SerializeField] private int increaseAmount = 1;
		[SerializeField] private int decreaseAmount = 1;
		[SerializeField] private int defaultScore = 0;
		[SerializeField] private bool printPlusOne;
		[SerializeField] private bool useClamp;
		[SerializeField] private bool useLoop;

		[SerializeField] private bool useSync;
		[SerializeField] private CustomBool isMaxScore;
		[SerializeField] private CustomBool isMinScore;

		[UdonSynced(), FieldChangeCallback(nameof(SyncedCanvasActive))] private bool _syncedCanvasActive = true;
		private bool SyncedCanvasActive
		{
			get => _syncedCanvasActive;
			set
			{
				_syncedCanvasActive = value;

				if (useSync)
					CanvasActive = SyncedCanvasActive;
			}
		}
		private bool _canvasActive;
		public bool CanvasActive
		{
			get => _canvasActive;
			private set
			{
				_canvasActive = value;
				OnCanvasActiveChange();
			}
		}

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
			OnCanvasActiveChange();
		}

		#region Score
		public void SetMinMaxScore(int min, int max)
		{
			MDebugLog($"{nameof(SetMinMaxScore)}");
			minScore = min;
			maxScore = max;
			if (Score < minScore || Score > maxScore)
				SetScore(Mathf.Clamp(Score, minScore, maxScore));
		}

		public void SetScore(int newScore, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetScore)}");

			int actualScore = newScore;

			if (useClamp)
			{
				actualScore = Mathf.Clamp(actualScore, minScore, maxScore);
			}
			else if (useLoop)
			{
				if (actualScore > maxScore)
					actualScore = actualScore - maxScore - 1;

				if (actualScore < minScore)
					actualScore = maxScore + actualScore;
			}
			else
			{
				if (actualScore > maxScore)
					return;

				if (actualScore < minScore)
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

		public void SetScore0() => SetScore(0);
		public void SetScore1() => SetScore(1);
		public void SetScore2() => SetScore(2);
		public void SetScore3() => SetScore(3);
		public void SetScore4() => SetScore(4);

		private void OnScoreChange()
		{
			// MDebugLog(nameof(OnScoreChange));

			if (isMaxScore)
				isMaxScore.SetValue(Score == maxScore);
			if (isMinScore)
				isMinScore.SetValue(Score == minScore);

			foreach (TextMeshProUGUI scoreText in scoreTexts)
				scoreText.text = (Score + (printPlusOne ? 1 : 0)).ToString();

			SendEvents();
		}

		public void IncreaseScore() => SetScore(Score + increaseAmount);
		public void AddScore(int amount) => SetScore(Score + amount);
		public void AddScore10() => AddScore(10);
		public void DecreaseScore() => SetScore(Score - decreaseAmount);
		public void SubScore(int amount) => SetScore(Score - amount);
		public void SubScore10() => SubScore(10);
		public void ResetScore() => SetScore(defaultScore);
		#endregion

		#region Canvas
		private void OnCanvasActiveChange()
		{
			MDebugLog(nameof(OnCanvasActiveChange));

			if (uiParent != null)
				uiParent.SetActive(CanvasActive);
			if (uiActiveButtonImage != null)
				uiActiveButtonImage.color = GetGreenOrRed(CanvasActive);
		}

		public void ToggleCanvas()
		{
			if (useSync)
			{
				SetOwner();
				SyncedCanvasActive = !SyncedCanvasActive;
				RequestSerialization();
			}
			else
			{
				CanvasActive = !SyncedCanvasActive;
			}
		}

		public void SetCanvasActive(bool active)
		{
			if (useSync)
			{
				SetOwner();
				SyncedCanvasActive = active;
				RequestSerialization();
			}
			else
			{
				CanvasActive = active;
			}
		}
		#endregion
	}
}