
using System.Configuration;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public enum QuizGameState
	{
		Wait,
		QuizTime,
		SelectAnswer,
		ShowPlayerAnswer,
		CheckAnswer,
		Scoring
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizManager : MEventSender
	{
		[Header("_" + nameof(QuizManager))]
		[SerializeField] protected int playerCount = 10;

		[SerializeField] protected TextMeshProUGUI curQuizIndexText;
		[SerializeField] protected TextMeshProUGUI[] curQuizTexts;
		[SerializeField] protected Image[] curQuizImages;
		[SerializeField] private GameObject[] waitTimeObjects;
		[SerializeField] private Sprite waitTimeQuizSprite;
		[SerializeField] private Transform wrongPos;
		[SerializeField] private Transform quizDatasParent;
		[SerializeField] private Transform quizSeatsParent;
		[SerializeField] private Image[] stateButtonImages;
		[SerializeField] protected MScore curQuizIndex_MScore;
		[SerializeField] private MTextSync seatIndexInputField;

		[SerializeField] protected MSFXManager sfxManager;
		[SerializeField] private AudioClip waitSFX;
		[SerializeField] private AudioClip quizTimeSFX;
		[SerializeField] private AudioClip selectAnswerSFX;
		[SerializeField] private AudioClip showPlayerAnswerSFX;
		[SerializeField] private AudioClip checkAnswerSFX;
		[SerializeField] private AudioClip scoringSFX;

		[Header("_" + nameof(QuizManager) + "_GameRule")]
		[SerializeField] private bool gameRule_AddScoreWhenCorrectAnswer = false;
		public bool GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER => gameRule_AddScoreWhenCorrectAnswer;
		[SerializeField] private bool gameRule_SubScoreWhenWrongAnswer = false;
		public bool GameRule_SUB_SCORE_WHEN_WRONG_ANSWER => gameRule_SubScoreWhenWrongAnswer;
		[SerializeField] private bool gameRule_DropPlayerWhenWrongAnswer = false;
		public bool GameRule_DROP_PLAYER_WHEN_WRONG_ANSWER => gameRule_DropPlayerWhenWrongAnswer;
		[SerializeField] private bool gameRule_DropPlayerWhenZeroScore = false;
		public bool GameRule_DROP_PLAYER_WHEN_ZERO_SCORE => gameRule_DropPlayerWhenZeroScore;

		protected int[] answerCount = new int[10];

		public QuizData[] QuizDatas
		{
			get
			{
				if (_quizDatas == null)
					_quizDatas = quizDatasParent.GetComponentsInChildren<QuizData>();

				return _quizDatas;
			}
		}
		private QuizData[] _quizDatas;

		public int CurQuizIndex
		{
			get => _curQuizIndex;
			set
			{
				MDebugLog($"{nameof(CurQuizData)} Changed, {CurQuizData} to {value}");
				_curQuizIndex = value;
				OnQuizIndexChange();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurQuizIndex))]
		private int _curQuizIndex = 0;

		public QuizGameState CurGameState
		{
			get => _curGameState;
			set
			{
				MDebugLog($"{nameof(CurGameState)} Changed, {CurGameState} to {value}");

				if (value != CurGameState)
				{
					if (value == QuizGameState.Wait) OnWait();
					else if (value == QuizGameState.QuizTime) OnQuizTime();
					else if (value == QuizGameState.SelectAnswer) OnSelectAnswer();
					else if (value == QuizGameState.ShowPlayerAnswer) OnShowPlayerAnswer();
					else if (value == QuizGameState.CheckAnswer) OnCheckAnswer();
					else if (value == QuizGameState.Scoring) OnScoring();
				}

				_curGameState = value;
				UpdateStuff();
				SendEvents();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurGameState))]
		private QuizGameState _curGameState = QuizGameState.Wait;

		public QuizData CurQuizData => QuizDatas[CurQuizIndex];

		public QuizSeat[] QuizSeats
		{
			get
			{
				if (_quizSeats == null || _quizSeats.Length == 0)
					_quizSeats = quizSeatsParent.GetComponentsInChildren<QuizSeat>();

				return _quizSeats;
			}
		}
		[SerializeField] private QuizSeat[] _quizSeats;

		protected virtual void Start()
		{
			curQuizIndex_MScore.SetMinMaxScore(0, QuizDatas.Length - 1);

			for (int i = 0; i < QuizSeats.Length; i++)
				QuizSeats[i].Init(this, i + 1);

			UpdateStuff();
		}

		public bool CanSelectAnsewr { get; set; } = true;

		public void SetCurGameState(QuizGameState newGameState)
		{
			MDebugLog($"{nameof(SetCurGameState)}, {newGameState}");

			SetOwner();
			CurGameState = newGameState;
			RequestSerialization();
		}
		public void SetCurGameState_Wait() => SetCurGameState(QuizGameState.Wait);
		public void SetCurGameState_QuizTime() => SetCurGameState(QuizGameState.QuizTime);
		public void SetCurGameState_SelectAnswer() => SetCurGameState(QuizGameState.SelectAnswer);
		public void SetCurGameState_ShowPlayerAnswer() => SetCurGameState(QuizGameState.ShowPlayerAnswer);
		public void SetCurGameState_CheckAnswer() => SetCurGameState(QuizGameState.CheckAnswer);
		public void SetCurGameState_Scoring() => SetCurGameState(QuizGameState.Scoring);

		public virtual void UpdateStuff()
		{
			MDebugLog($"{nameof(UpdateStuff)}");

			foreach (var quizSeat in QuizSeats)
				quizSeat.UpdateStuff();

			for (int i = 0; i < stateButtonImages.Length; i++)
				stateButtonImages[i].color = MColorUtil.GetColorByBool(i == (int)CurGameState, MColor.Green, MColor.Gray);

			if (curQuizIndexText)
				curQuizIndexText.text = (_curQuizIndex + 1).ToString();

			answerCount = new int[(int)QuizAnswerType.None + 1];
			foreach (var quizSeat in QuizSeats)
				answerCount[(int)quizSeat.ExpectedAnswer]++;

			foreach (var curQuizText in curQuizTexts)
			{
				curQuizText.text = CurQuizData.Quiz;
				// curQuizText.gameObject.SetActive(!(CurGameState == QuizGameState.Wait || CurGameState == QuizGameState.FindWrongPlayer));
			}

			bool isCurStateWaiting = CurGameState == QuizGameState.Wait;
			foreach (GameObject waitTimeObject in waitTimeObjects)
				waitTimeObject.SetActive(isCurStateWaiting);

			Sprite targetSprite = (waitTimeQuizSprite && isCurStateWaiting) ? waitTimeQuizSprite : CurQuizData.QuizSprite;
			foreach (var curQuizImage in curQuizImages)
				curQuizImage.sprite = targetSprite;

			SendEvents();
		}

		public void NextQuiz() => SetQuizIndex(CurQuizIndex + 1);
		public void PrevQuiz() => SetQuizIndex(CurQuizIndex - 1);
		public void UpdateQuizIndex()
		{
			// Called By MScore Event
			if (IsOwner())
				SetQuizIndex(curQuizIndex_MScore.SyncedScore);
		}
		public virtual void SetQuizIndex(int newIndex)
		{
			if (0 <= newIndex && newIndex <= QuizDatas.Length - 1)
			{
				if (IsOwner())
					foreach (var quizSeat in QuizSeats)
						quizSeat.ResetAnswer();

				SetOwner();
				CurGameState = QuizGameState.Wait;
				CurQuizIndex = newIndex;
				RequestSerialization();
			}
		}

		public virtual void OnQuizIndexChange()
		{
			UpdateStuff();
			SendEvents();
		}

		public void TeleportToSeat()
		{
			if (!IsDigit(seatIndexInputField.SyncText))
				return;

			int seatIndex = int.Parse(seatIndexInputField.SyncText);

			if (0 < seatIndex && seatIndex <= playerCount)
				TP(QuizSeats[seatIndex - 1].transform);
		}

		public void TP_WrongPos()
		{
			if (wrongPos)
				TP(wrongPos);
		}

		public virtual void OnWait()
		{
			MDebugLog($"{nameof(OnWait)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnWait();
			PlaySFX(waitSFX);
		}
		public virtual void OnQuizTime()
		{
			MDebugLog($"{nameof(OnQuizTime)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnQuizTime();
			PlaySFX(quizTimeSFX);
		}
		public virtual void OnSelectAnswer()
		{
			MDebugLog($"{nameof(OnSelectAnswer)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnSelectAnswer();
			PlaySFX(selectAnswerSFX);
		}
		public virtual void OnShowPlayerAnswer()
		{
			MDebugLog($"{nameof(OnShowPlayerAnswer)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnShowPlayerAnswer();
			PlaySFX(showPlayerAnswerSFX);
		}
		public virtual void OnCheckAnswer()
		{
			MDebugLog($"{nameof(OnCheckAnswer)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnCheckAnswer();
			PlaySFX(checkAnswerSFX);
		}
		public virtual void OnScoring()
		{
			MDebugLog($"{nameof(OnScoring)}");
			foreach (var quizSeat in QuizSeats)
				quizSeat.OnScoring();
			PlaySFX(scoringSFX);
		}

		private void PlaySFX(AudioClip audioClip)
		{
			if (audioClip == null)
				return;

			sfxManager.PlaySFX(audioClip);
		}

		public void SelectAnswerO() => SelectAnswer(QuizAnswerType.O);
		public void SelectAnswerX() => SelectAnswer(QuizAnswerType.X);
		public void SelectAnswerOne() => SelectAnswer(QuizAnswerType.One);
		public void SelectAnswerTwo() => SelectAnswer(QuizAnswerType.Two);
		public void SelectAnswerThree() => SelectAnswer(QuizAnswerType.Three);
		public void SelectAnswerFour() => SelectAnswer(QuizAnswerType.Four);
		public void SelectAnswerFive() => SelectAnswer(QuizAnswerType.Five);
		public void SelectAnswer(QuizAnswerType quizAnswerType)
		{
			QuizSeat localplayerQuizSeat = null;
			foreach (QuizSeat quizSeat in QuizSeats)
				if (quizSeat.IsLocalPlayerOwner)
				{
					localplayerQuizSeat = quizSeat;
					break;
				}

			if (localplayerQuizSeat == null)
				return;

			localplayerQuizSeat.SelectAnswer(quizAnswerType);
		}
	}
}