using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizManager : MTurnSeatManager
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
		[SerializeField] protected MValue curQuizIndex_MScore;
		[SerializeField] private MTextSync seatIndexInputField;

		[SerializeField] protected MSFXManager sfxManager;
		[SerializeField] private AudioClip waitSFX;
		[SerializeField] private AudioClip quizTimeSFX;
		[SerializeField] private AudioClip selectAnswerSFX;
		[SerializeField] private AudioClip showPlayerAnswerSFX;
		[SerializeField] private AudioClip checkAnswerSFX;
		[SerializeField] private AudioClip scoringSFX;

		[field: Header("_" + nameof(QuizManager) + "_GameRule")]
		[field: SerializeField] public bool GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER { get; private set; } = false;
		[field: SerializeField] public bool GameRule_SUB_SCORE_WHEN_WRONG_ANSWER { get; private set; } = false;
		[field: SerializeField] public bool GameRule_DROP_PLAYER_WHEN_WRONG_ANSWER { get; private set; } = false;
		[field: SerializeField] public bool GameRule_DROP_PLAYER_WHEN_ZERO_SCORE { get; private set; } = false;

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
		[UdonSynced(), FieldChangeCallback(nameof(CurQuizIndex))]
		private int _curQuizIndex = 0;

		protected override void OnGameStateChange(int origin, int value)
		{
			if (value != origin)
			{
				if (value == (int)QuizGameState.Wait) OnWait();
				else if (value == (int)QuizGameState.QuizTime) OnQuizTime();
				else if (value == (int)QuizGameState.SelectAnswer) OnSelectAnswer();
				else if (value == (int)QuizGameState.ShowPlayerAnswer) OnShowPlayerAnswer();
				else if (value == (int)QuizGameState.CheckAnswer) OnCheckAnswer();
				else if (value == (int)QuizGameState.Scoring) OnScoring();
			}
		}

		public QuizData CurQuizData => QuizDatas[CurQuizIndex];

		protected override void Start()
		{
			curQuizIndex_MScore.SetMinMaxValue(0, QuizDatas.Length - 1);
			base.Start();
			UpdateStuff();
		}

		public bool CanSelectAnsewr { get; set; } = true;

		public void SetCurGameState(QuizGameState newGameState)
		{
			MDebugLog($"{nameof(SetCurGameState)}, {newGameState}");
			SetGameState((int)newGameState);
		}
		public void SetCurGameState_Wait() => SetCurGameState(QuizGameState.Wait);
		public void SetCurGameState_QuizTime() => SetCurGameState(QuizGameState.QuizTime);
		public void SetCurGameState_SelectAnswer() => SetCurGameState(QuizGameState.SelectAnswer);
		public void SetCurGameState_ShowPlayerAnswer() => SetCurGameState(QuizGameState.ShowPlayerAnswer);
		public void SetCurGameState_CheckAnswer() => SetCurGameState(QuizGameState.CheckAnswer);
		public void SetCurGameState_Scoring() => SetCurGameState(QuizGameState.Scoring);

		public override void UpdateStuff()
		{
			base.UpdateStuff();
			
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.UpdateStuff();

			for (int i = 0; i < stateButtonImages.Length; i++)
				stateButtonImages[i].color = MColorUtil.GetColorByBool(i == CurGameState, MColor.Green, MColor.Gray);

			if (curQuizIndexText)
				curQuizIndexText.text = (_curQuizIndex + 1).ToString();

			answerCount = new int[(int)QuizAnswerType.None + 1];
			foreach (QuizSeat quizSeat in TurnSeats)
				answerCount[(int)quizSeat.ExpectedAnswer]++;

			foreach (TextMeshProUGUI curQuizText in curQuizTexts)
			{
				curQuizText.text = CurQuizData.Quiz;
				// curQuizText.gameObject.SetActive(!(CurGameState == QuizGameState.Wait || CurGameState == QuizGameState.FindWrongPlayer));
			}

			bool isCurStateWaiting = CurGameState == (int)QuizGameState.Wait;
			foreach (GameObject waitTimeObject in waitTimeObjects)
				waitTimeObject.SetActive(isCurStateWaiting);

			Sprite targetSprite = (waitTimeQuizSprite && isCurStateWaiting) ? waitTimeQuizSprite : CurQuizData.QuizSprite;
			foreach (Image curQuizImage in curQuizImages)
				curQuizImage.sprite = targetSprite;

			SendEvents();
		}

		public void NextQuiz() => SetQuizIndex(CurQuizIndex + 1);
		public void PrevQuiz() => SetQuizIndex(CurQuizIndex - 1);
		public void UpdateQuizIndex()
		{
			// Called By MScore Event
			if (IsOwner())
				SetQuizIndex(curQuizIndex_MScore.SyncedValue);
		}
		public virtual void SetQuizIndex(int newIndex)
		{
			if (0 <= newIndex && newIndex <= QuizDatas.Length - 1)
			{
				if (IsOwner())
					foreach (MTurnSeat turnSeat in TurnSeats)
						turnSeat.ResetData();

				SetOwner();
				CurGameState = (int)QuizGameState.Wait;
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
				TP(TurnSeats[seatIndex - 1].transform);
		}

		public void TP_WrongPos()
		{
			if (wrongPos)
				TP(wrongPos);
		}

		public virtual void OnWait()
		{
			MDebugLog($"{nameof(OnWait)}");
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.OnWait();
			PlaySFX(waitSFX);
		}
		public virtual void OnQuizTime()
		{
			MDebugLog($"{nameof(OnQuizTime)}");
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.OnQuizTime();
			PlaySFX(quizTimeSFX);
		}
		public virtual void OnSelectAnswer()
		{
			MDebugLog($"{nameof(OnSelectAnswer)}");
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.OnSelectAnswer();
			PlaySFX(selectAnswerSFX);
		}
		public virtual void OnShowPlayerAnswer()
		{
			MDebugLog($"{nameof(OnShowPlayerAnswer)}");
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.OnShowPlayerAnswer();
			PlaySFX(showPlayerAnswerSFX);
		}
		public virtual void OnCheckAnswer()
		{
			MDebugLog($"{nameof(OnCheckAnswer)}");
			foreach (QuizSeat quizSeat in TurnSeats)
				quizSeat.OnCheckAnswer();
			PlaySFX(checkAnswerSFX);
		}
		public virtual void OnScoring()
		{
			MDebugLog($"{nameof(OnScoring)}");
			foreach (QuizSeat quizSeat in TurnSeats)
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
			MTurnSeat localplayerSeat = GetLocalPlayerSeat();

			if (localplayerSeat)
				localplayerSeat.SetData((int)quizAnswerType);
		}
	}
}