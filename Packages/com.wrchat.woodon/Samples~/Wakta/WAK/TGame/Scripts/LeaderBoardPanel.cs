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
	public class LeaderBoardPanel : MBase
	{
		[Header("_" + nameof(LeaderBoardPanel))]
		private TGameManager gameManager;
		private TGameUIPlayerBlock[] PlayerBlocks;
		
		public void Init(TGameManager gameManager)
		{
			this.gameManager = gameManager;
			PlayerBlocks = GetComponentsInChildren<TGameUIPlayerBlock>(true);
			for (int index = 0; index < TGameManager.PLAYER_COUNT; index++)
			{
				PlayerBlocks[index].Init();
				PlayerBlocks[index].SetShowCoinAlltime(true);
			}
		}

		public void UpdateUI()
		{
			int[] totalCoinsByIndex = gameManager.TotalCoinsByNumber;
			int[] sortedTotalCoinsByIndex = gameManager.SortedTotalCoinsByNumber;

			// index는 플레이어 등록 기준
			for (int index = 0; index < TGameManager.PLAYER_COUNT; index++)
			{
				string coinText = (totalCoinsByIndex[index] == NONE_INT * 5) ? "0" : totalCoinsByIndex[index].ToString();
				PlayerBlocks[index].SetCoin(coinText);

				string playerName = gameManager.GetPlayerName(index, false);
				Sprite playerSprite = gameManager.GetPlayerImage(index);
				PlayerBlocks[index].SetPlayer(playerName, playerSprite);
			}

			for (int order = TGameManager.PLAYER_COUNT - 1; order >= 0; order--)
				for (int index = 0; index < TGameManager.PLAYER_COUNT; index++)
					if (sortedTotalCoinsByIndex[order] == totalCoinsByIndex[index])
					{
						TGamePlayerUIState state =
							!gameManager.IsGaming ? TGamePlayerUIState.TakedCoin :
							(sortedTotalCoinsByIndex[order] == sortedTotalCoinsByIndex[0]) ? TGamePlayerUIState.GreatThief :
							(sortedTotalCoinsByIndex[order] == sortedTotalCoinsByIndex[6]) ? TGamePlayerUIState.MinCoin : TGamePlayerUIState.TakedCoin;

						PlayerBlocks[index].SetState(state);
						PlayerBlocks[index].transform.SetSiblingIndex(0);
					}

			// UI 정렬 갱신
			RectTransform[] rectTransforms = transform.GetComponentsInChildren<RectTransform>();
			foreach (RectTransform item in rectTransforms)
				LayoutRebuilder.ForceRebuildLayoutImmediate(item);
		}
	}
}