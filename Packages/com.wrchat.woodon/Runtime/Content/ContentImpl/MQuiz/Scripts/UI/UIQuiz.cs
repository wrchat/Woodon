using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-5000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuiz : UIContentBase
	{
		[Header("_" + nameof(UIQuiz))]
		[SerializeField] protected TextMeshProUGUI[] curQuizIndexTexts;
		[SerializeField] protected GameObject[] waitTimeHiders;
		[SerializeField] protected CanvasGroup[] answerCanvasGroups;
		[SerializeField] protected CanvasGroup[] explainCanvasGroups;
		[SerializeField] protected MAnimator[] mAnimatorsByCurQuizAnswer;
		[SerializeField] protected UIWDataContainer[] quizDataUIs;

		[SerializeField] protected UIQuizAnswerBlock[] answerBlocks;
		[SerializeField] protected QuizSeat quizSeat; // 대표자

		protected QuizManager QuizManager => (QuizManager)contentManager;

		protected override void Init()
		{
			base.Init();

			if (quizSeat != null)
				quizSeat.RegisterListener(this, nameof(UpdateQuizDataUIs));
		}

		public override void UpdateUI()
		{
			base.UpdateUI();

			UpdateIndexUI();
			UpdateWaitTimeHiders();
			UpdateQuizDataUIs();

			bool isCurStateCheckAnswer = QuizManager.IsContentState(QuizContentState.CheckAnswer);
			foreach (CanvasGroup answerCanvasGroup in answerCanvasGroups)
				WUtil.SetCanvasGroupActive(answerCanvasGroup, isCurStateCheckAnswer);

			bool isExplainTime = QuizManager.ContentState >= (int)QuizContentState.Explaining;
			foreach (CanvasGroup explainCanvasGroup in explainCanvasGroups)
				WUtil.SetCanvasGroupActive(explainCanvasGroup, isExplainTime);
		}

		private void UpdateIndexUI()
		{
			if (QuizManager.CurQuizIndex == NONE_INT)
			{
				foreach (TextMeshProUGUI curQuizIndexText in curQuizIndexTexts)
					curQuizIndexText.text = string.Empty;
				return;
			}

			foreach (TextMeshProUGUI curQuizIndexText in curQuizIndexTexts)
				curQuizIndexText.text = (QuizManager.CurQuizIndex + 1).ToString();
		}

		private void UpdateWaitTimeHiders()
		{
			bool nowWaiting = QuizManager.IsContentState(QuizContentState.Wait);
			foreach (GameObject waitTimeHider in waitTimeHiders)
				waitTimeHider.SetActive(nowWaiting);
		}

		public void UpdateQuizDataUIs()
		{
			// UIWDataContainer
			foreach (UIWDataContainer quizDataUI in quizDataUIs)
				quizDataUI.UpdateUI(QuizManager.CurQuizData);

			// MAnimator
			foreach (MAnimator mAnimator in mAnimatorsByCurQuizAnswer)
				mAnimator.SetInt_L((int)QuizManager.CurQuizData.QuizAnswer);

			// AnswerBlock
			bool isCurStateAfterCheckAnswer = QuizManager.ContentState >= (int)QuizContentState.CheckAnswer;
			foreach (UIQuizAnswerBlock answerBlock in answerBlocks)
			{
				if (quizSeat)
				{
					answerBlock.UpdateUI(isCurStateAfterCheckAnswer, QuizManager.CurQuizData.QuizAnswer, quizSeat.ExpectedAnswer);
				}
				else
				{
					answerBlock.UpdateUI(isCurStateAfterCheckAnswer, QuizManager.CurQuizData.QuizAnswer, QuizAnswerType.None);
				}
			}
		}
	}
}
