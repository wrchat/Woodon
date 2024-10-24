using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	public enum TGameRoundUIState
	{
		StartedYet,
		Coin,
		RoundEndOrStaffCanvas,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameUIRoundPanel : MBase
	{
		[Header("_" + nameof(TGameUIRoundPanel))]
		private TGameUIPlayerBlock[] playerBlocks;
		[SerializeField] private TextMeshProUGUI roundText;
		[SerializeField] private bool isStaffCanvas = false;
		[SerializeField] private Image fadeImage;

		private TGameRoundUIState state;

		public void Init()
		{
			playerBlocks = GetComponentsInChildren<TGameUIPlayerBlock>(true);
			foreach (TGameUIPlayerBlock playerBlock in playerBlocks)
				playerBlock.Init();
		}

		public void UpdateUI(TGameManager gameManager, int thisRound)
		{
			foreach (TGameUIPlayerBlock playerBlock in playerBlocks)
				playerBlock.Init();

			TGameRoundData roundData = gameManager.Data.RoundDatas[thisRound];
			int curRound = gameManager.Data.CurRound;

			bool isThisRound = curRound == thisRound;
			roundText.color = isThisRound
				? new Color(221f / 255f, 177f / 255f, 71f / 255f)
				: Color.black;

			if (curRound < thisRound)
				state = TGameRoundUIState.StartedYet;
			else if (roundData.isRoundEnd || isStaffCanvas)
				state = TGameRoundUIState.RoundEndOrStaffCanvas;
			else
				state = TGameRoundUIState.Coin;

			switch (state)
			{
				// None
				case TGameRoundUIState.StartedYet:
					break;
				// None ~ TakenCoin
				case TGameRoundUIState.Coin:
					UpdatePlayer(gameManager, roundData);
					Coin(gameManager, roundData);
					break;
				// Shooted ~ GreatThief
				case TGameRoundUIState.RoundEndOrStaffCanvas:
					UpdatePlayer(gameManager, roundData);
					RoundEndOrStaffCanvas(roundData);
					break;
			}

			fadeImage.gameObject.SetActive(state == TGameRoundUIState.StartedYet);
		}

		private void UpdatePlayer(TGameManager gameManager, TGameRoundData roundData)
		{
			// 플레이어 정보
			for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
			{
				// 현재 PlayerNum
				int playerNumber = roundData.NumberByOrder[order];

				// 플레이어 정보
				string playerName = gameManager.GetPlayerName(playerNumber, false);
				Sprite playerSprite = gameManager.GetPlayerImage(playerNumber);
				playerBlocks[order].SetPlayer(playerName, playerSprite);
			}
		}

		public void Coin(TGameManager gameManager, TGameRoundData roundData)
		{
			for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
			{
				TGamePlayerUIState initState = roundData.CoinByOrder[order] == NONE_INT
					? TGamePlayerUIState.None
					: TGamePlayerUIState.TakedCoin;
				playerBlocks[order].SetState(initState);

				VRCPlayerApi targetPlayer = gameManager.GetPlayerAPI(roundData.NumberByOrder[order]);
				if (targetPlayer != null)
				{
					string tag0 = Networking.LocalPlayer.GetPlayerTag($"{targetPlayer.playerId}{VoiceAreaTag.AREA_A}");
					string tag1 = Networking.LocalPlayer.GetPlayerTag($"{targetPlayer.playerId}{VoiceAreaTag.AREA_B}");

					if (tag1 == TRUE_STRING)
						playerBlocks[order].SetState(TGamePlayerUIState.Safe);
					else if (tag0 == TRUE_STRING)
						playerBlocks[order].SetState(TGamePlayerUIState.Waiting);
				}
			}
		}

		private void RoundEndOrStaffCanvas(TGameRoundData roundData)
		{
			// 훔치는 순서대로 한 명씩
			for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
			{
				// 현재 PlayerNum
				int playerNumber = roundData.NumberByOrder[order];

				int playerCoin = roundData.CoinByOrder[order];
				int calcCoin = roundData.FinalCoinByOrder[order];

				string playerCoinString = calcCoin == NONE_INT ? "-" : playerCoin.ToString();
				playerBlocks[order].SetCoin(playerCoinString);

				// Debug.Log($"PlayerNum: {playerNumber}, Coin: {playerCoin}, CalcCoin: {calcCoin}, MinCoin: {roundData.MinCoin}, MaxCoin: {roundData.MaxCoin}, IsPlayerShooted: {roundData.IsPlayerShooted(playerNumber)}, DiedNumberByOrder: {roundData.DiedNumberByOrder[order]}");
				// 가장 적게 가져갔다
				// 총에 맞지 않아도 가장 적게 가져갈 수 있는 경우가 있기에, 해골 문양 띄우는 건 따로 계산
				if (calcCoin == roundData.MinCoin)
				{
					playerBlocks[order].SetState(TGamePlayerUIState.MinCoin);
				}
				// 가장 많이 가져갔다
				else if (calcCoin == roundData.MaxCoin)
				{
					playerBlocks[order].SetState(TGamePlayerUIState.GreatThief);
				}

				// 해골 문양 띄울 지
				// if (roundData.DiedNumberByOrder[order])
				if (roundData.IsPlayerShooted(playerNumber) || roundData.DiedNumberByOrder[order])
				{
					playerBlocks[order].SetState(TGamePlayerUIState.Shooted);
				}

				// 지목자
				if (roundData.isRoundEnd && (roundData.PointerNumber == playerNumber))
				{
					if (roundData.pointingResult == TGamePointingResult.Fail)
					{
						playerBlocks[order].SetState(TGamePlayerUIState.Shooted);
					}
					else
					{
						if (roundData.TotalPointCoin == 0)
							playerBlocks[order].SetCoin(playerCoinString);
						else
							playerBlocks[order].SetCoin(playerCoinString + "+" + roundData.TotalPointCoin);
					}
				}
			}
		}
	}
}