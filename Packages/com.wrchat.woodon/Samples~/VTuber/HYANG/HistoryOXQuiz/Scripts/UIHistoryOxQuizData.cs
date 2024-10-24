using UdonSharp;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.HistoryOXQuiz
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIHistoryOxQuizData : UIQuizData
	{
		// [Header("_" + nameof(UIHistoryOxQuizData))]

		public override void UpdateUI(MData mData)
		{
			base.UpdateUI(mData);

			// bool checkAnswer = quizManager.CurGameState == (int)QuizGameState.CheckAnswer;
			// dataImages[1].gameObject.SetActive(checkAnswer);

			// bool explain = quizManager.CurGameState == (int)QuizGameState.Explaining;
			// dataImages[2].gameObject.SetActive(explain);
		}
	}
}