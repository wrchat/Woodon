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
		[SerializeField] protected WBool showResult;
		[SerializeField] protected WBool isCorrectAnswer;
		[SerializeField] protected WBool selectedThisAnswer;

		public virtual void UpdateUI(bool showResult, QuizAnswerType correctAnswer, QuizAnswerType selectedAnswer)
		{
			this.showResult.SetValue(showResult);
			isCorrectAnswer.SetValue(targetAnswerType == correctAnswer);
			selectedThisAnswer.SetValue(targetAnswerType == selectedAnswer);
		}
	}
}