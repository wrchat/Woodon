using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuiz : MBase
	{
		[Header("_" + nameof(UIQuiz))]
		[SerializeField] protected QuizManager quizManager;
		[SerializeField] protected UIQuizData[] quizDataUIs;

		protected virtual void Start()
		{
			Init();
		}
		
		protected virtual void Init()
		{
			foreach (UIQuizData quizDataUI in quizDataUIs)
				quizDataUI.Init(quizManager);

			quizManager.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public virtual void UpdateUI()
		{
			foreach (UIQuizData quizDataUI in quizDataUIs)
				quizDataUI.UpdateUI();
		}
	}
}
