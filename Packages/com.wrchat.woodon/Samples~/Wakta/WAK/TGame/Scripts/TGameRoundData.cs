using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	public enum TGameRoundState
	{
		None,
		Steal0,
		Steal1,
		Steal2,
		Steal3,
		Steal4,
		Steal5,
		Steal6,
		Pointing,
		Judge,
		End,
	}

	public enum TGamePointingResult
	{
		// ShootedNumbers의 마지막 데이터에 들어가는 값
		// ShootedNumbers의 요소들을 -1로 초기화하기 때문에 None은 반드시 -1
		// ShootedNumbers는 0 ~ 6 까지의 플레이어 인덱스를 가지고 있기 때문에,
		// 모든 Enum 값을 음수로 설정
		None = -1,
		Skip = -2,
		Fail = -3,
		Success = -4,
		Success_AllKill = -5
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameRoundData : MBase
	{
		/// <summary>
		/// 플레이어 순서
		/// </summary>
		public int[] NumberByOrder { get; private set; }

		/// <summary>
		/// 플레이어가 은행에서 훔친 코인
		/// </summary>
		public int[] CoinByOrder { get; private set; }

		/// <summary>
		/// 총 맞은 플레이어들
		/// </summary>
		public int[] ShootedNumbers { get; private set; }

		/// <summary>
		/// 죽은 플레이어들
		/// </summary>
		public bool[] DiedNumberByOrder { get; private set; }

		/// <summary>
		/// 플레이어들이 이번 라운드에서 최종적으로 가지는 코인
		/// </summary>
		public int[] FinalCoinByOrder { get; private set; }

		/// <summary>
		/// 지목자가 가져가는 코인
		/// </summary>
		public int TotalPointCoin { get; private set; }

		/// <summary>
		/// 지목자의 인덱스
		/// </summary>
		public int PointerNumber { get; private set; }

		/// <summary>
		/// 최소 코인 (최종 기준)
		/// </summary>
		public int MinCoin { get; private set; }

		/// <summary>
		/// 최대 코인 (최종 기준)
		/// </summary>
		public int MaxCoin { get; private set; }

		/// <summary>
		/// 현재 라운드 진행도
		/// </summary>
		public TGameRoundState CurState;

		public bool isRoundEnd;
		public TGamePointingResult pointingResult = TGamePointingResult.None;

		public string DataToString()
		{
			// 0#0#0#0#0#0#0 - 플레이어 순서
			// #
			// 0#0#0#0#0#0#0 - 플레이어가 은행에서 훔친 코인
			// #
			// 0#0#0#0#0#0#0  - 총에 맞은 플레이어 (지목 당한)
			// #
			// 0             - 라운드 종료 여부 (같은 코인 제거 이후)

			string s = string.Empty;

			// 0 ~ 6
			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
				s += DATA_SEPARATOR.ToString() + NumberByOrder[i];

			// 7 ~ 13
			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
				s += DATA_SEPARATOR.ToString() + CoinByOrder[i];

			// 14 ~ 22
			for (int i = 0; i < ShootedNumbers.Length; i++)
				s += DATA_SEPARATOR.ToString() + ShootedNumbers[i];

			s += DATA_SEPARATOR.ToString() + (isRoundEnd ? 1 : 0);

			s = s.Trim(new char[] { DATA_SEPARATOR });

			return s;
		}

		// 이거 누가 한다고 한거야

		public void Init()
		{
			NumberByOrder = new int[TGameManager.PLAYER_COUNT];
			CoinByOrder = new int[TGameManager.PLAYER_COUNT];
			ShootedNumbers = new int[TGameManager.PLAYER_COUNT]; // 6명이 쏘였는지 여부 + 데이터
			DiedNumberByOrder = new bool[TGameManager.PLAYER_COUNT];
			FinalCoinByOrder = new int[TGameManager.PLAYER_COUNT];

			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				NumberByOrder[i] = NONE_INT;
				CoinByOrder[i] = NONE_INT;
				ShootedNumbers[i] = NONE_INT;
				DiedNumberByOrder[i] = false;
				FinalCoinByOrder[i] = NONE_INT;
			}

			PointerNumber = NONE_INT;
			CurState = TGameRoundState.None;
			isRoundEnd = false;
			pointingResult = TGamePointingResult.None;
			TotalPointCoin = NONE_INT;

			MinCoin = int.MaxValue;
			MaxCoin = int.MinValue;
		}

		public void UpdateData(string dataPack)
		{
			// 데이터 필드 초기화
			Init();

			// 데이터 바인딩
			string[] turnDatas = dataPack.Split(DATA_SEPARATOR);

			for (int turn = 0; turn < turnDatas.Length; turn++)
			{
				// 0 ~ 6 PlayerIndex
				if (turn < TGameManager.PLAYER_COUNT * 1)
					NumberByOrder[turn] = int.Parse(turnDatas[turn]);
				// 7 ~ 13 PlayerCoin
				else if (turn < TGameManager.PLAYER_COUNT * 2)
					CoinByOrder[turn % 7] = int.Parse(turnDatas[turn]);
				// 14 ~ 22 StolenPlayerIndex
				else if (turn < TGameManager.PLAYER_COUNT * 3)
					ShootedNumbers[turn - 14] = int.Parse(turnDatas[turn]);
				else if (turn == TGameManager.PLAYER_COUNT * 3)
					isRoundEnd = (int.Parse(turnDatas[turn]) == 1);
			}

			// ---- ---- ---- ----

			// Decoding

			pointingResult = (TGamePointingResult)ShootedNumbers[TGameManager.PLAYER_COUNT - 1];

			// 초기화
			for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
				FinalCoinByOrder[order] = CoinByOrder[order];

			// 총에 쏘인 사람
			int shootedPlayerCount = 0;
			for (int i = 0; i < TGameManager.PLAYER_COUNT - 2; i++)
			{
				if (ShootedNumbers[i] != NONE_INT)
				{
					// PlayerDiedByOrder[ShootedPlayerNumbers[i]] = true;
					shootedPlayerCount++;
				}
			}

			// 가장 적게 양의 코인을 훔친, 지목자 찾기
			// 0225. 중복이 있으면 두 번째로 적게 가져간, 또 중복이 있으면 세 번째로 적게 가져간
			// 0304. 모두 중복이라면 지목 단계 스킵
			int[] sortedCoins = new int[TGameManager.PLAYER_COUNT];
			GetSortCoins(ref sortedCoins, false);
			bool everyoneDup = false;

			for (int i = TGameManager.PLAYER_COUNT - 1; i >= 0; i--)
			{
				int minCoin = sortedCoins[i];
				int minCoinPlayerCount = 0;
				for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
				{
					if (CoinByOrder[order] == minCoin)
					{
						minCoinPlayerCount++;
						PointerNumber = NumberByOrder[order];
						if (minCoinPlayerCount >= 2)
						{
							if (i == 0)
								everyoneDup = true;
							break;
						}
					}
				}

				if (minCoinPlayerCount == 1)
					break;
			}

			// Calc CurRoundState
			if (isRoundEnd)
			{
				CurState = TGameRoundState.End;
			}
			else
			{
				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
				{
					if (CoinByOrder[i] == NONE_INT)
					{
						CurState = (TGameRoundState)((int)TGameRoundState.Steal0 + i);
						break;
					}
				}

				if (CurState == TGameRoundState.None)
					CurState = TGameRoundState.Pointing;

				// 모두 쏘였거나 (지목자의 모든 지목이 정답),
				// 중간에 그만 쏘기로 결정했거나,
				// 쏘다가 실패했거나
				// 0304. 모두 중복이면

				// 무튼 그래서 심판 단계면
				if (CurState == TGameRoundState.Pointing &&
					(pointingResult != TGamePointingResult.None ||
					(CoinByOrder[TGameManager.PLAYER_COUNT - 1] != NONE_INT && everyoneDup)))
				{
					CurState = TGameRoundState.Judge;
				}
			}

			if ((int)CurState >= (int)TGameRoundState.Judge)
			{
				// 전부 쐈거나, 중간에 그만뒀거나
				bool stealSuccess = pointingResult == TGamePointingResult.Success_AllKill ||
									pointingResult == TGamePointingResult.Success;

				// 스틸 코인 계산
				TotalPointCoin = 0;
				for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
				{
					for (int i = 0; i < TGameManager.PLAYER_COUNT - 1; i++)
					{
						if (NumberByOrder[order] == ShootedNumbers[i])
						{
							TotalPointCoin += CoinByOrder[order];
							FinalCoinByOrder[order] = 0;
							break;
						}
					}
				}

				// 지목자에게 코인 전달
				for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
				{
					if (NumberByOrder[order] == PointerNumber)
					{
						if (stealSuccess)
							FinalCoinByOrder[order] += TotalPointCoin;
						else
							FinalCoinByOrder[order] = 0;
						break;
					}
				}

				// 중복 코인 0으로
				for (int order0 = 0; order0 < TGameManager.PLAYER_COUNT - 1; order0++)
				{
					if (NumberByOrder[order0] == PointerNumber)
						continue;

					bool findSame = false;

					for (int order1 = order0 + 1; order1 < TGameManager.PLAYER_COUNT; order1++)
					{
						if (NumberByOrder[order1] == PointerNumber)
							continue;

						if (FinalCoinByOrder[order0] == FinalCoinByOrder[order1])
						{
							findSame = true;
							FinalCoinByOrder[order1] = 0;
							DiedNumberByOrder[order1] = true;
						}
					}

					if (findSame)
					{
						FinalCoinByOrder[order0] = 0;
						DiedNumberByOrder[order0] = true;
					}
				}

				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
				{
					int curCoin = FinalCoinByOrder[i];

					if (curCoin < MinCoin)
						MinCoin = curCoin;

					if (curCoin > MaxCoin)
						MaxCoin = curCoin;
				}

				string debug = string.Empty;
				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
					debug += FinalCoinByOrder[i] + "_";
			}
		}

		public bool IsPlayerCoinOrder(int playerNumber, int order)
		{
			int playerCoin = NONE_INT;
			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				if (NumberByOrder[i] == playerNumber)
				{
					playerCoin = CoinByOrder[i];
					break;
				}
			}

			int[] sortedCoins = new int[TGameManager.PLAYER_COUNT];
			GetSortCoins(ref sortedCoins, false);

			string debugString = string.Empty;
			for (int i = 0; i < sortedCoins.Length; i++)
				debugString += sortedCoins[i];

			// 정렬 코인[order] == 플레이어가 훔친 코인
			return sortedCoins[order] == playerCoin;
		}

		public void GetSortCoins(ref int[] arr, bool calcArr)
		{
			if (arr == null)
				arr = new int[TGameManager.PLAYER_COUNT];

			Array.Copy(calcArr ? FinalCoinByOrder : CoinByOrder, arr, TGameManager.PLAYER_COUNT);
			for (int i = 0; i < arr.Length - 1; i++)
			{
				for (int j = 0; j < arr.Length - 1; j++)
				{
					if (arr[j] < arr[j + 1])
					{
						int temp = arr[j + 1];
						arr[j + 1] = arr[j];
						arr[j] = temp;
					}
				}
			}
		}

		public bool IsPlayerShooted(int targetNumber)
		{
			// 마지막 데이터는 PointingResult라 제외
			for (int i = 0; i < ShootedNumbers.Length - 1; i++)
				if (ShootedNumbers[i] == targetNumber)
					return true;

			return false;
		}
	}
}