using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.HistoryOXQuiz
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISubjectButton : MBase
	{
		[Header("_" + nameof(UISubjectButton))]
		[SerializeField] private MValue animatorState;

		private UIHistoryOXQuiz historyOXQuizUI;
		private int index;

		public void Init(UIHistoryOXQuiz historyOXQuiz, int index)
		{
			historyOXQuizUI = historyOXQuiz;
			this.index = index;
		}

		public void SetAnimatorValue(int value)
		{
			animatorState.SetValue(value);
		}

		public void Click()
		{
			historyOXQuizUI.SelectFinalQuiz(index);
		}
	}
}