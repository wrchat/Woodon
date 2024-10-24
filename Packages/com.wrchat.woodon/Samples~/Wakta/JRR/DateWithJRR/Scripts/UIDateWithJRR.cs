using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.JRR.DateWithJRR
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDateWithJRR : UIQuizData
	{
		[SerializeField] private GameObject temp;

		public override void UpdateUI()
		{
			base.UpdateUI();

			DateWithJRR_Manager dateWithJRR_Manager = (DateWithJRR_Manager)quizManager;
			temp.SetActive(dateWithJRR_Manager.CurDetailAnswerIndex == 5);
		}
	}
}