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

		public int Score => IntData;
		public QuizAnswerType ExpectedAnswer => (QuizAnswerType)TurnData;
		protected QuizManager QuizManager => (QuizManager)contentManager;

		public bool HasSelectedAnswer => ExpectedAnswer != QuizAnswerType.None;
		public bool IsAnswerCorrect => ExpectedAnswer == QuizManager.CurQuizData.QuizAnswer;

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			for (int i = 0; i < selectAnswerDecoImages.Length; i++)
				selectAnswerDecoImages[i].color = WColorUtil.GetColorByBool(i == (int)ExpectedAnswer, WColorPreset.Green, WColorPreset.WhiteGray);
			base.OnTurnDataChange(changeState);
		}

		public void SelectAnswer(QuizAnswerType newAnswer, bool force = false)
		{
			force |= QuizManager.HACK_CanSelectAnswerAnyTime;
			if (force == false)
			{
				if (QuizManager.IsContentState(QuizContentState.SelectAnswer) == false)
					return;

				if (QuizManager.CanSelectAnswer == false)
					return;
			}

			TurnData = (int)newAnswer;
			SerializeData();
		}

		public virtual void OnWait() => WDebugLog($"{nameof(OnWait)}");
		public virtual void OnQuizTime() => WDebugLog($"{nameof(OnQuizTime)}");
		public virtual void OnSelectAnswer() => WDebugLog($"{nameof(OnSelectAnswer)}");
		public virtual void OnShowPlayerAnswer() => WDebugLog($"{nameof(OnShowPlayerAnswer)}");
		public virtual void OnCheckAnswer() => WDebugLog($"{nameof(OnCheckAnswer)}");
		public virtual void OnScoring()
		{
			WDebugLog($"{nameof(OnScoring)}");

			if (IsTargetPlayer() == false)
				return;

			if (IsAnswerCorrect)
			{
				if (QuizManager.AddScoreWhenCorrectAnswer)
				{
					IntData = Score + 1;
					SerializeData();
				}
			}
			else
			{
				if (QuizManager.DropPlayerWhenWrongAnswer)
				{
					ResetSeat();
					QuizManager.TP_WrongPos();
				}
				else if (QuizManager.SubScoreWhenWrongAnswer)
				{
					IntData = Score - 1;
					SerializeData();

					if (QuizManager.DropPlayerWhenZeroScore && (Score <= 0))
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