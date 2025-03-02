using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuizAnswerBlock : WBase
	{
		[Header("_" + nameof(UIQuizAnswerBlock))]
		[SerializeField] protected QuizAnswerType targetAnswerType;
		[SerializeField] protected MBool showResult;
		[SerializeField] protected MBool isCorrectAnswer;
		[SerializeField] protected MBool selectedThisAnswer;

		public virtual void UpdateUI(bool showResult, QuizAnswerType correctAnswer, QuizAnswerType selectedAnswer)
		{
			this.showResult.SetValue(showResult);
			isCorrectAnswer.SetValue(targetAnswerType == correctAnswer);
			selectedThisAnswer.SetValue(targetAnswerType == selectedAnswer);
		}
	}
}