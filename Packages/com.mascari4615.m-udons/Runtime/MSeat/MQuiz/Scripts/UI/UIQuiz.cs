using UdonSharp;
using UnityEngine;

namespace Mascari4615
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
			quizManager.RegisterListener(this, nameof(UpdateUI));

			foreach (UIQuizData quizDataUI in quizDataUIs)
				quizDataUI.Init(quizManager);
		}

		public virtual void UpdateUI()
		{
			foreach (UIQuizData quizDataUI in quizDataUIs)
				quizDataUI.UpdateUI();
		}
	}
}
