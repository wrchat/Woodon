using TMPro;
using UdonSharp;
using UnityEngine;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuizData : UIMData
	{
		[Header("_" + nameof(UIQuizData))]
		[SerializeField] protected TextMeshProUGUI[] curQuizIndexTexts;
		[SerializeField] protected GameObject[] waitTimeHiders;
		[SerializeField] protected CanvasGroup[] answerCanvasGroups;
		[SerializeField] protected CanvasGroup[] explainCanvasGroups;

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
		}

		public override void UpdateUI(MData mData)
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