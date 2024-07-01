using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizSeat : MTurnSeat
	{
		public int Score => Data;
		public QuizAnswerType ExpectedAnswer => (QuizAnswerType)TurnData;

		public bool HasSelectedAnswer => ExpectedAnswer != QuizAnswerType.None;
		public bool IsAnswerCorrect => ExpectedAnswer == quizManager.CurQuizData.QuizAnswer;

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			for (int i = 0; i < selectAnswerDecoImages.Length; i++)
				selectAnswerDecoImages[i].color = MColorUtil.GetColorByBool(i == (int)ExpectedAnswer, MColor.Green, MColor.WhiteGray);
			base.OnTurnDataChange(changeState);
		}

		[SerializeField] private Image[] selectAnswerDecoImages;
		protected QuizManager quizManager;
		private string[] answerToString =
		{
			"O", "X", "1", "2", "3", "4", "5", string.Empty
		};

		private void Start()
		{
			OnOwnerChange();
			OnTurnDataChange(DataChangeState.None);
		}

		public void SelectAnswer(QuizAnswerType newAnswer)
		{
			if (quizManager.CurGameState != (int)QuizGameState.SelectAnswer)
				return;

			if (!quizManager.CanSelectAnsewr)
				return;

			SetTurnData((int)newAnswer);
		}

		public void SelectAnswerForce(QuizAnswerType newAnswer)
		{
			SetTurnData((int)newAnswer);
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

			if (IsLocalPlayerID(OwnerID) == false)
				return;

			if (IsAnswerCorrect)
			{
				if (quizManager.GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER)
					SetData(Score + 1);
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
					SetData(Score - 1);

					if (quizManager.GameRule_DROP_PLAYER_WHEN_ZERO_SCORE && (Score <= 0))
					{
						ResetSeat();
						quizManager.TP_WrongPos();
					}
				}
			}
		}
	}
}