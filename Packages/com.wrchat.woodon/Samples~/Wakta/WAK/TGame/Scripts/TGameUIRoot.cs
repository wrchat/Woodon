using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameUIRoot : MBase
	{
		[field: Header("_" + nameof(TGameUIRoot))]
		private TGameUIRoundPanel[] roundPanels;
		public LeaderBoardPanel LeaderBoardPanel { get; private set; }

		private TGameManager gameManager;

		public void Init(TGameManager gameManager)
		{
			this.gameManager = gameManager;

			roundPanels = GetComponentsInChildren<TGameUIRoundPanel>(true);
			LeaderBoardPanel = GetComponentInChildren<LeaderBoardPanel>(true);

			foreach (TGameUIRoundPanel roundPanel in roundPanels)
				roundPanel.Init();
			LeaderBoardPanel.Init(gameManager);
		}

		public void UpdateUI()
		{
			for (int i = 0; i < gameManager.Data.RoundDatas.Length; i++)
				roundPanels[i].UpdateUI(gameManager, i);
		}
	}
}