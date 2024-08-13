using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIQuiz : MBase
	{
		[Header("_" + nameof(UIQuiz))]
		[SerializeField] protected QuizManager quizManager;
		[SerializeField] protected TextMeshProUGUI[] curQuizIndexTexts;
		[SerializeField] protected GameObject[] waitTimeHiders;
		[SerializeField] protected UIMData[] mDataUIs;

		private void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			quizManager.RegisterListener(this, nameof(UpdateUI));
		}

		public virtual void UpdateUI()
		{
			foreach (TextMeshProUGUI curQuizIndexText in curQuizIndexTexts)
				curQuizIndexText.text = (quizManager.CurQuizIndex + 1).ToString();

			bool nowWaiting = quizManager.CurGameState == (int)QuizGameState.Wait;
			foreach (GameObject waitTimeHider in waitTimeHiders)
				waitTimeHider.SetActive(nowWaiting);

			foreach (UIMData mDataUI in mDataUIs)
				mDataUI.UpdateUI(quizManager.CurQuizData);
		}
	}
}