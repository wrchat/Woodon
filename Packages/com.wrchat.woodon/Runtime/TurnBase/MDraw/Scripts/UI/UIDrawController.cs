using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawController : MBase
	{
		private DrawManager drawManager;
		private UIDrawControllerTeamButton[] teamButtons;

		private void Start()
		{
			teamButtons = GetComponentsInChildren<UIDrawControllerTeamButton>();

			for (int i = 0; i < teamButtons.Length; i++)
				teamButtons[i].Init(this, (TeamType)i);
		}

		public void Init(DrawManager drawManager) => this.drawManager = drawManager;

		public void ShowTeam(TeamType teamType) => drawManager.ShowTeam(teamType);
	}
}