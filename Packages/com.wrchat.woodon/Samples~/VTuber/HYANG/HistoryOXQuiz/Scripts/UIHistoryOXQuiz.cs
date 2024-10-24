using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.HistoryOXQuiz
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIHistoryOXQuiz : UIQuiz
	{
		[Header("_" + nameof(UIHistoryOXQuiz))]
		[SerializeField] private int startIndex;
		[SerializeField] private MValue curQuizIndex;

		private UISubjectButton[] subjectButtons;

		protected override void Init()
		{
			base.Init();

			subjectButtons = GetComponentsInChildren<UISubjectButton>();
			for (int i = 0; i < subjectButtons.Length; i++)
				subjectButtons[i].Init(this, i);
		}

		public void StartSelectFinalQuiz()
		{
			for (int i = 0; i < subjectButtons.Length; i++)
				subjectButtons[i].SetAnimatorValue(1);
		}
		
		public void SelectFinalQuiz(int index)
		{
			MDebugLog($"{nameof(SelectFinalQuiz)}: {index}");

			for (int i = 0; i < subjectButtons.Length; i++)
				subjectButtons[i].SetAnimatorValue(i == index ? 2 : 3);

			curQuizIndex.SetValue(startIndex + index);
		}
	}
}
