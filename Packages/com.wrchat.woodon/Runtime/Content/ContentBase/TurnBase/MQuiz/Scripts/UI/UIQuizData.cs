using TMPro;
using UdonSharp;
using UnityEngine;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuizData : UIMDataContainer
	{
		[Header("_" + nameof(UIQuizData))]
		[SerializeField] protected TextMeshProUGUI[] curQuizIndexTexts;
		[SerializeField] protected GameObject[] waitTimeHiders;
		[SerializeField] protected CanvasGroup[] answerCanvasGroups;
		[SerializeField] protected CanvasGroup[] explainCanvasGroups;
		[SerializeField] protected MAnimator[] mAnimatorsByGameState;
		[SerializeField] protected MAnimator[] mAnimatorsByCurQuizAnswer;

		protected QuizManager quizManager;

		public void Init(QuizManager quizManager)
		{
			this.quizManager = quizManager;
		}

		public virtual void UpdateUI()
		{
			if (quizManager.IsInited == false)
				return;

			if (quizManager.CurQuizIndex == NONE_INT)
				return;

			foreach (TextMeshProUGUI curQuizIndexText in curQuizIndexTexts)
				curQuizIndexText.text = (quizManager.CurQuizIndex + 1).ToString();

			bool nowWaiting = quizManager.CurGameState == (int)QuizGameState.Wait;
			foreach (GameObject waitTimeHider in waitTimeHiders)
				waitTimeHider.SetActive(nowWaiting);

			UpdateUI(quizManager.CurQuizData);

			foreach (MAnimator mAnimator in mAnimatorsByGameState)
				mAnimator.SetInt_L(quizManager.CurGameState);

			foreach (MAnimator mAnimator in mAnimatorsByCurQuizAnswer)
				mAnimator.SetInt_L((int)quizManager.CurQuizData.QuizAnswer);
		}

		public override void UpdateUI(MDataContainer mData)
		{
			base.UpdateUI(mData);

			bool checkAnswer = quizManager.CurGameState == (int)QuizGameState.CheckAnswer;
			foreach (CanvasGroup answerCanvasGroup in answerCanvasGroups)
				SetCanvasGroupActive(answerCanvasGroup, checkAnswer);

			bool explain = quizManager.CurGameState >= (int)QuizGameState.Explaining;
			foreach (CanvasGroup explainCanvasGroup in explainCanvasGroups)
				SetCanvasGroupActive(explainCanvasGroup, explain);
		}
	}
}