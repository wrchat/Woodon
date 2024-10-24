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
	public class CoinMemoPanel : MBase
	{
		[Header("_" + nameof(CoinMemoPanel))]
		private TGameManager gameManager;
		private TGameCoinMemo coinMemo;
		[SerializeField] private GameObject[] selectIines;
		[SerializeField] private Image[] playerImages;
		[SerializeField] private CoinBox[] stealCoinBoxes;
		[SerializeField] private CoinBox[] remainCoinBoxes;
		[SerializeField] private TextMeshProUGUI coinText;

		public void Init(TGameManager gameManager, TGameCoinMemo coinMemo)
		{
			this.gameManager = gameManager;
			this.coinMemo = coinMemo;
		}

		public void ResetMemo() => coinMemo.ResetMemo();
		public void ActualReset()
		{
			if (gameManager.IsGaming == false)
				return;

			UpdateLock();

			foreach (CoinBox remainCoinBox in remainCoinBoxes)
			{
				if (!remainCoinBox.IsLock)
					remainCoinBox.UpdateCoin(0);
			}

			foreach (CoinBox stealCoinBox in stealCoinBoxes)
			{
				if (!stealCoinBox.IsLock)
					stealCoinBox.UpdateCoin(0);
			}

			SetCurCoinBox(false, 0);
			UpdateMemo();
		}

		// 갱신
		public void UpdateMemo()
		{
			MDebugLog(nameof(UpdateMemo));
			coinText.text = coinMemo.MScore.Value.ToString();

			if (gameManager.IsGaming == false)
				return;

			UpdateLock();

			// 자기 차례이기 전에, 자기 코인을 메모한 이후, 코인을 훔쳐가면
			// Lock된 StealCoinBox의 Coin을 조정하는 문제 방지
			if (stealCoinBoxes[coinMemo.TargetCoinBoxIndex].IsLock)
			{
				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
				{
					if (stealCoinBoxes[i].IsLock != false)
						SetCurCoinBox(false, i);
				}
			}

			if (coinMemo.IsTargetRemainCoinBox)
				remainCoinBoxes[coinMemo.TargetCoinBoxIndex].UpdateCoin(coinMemo.MScore.Value);
			else
				stealCoinBoxes[coinMemo.TargetCoinBoxIndex].UpdateCoin(coinMemo.MScore.Value);

			if (gameManager.IsGaming)
				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
					playerImages[i].sprite = gameManager.GetPlayerImage(gameManager.CurRoundData.NumberByOrder[i]);
			else
				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
					playerImages[i].sprite = null;

			int remainCoin = gameManager.Bank.MAX_COIN_BY_ROUND[gameManager.Data.CurRound];
			remainCoinBoxes[0].UpdateCoin(remainCoin);

			for (int i = 1; i < TGameManager.PLAYER_COUNT + 1; i++)
			{
				if (remainCoinBoxes[i].IsLock)
					remainCoin = remainCoinBoxes[i].Coin;
				else
					remainCoin -= stealCoinBoxes[i - 1].Coin;

				remainCoinBoxes[i].UpdateCoin(remainCoin);
			}

			// HACK
			for (int i = 0; i < remainCoinBoxes.Length; i++)
				remainCoinBoxes[i].UpdateColor(false);

			for (int i = 0; i < stealCoinBoxes.Length; i++)
				stealCoinBoxes[i].UpdateColor(i == coinMemo.TargetCoinBoxIndex);
		}

		public void SetCurCoinBox(bool isRemainCoinBox, int index)
		{
			coinMemo.SetCurCoinBox(isRemainCoinBox, index);
		}
		public void A_SetCurCoinBox(bool isRemainCoinBox)
		{
			CoinBox target = isRemainCoinBox ? remainCoinBoxes[coinMemo.TargetCoinBoxIndex] : stealCoinBoxes[coinMemo.TargetCoinBoxIndex];
			// coinMemo.MScore.SetCanvasActive(!target.IsLock);
			coinMemo.MScore.SetValue(target.Coin);

			for (int i = 0; i < remainCoinBoxes.Length; i++)
				remainCoinBoxes[i].UpdateColor(isRemainCoinBox && (i == coinMemo.TargetCoinBoxIndex));

			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				stealCoinBoxes[i].UpdateColor(!isRemainCoinBox && (i == coinMemo.TargetCoinBoxIndex));
				selectIines[i].SetActive(i == coinMemo.TargetCoinBoxIndex);
			}
		}

		private void UpdateLock()
		{
			// Init Lock
			{
				for (int i = 0; i < stealCoinBoxes.Length; i++)
					stealCoinBoxes[i].IsLock = false;
				for (int i = 0; i < remainCoinBoxes.Length; i++)
					remainCoinBoxes[i].IsLock = false;

				remainCoinBoxes[0].IsLock = true;
				remainCoinBoxes[0].UpdateCoin(100);
			}

			if (!gameManager.IsGaming)
				return;

			int localPlayerTurn = FindLocalPlayerTurn();
			if (localPlayerTurn == NONE_INT)
				return;

			// 코인 가져가기 전에는 수동
			if (gameManager.CurRoundData.CurState <= TGameRoundState.Steal6)
				if ((int)gameManager.CurRoundData.CurState - (int)TGameRoundState.Steal0 <= localPlayerTurn)
					return;

			// 코인 가져간 이후에는 자동 <-- Lock
			{
				int localPlayerCoin = gameManager.CurRoundData.CoinByOrder[localPlayerTurn];
				CoinBox localPlayerButton = stealCoinBoxes[localPlayerTurn];

				localPlayerButton.IsLock = true;
				localPlayerButton.UpdateCoin(localPlayerCoin);

				int beforeStealCoin = gameManager.Bank.MAX_COIN_BY_ROUND[gameManager.Data.CurRound];
				for (int i = 0; i < localPlayerTurn; i++)
					beforeStealCoin -= gameManager.CurRoundData.CoinByOrder[i];
				int afterStealCoin = beforeStealCoin - localPlayerCoin;

				remainCoinBoxes[localPlayerTurn].IsLock = true;
				remainCoinBoxes[localPlayerTurn].UpdateCoin(beforeStealCoin);
				remainCoinBoxes[localPlayerTurn + 1].IsLock = true;
				remainCoinBoxes[localPlayerTurn + 1].UpdateCoin(afterStealCoin);
			}
		}

		private int FindLocalPlayerTurn()
		{
			int localPlayerIndex = NONE_INT;
			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				if (gameManager.IsLocalPlayerNumber(i))
				{
					localPlayerIndex = i;
					break;
				}
			}

			if (localPlayerIndex == NONE_INT)
				return NONE_INT;

			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				if (localPlayerIndex == gameManager.CurRoundData.NumberByOrder[i])
					return i;
			}

			return NONE_INT;
		}

		public void SubScore10()
		{
			coinMemo.MScore.SubValue(coinMemo.MScore.DecreaseAmount * 10);
			coinMemo.UpdateUI();
		}

		public void AddScore10()
		{
			coinMemo.MScore.AddValue(coinMemo.MScore.IncreaseAmount * 10);
			coinMemo.UpdateUI();
		}

		public void IncreaseScore()
		{
			coinMemo.MScore.IncreaseValue();
			coinMemo.UpdateUI();
		}

		public void DecreaseScore()
		{
			coinMemo.MScore.DecreaseValue();
			coinMemo.UpdateUI();
		}
	}
}