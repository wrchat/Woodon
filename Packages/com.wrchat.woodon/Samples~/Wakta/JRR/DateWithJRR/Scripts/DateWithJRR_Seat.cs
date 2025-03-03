using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.JRR.DateWithJRR
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DateWithJRR_Seat : QuizSeat
	{
		[SerializeField] private WBool isCurStateSelectAnswer;
		[SerializeField] private WBool isExpectedAnswerNone;

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			WDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");
			UpdateSeat_();
		}

		protected override void UpdateSeat_()
		{
			base.UpdateSeat_();

			bool isCurStateSelectAnswer = QuizManager.IsContentState(QuizContentState.SelectAnswer);
			this.isCurStateSelectAnswer.SetValue(isCurStateSelectAnswer);

			bool isExpectedAnswerNone = ExpectedAnswer == QuizAnswerType.None;
			this.isExpectedAnswerNone.SetValue(isExpectedAnswerNone);
		}
	}
}