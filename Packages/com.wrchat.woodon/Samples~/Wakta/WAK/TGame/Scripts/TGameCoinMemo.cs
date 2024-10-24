using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameCoinMemo : MBase
	{
		[field: Header("_" + nameof(TGameCoinMemo))]
		public bool IsTargetRemainCoinBox { get; private set; }
		public int TargetCoinBoxIndex { get; private set; }

		public MValue MScore { get; private set; }
		private TGameManager gameManager;
		private CoinMemoPanel[] uis;

		public void Init(TGameManager gameManager)
		{
			this.gameManager = gameManager;
			MScore = GetComponentInChildren<MValue>(true);
			uis = transform.root.GetComponentsInChildren<CoinMemoPanel>(true);
			foreach (CoinMemoPanel ui in uis)
				ui.Init(gameManager, this);
		}

		public void ResetMemo()
		{
			MDebugLog(nameof(ResetMemo));
			foreach (CoinMemoPanel ui in uis)
				ui.ActualReset();
			
			if (gameManager.IsGaming == false)
				return;
			
			MScore.SetMinMaxValue(0, gameManager.Bank.MAX_COIN_BY_ROUND[gameManager.Data.CurRound] / 5);
			MScore.SetValue(0);
			UpdateUI();
		}

		public void UpdateUI()
		{
			foreach (CoinMemoPanel ui in uis)
				ui.UpdateMemo();
		}

		public void SetCurCoinBox(bool isRemainCoinBox, int index)
		{
			IsTargetRemainCoinBox = isRemainCoinBox;
			TargetCoinBoxIndex = index;

			foreach (CoinMemoPanel ui in uis)
				ui.A_SetCurCoinBox(isRemainCoinBox);
		}
	}
}