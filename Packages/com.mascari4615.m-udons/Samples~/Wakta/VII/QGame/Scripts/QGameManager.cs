
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QGameManager : QuizManager
	{
		[Header("_" + nameof(QGameManager))]
		[SerializeField] private Animator showAnswerAnimator;
		[SerializeField] private TextMeshProUGUI showAnswerText;
		[SerializeField] private Image quizTextBackground;
		[SerializeField] private MTextSync curQuizIndexInputField;
		[SerializeField] private TextMeshProUGUI oxCountText;

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			bool isSelectTime = (CurGameState == (int)QuizGameState.QuizTime || CurGameState == (int)QuizGameState.SelectAnswer);

			if (!isSelectTime)
				CanSelectAnsewr = false;

			bool quizTextBackgourndActive = (CurGameState == (int)QuizGameState.QuizTime) || ((CurGameState == (int)QuizGameState.SelectAnswer) && CanSelectAnsewr);
			quizTextBackground.gameObject.SetActive(quizTextBackgourndActive);

			oxCountText.text =
				((int)CurGameState >= (int)QuizGameState.ShowPlayerAnswer) ?
				$"<color=#4C82C7>{answerCount[(int)QuizAnswerType.O]}</color> : <color=#B35153>{answerCount[(int)QuizAnswerType.X]}</color>" :
				string.Empty;
		}

		public override void OnQuizIndexChange()
		{
			base.OnQuizIndexChange();

			if (IsOwner())
				foreach (MTurnSeat turnSeat in TurnSeats)
					turnSeat.ResetData();
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
					showAnswerText.color = MColorUtil.GetColor(MColor.Blue);
					break;
				case QuizAnswerType.X:
					showAnswerText.text = "X";
					showAnswerText.color = MColorUtil.GetColor(MColor.Red);
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
			curQuizIndexInputField.SyncInputFieldText();
			string syncedText = curQuizIndexInputField.SyncText;

			if (!IsDigit(syncedText))
				return;

			int parse = int.Parse(syncedText) - 1;

			if (parse < 0 || parse >= QuizDatas.Length)
				return;

			curQuizIndex_MScore.SetScore(parse);
		}

		[SerializeField] private MTargetBool wallActiveViichanBool;
		[SerializeField] private CustomBool trueBool;
		[SerializeField] private SyncedBool wallActiveBool;
		[SerializeField] private ObjectActive wallActive;
		public void UpdateWall()
		{
			wallActive.SetCustomBool(wallActiveBool.Value ? wallActiveViichanBool : trueBool);
		}
	}
}