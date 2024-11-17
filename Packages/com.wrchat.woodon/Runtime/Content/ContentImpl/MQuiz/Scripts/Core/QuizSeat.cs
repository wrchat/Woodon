using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizSeat : MSeat
	{
		[Header("_" + nameof(QuizSeat))]
		[SerializeField] private Image[] selectAnswerDecoImages;
		private readonly string[] answerToString =
		{
			"O", "X", "1", "2", "3", "4", "5", string.Empty
		};

		public int Score => IntData;
		public QuizAnswerType ExpectedAnswer => (QuizAnswerType)TurnData;
		protected QuizManager QuizManager => (QuizManager)contentManager;

		public bool HasSelectedAnswer => ExpectedAnswer != QuizAnswerType.None;
		public bool IsAnswerCorrect => ExpectedAnswer == QuizManager.CurQuizData.QuizAnswer;

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			for (int i = 0; i < selectAnswerDecoImages.Length; i++)
				selectAnswerDecoImages[i].color = MColorUtil.GetColorByBool(i == (int)ExpectedAnswer, MColorPreset.Green, MColorPreset.WhiteGray);
			base.OnTurnDataChange(changeState);
		}

		public void SelectAnswer(QuizAnswerType newAnswer, bool force = false)
		{
			if (force == false)
			{
				if (QuizManager.CurGameState != (int)QuizGameState.SelectAnswer)
					return;

				if (QuizManager.CanSelectAnswer == false)
					return;
			}

			TurnData = (int)newAnswer;
			SerializeData();
		}

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

			if (IsTargetPlayer() == false)
				return;

			if (IsAnswerCorrect)
			{
				if (QuizManager.GameRule_ADD_SCORE_WHEN_CORRECT_ANSWER)
				{
					IntData = Score + 1;
					SerializeData();
				}
			}
			else
			{
				if (QuizManager.GameRule_DROP_PLAYER_WHEN_WRONG_ANSWER)
				{
					ResetSeat();
					QuizManager.TP_WrongPos();
				}
				else if (QuizManager.GameRule_SUB_SCORE_WHEN_WRONG_ANSWER)
				{
					IntData = Score - 1;
					SerializeData();

					if (QuizManager.GameRule_DROP_PLAYER_WHEN_ZERO_SCORE && (Score <= 0))
					{
						ResetSeat();
						QuizManager.TP_WrongPos();
					}
				}
			}
		}

		#region HorribleEvents
		public void SelectAnswerO() => SelectAnswer(QuizAnswerType.O);
		public void SelectAnswerX() => SelectAnswer(QuizAnswerType.X);
		public void SelectAnswer1() => SelectAnswer(QuizAnswerType.One);
		public void SelectAnswer2() => SelectAnswer(QuizAnswerType.Two);
		public void SelectAnswer3() => SelectAnswer(QuizAnswerType.Three);
		public void SelectAnswer4() => SelectAnswer(QuizAnswerType.Four);
		public void SelectAnswer5() => SelectAnswer(QuizAnswerType.Five);
		#endregion
	}
}