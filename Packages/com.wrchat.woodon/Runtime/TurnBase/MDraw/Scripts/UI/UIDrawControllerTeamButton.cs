using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawControllerTeamButton : MBase
	{
		[SerializeField] private TextMeshProUGUI buttonText;

		private UIDrawController drawController;
		private TeamType teamType;

		public void Init(UIDrawController drawController, TeamType teamType)
		{
			this.drawController = drawController;
			this.teamType = teamType;

			buttonText.text = $"{teamType}팀 공개";
		}

		public void Click()
		{
			drawController.ShowTeam(teamType);
		}
	}
}