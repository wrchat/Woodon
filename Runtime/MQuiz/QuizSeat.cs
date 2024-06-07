
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizSeat : MBase
	{
		[Header("_" + nameof(QuizSeat))]
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(OwnerID))]
		private int _ownerID = NONE_INT;
		public int OwnerID
		{
			get => _ownerID;
			set
			{
				_ownerID = value;
				OnOwnerChange();
			}
		}

		public bool IsLocalPlayerOwner =>
			(Networking.LocalPlayer != null) &&
			(Networking.LocalPlayer.playerId == OwnerID);

		private void OnOwnerChange()
		{
			if (ownerObjectActive)
				ownerObjectActive.SetActive(IsLocalPlayerOwner);

			if (ownerNameText)
			{
				string ownerName;
				VRCPlayerApi ownerPlayerAPI = VRCPlayerApi.GetPlayerById(_ownerID);

				if (ownerPlayerAPI == null)
					ownerName = "-";
				else
					ownerName = ownerPlayerAPI.displayName;

				ownerNameText.text = ownerName;
			}

			if (seatIndexTexts != null && seatIndexTexts.Length > 0)
				seatIndexTexts[0].gameObject.SetActive(_ownerID != NONE_INT);

			if (quizManager)
				quizManager.UpdateStuff();
		}

		public bool HasSelectedAnswer => ExpectedAnswer != QuizAnswerType.None;
		public bool IsAnswerCorrect => ExpectedAnswer == quizManager.CurQuizData.QuizAnswer;
		public QuizAnswerType ExpectedAnswer
		{
			get => _expectedAnswer;
			set
			{
				_expectedAnswer = value;
				OnExpectedAnswerChange();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(ExpectedAnswer))]
		private QuizAnswerType _expectedAnswer = QuizAnswerType.None;

		private void OnExpectedAnswerChange()
		{
			MDebugLog($"{nameof(OnExpectedAnswerChange)}, {ExpectedAnswer}");

			for (int i = 0; i < selectAnswerDecoImages.Length; i++)
				selectAnswerDecoImages[i].color = MColorUtil.GetColorByBool(i == (int)_expectedAnswer, MColor.Green, MColor.WhiteGray);
			foreach (var expectedAnswerText in expectedAnswerTexts)
				expectedAnswerText.text = (ExpectedAnswer != QuizAnswerType.None) ? answerToString[(int)ExpectedAnswer] : string.Empty;
			foreach (var expectedAnswerImage in expectedAnswerImages)
			{
				// HACK
				// expectedAnswerImage.sprite = (ExpectedAnswer != QuizAnswerType.None) ? expectedAnswerSprites[(int)ExpectedAnswer] : noneAnswerSprite;
				expectedAnswerImage.sprite = (ExpectedAnswer != QuizAnswerType.None) ? null : noneAnswerSprite;
			}
			if (quizManager)
				quizManager.UpdateStuff();
		}

		[UdonSynced(UdonSyncMode.None)] private int _score = 1;
		public int Score => _score;
		public void SetScore(int newScore)
		{
			SetOwner();
			_score = newScore;
			RequestSerialization();
		}

		[SerializeField] private ObjectActive ownerObjectActive;
		[SerializeField] private TextMeshProUGUI ownerNameText;
		[SerializeField] private TextMeshProUGUI[] seatIndexTexts;
		[SerializeField] private TextMeshProUGUI[] expectedAnswerTexts;
		[SerializeField] private Image[] expectedAnswerImages;
		[SerializeField] private Sprite[] expectedAnswerSprites;
		[SerializeField] private Sprite noneAnswerSprite;
		[SerializeField] private Image[] selectAnswerDecoImages;
		protected QuizManager quizManager;
		private string[] answerToString =
		{
			"O", "X", "1", "2", "3", "4", "5", string.Empty
		};

		private void Start()
		{
			OnOwnerChange();
			OnExpectedAnswerChange();
		}

		public int Index { get; private set; }

		public void Init(QuizManager quizManager, int index)
		{
			this.quizManager = quizManager;
			Index = index;

			if (seatIndexTexts != null)
			{
				foreach (var seatIndexText in seatIndexTexts)
					seatIndexText.text = index.ToString();
			}
		}

		public virtual void UpdateStuff() { MDebugLog(nameof(UpdateStuff)); }

		public void UseSeat()
		{
			SetOwner();
			foreach (var quizSeat in quizManager.QuizSeats)
			{
				if (Networking.LocalPlayer.playerId == quizSeat.OwnerID)
					quizSeat.ResetSeat();
			}
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();
		}

		public void ResetSeat()
		{
			ResetAnswer();

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();
		}

		public void SelectAnswer(QuizAnswerType selectedAnswer)
		{
			if (quizManager.CurGameState != QuizGameState.SelectAnswer)
				return;

			if (!quizManager.CanSelectAnsewr)
				return;

			SetOwner();
			ExpectedAnswer = selectedAnswer;
			RequestSerialization();
		}

		public void SelectAnswerForce(QuizAnswerType selectedAnswer)
		{
			SetOwner();
			ExpectedAnswer = selectedAnswer;
			RequestSerialization();
		}

		public void ResetAnswer()
		{
			SetOwner();
			ExpectedAnswer = QuizAnswerType.None;
			RequestSerialization();
		}

		public void SelectAnswerO() => SelectAnswer(QuizAnswerType.O);
		public void SelectAnswerX() => SelectAnswer(QuizAnswerType.X);
		public void SelectAnswerOne() => SelectAnswer(QuizAnswerType.One);
		public void SelectAnswerTwo() => SelectAnswer(QuizAnswerType.Two);
		public void SelectAnswerThree() => SelectAnswer(QuizAnswerType.Three);
		public void SelectAnswerFour() => SelectAnswer(QuizAnswerType.Four);
		public void SelectAnswerFive() => SelectAnswer(QuizAnswerType.Five);

		public virtual void OnWait()
		{
			MDebugLog($"{nameof(OnWait)}");
		}
		public virtual void OnQuizTime()
		{
			MDebugLog($"{nameof(OnQuizTime)}");
		}
		public virtual void OnSelectAnswer()
		{
			MDebugLog($"{nameof(OnSelectAnswer)}");
		}
		public virtual void OnShowPlayerAnswer()
		{
			MDebugLog($"{nameof(OnShowPlayerAnswer)}");
		}
		public virtual void OnCheckAnswer()
		{
			MDebugLog($"{nameof(OnCheckAnswer)}");
		}
		public virtual void OnScoring()
		{
			MDebugLog($"{nameof(OnScoring)}");

			if (!IsLocalPlayerOwner)
				return;

			if (IsAnswerCorrect)
			{
				if (quizManager.GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER)
					SetScore(Score + 1);
			}
			else
			{
				if (quizManager.GameRule_DROP_PLAYER_WHEN_WRONG_ANSWER)
				{
					ResetSeat();
					quizManager.TP_WrongPos();
				}
				else if (quizManager.GameRule_SUB_SCORE_WHEN_WRONG_ANSWER)
				{
					SetScore(Score - 1);

					if (quizManager.GameRule_DROP_PLAYER_WHEN_ZERO_SCORE && (Score <= 0))
					{
						ResetSeat();
						quizManager.TP_WrongPos();
					}
				}
			}
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (player.playerId == OwnerID)
			{
				if (Networking.IsMaster)
					ResetSeat();
			}
		}
	}
}