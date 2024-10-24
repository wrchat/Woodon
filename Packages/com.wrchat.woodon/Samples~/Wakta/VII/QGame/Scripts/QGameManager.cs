using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615.Project.Wakta.VII.QGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QGameManager : QuizManager
	{
		[Header("_" + nameof(QGameManager))]
		[SerializeField] private Animator showAnswerAnimator;
		[SerializeField] private TextMeshProUGUI showAnswerText;
		[SerializeField] private Image quizTextBackground;
		[SerializeField] private MString curQuizIndexMString;
		[SerializeField] private UIMString curQuizIndexStringUI;
		[SerializeField] private TextMeshProUGUI oxCountText;

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			bool isSelectTime = (CurGameState == (int)QuizGameState.ShowQuiz || CurGameState == (int)QuizGameState.SelectAnswer);

			if (!isSelectTime)
				CanSelectAnsewr = false;

			bool quizTextBackgourndActive = (CurGameState == (int)QuizGameState.ShowQuiz) || ((CurGameState == (int)QuizGameState.SelectAnswer) && CanSelectAnsewr);
			quizTextBackground.gameObject.SetActive(quizTextBackgourndActive);

			oxCountText.text =
				((int)CurGameState >= (int)QuizGameState.ShowPlayerAnswer) ?
				$"<color=#4C82C7>{answerCount[(int)QuizAnswerType.O]}</color> : <color=#B35153>{answerCount[(int)QuizAnswerType.X]}</color>" :
				string.Empty;
		}

		[SerializeField] private QGameSeat[] qGameSeats;

		public override void OnSelectAnswer()
		{
			base.OnSelectAnswer();

			CanSelectAnsewr = true;
			SendCustomEventDelayedSeconds(nameof(TurnOffQuizUI), 15f);
        }

		public void TurnOffQuizUI()
		{
			if (CurGameState != (int)QuizGameState.SelectAnswer)
				return;

			CanSelectAnsewr = false;
			UpdateStuff();
		}
	
		public override void OnCheckAnswer()
		{
			base.OnCheckAnswer();
			switch (CurQuizData.QuizAnswer)
			{
				case QuizAnswerType.O:
					showAnswerText.text = "O";
					showAnswerText.color = MColorUtil.GetColor(MColorPreset.Blue);
					break;
				case QuizAnswerType.X:
					showAnswerText.text = "X";
					showAnswerText.color = MColorUtil.GetColor(MColorPreset.Red);
					break;
				case QuizAnswerType.String:
					showAnswerText.text = CurQuizData.QuizAnswerString;
					showAnswerText.color = Color.white;
					break;
				default:
					showAnswerText.text = string.Empty;
					break;
			}

			showAnswerAnimator.SetTrigger("POP");
		}

		public void UpdateQuizIndexByInputField()
		{
			curQuizIndexStringUI.SubmitInputField();
			string syncedText = curQuizIndexMString.Value;

			if (!IsDigit(syncedText))
				return;

			int parse = int.Parse(syncedText) - 1;

			if (parse < 0 || parse >= QuizDatas.Length)
				return;

			curQuizIndex.SetValue(parse);
		}

		[SerializeField] private MTargetBool wallActiveViichanBool;
		[SerializeField] private MBool trueBool;
		[SerializeField] private MBool wallActiveBool;
		[SerializeField] private ObjectActive wallActive;
		public void UpdateWall()
		{
			wallActive.SetMBool(wallActiveBool.Value ? wallActiveViichanBool : trueBool);
		}
	}
}