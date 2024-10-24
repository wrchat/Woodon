using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TGameData : MBase
	{
		[Header("_" + nameof(TGameData))]
		[SerializeField] private TGameManager gameManager;
		[field: SerializeField] public TGameRoundData[] RoundDatas { get; private set; }
		[SerializeField] private TextMeshProUGUI debugText;

		private int _curRound = NONE_INT;
		public int CurRound
		{
			get => _curRound;
			set
			{
				_curRound = value;
				gameManager.OnRoundChange();
			}
		}

		public bool IsLastRound => CurRound == RoundDatas.Length - 1;

		[UdonSynced, FieldChangeCallback(nameof(GameData))]
		private string _gameData = string.Empty;
		public string GameData
		{
			get => _gameData;
			set
			{
				MDebugLog($"{nameof(GameData)} Changed : {value}");
				_gameData = value;

				debugText.text = string.Empty;
				if (_gameData == string.Empty || !_gameData.Contains(DATA_PACK_SEPARATOR.ToString()))
					return;

				// Binding
				string[] dataPack = _gameData.Split('|');
				string[] roundDataPacks = dataPack[0].Split(DATA_PACK_SEPARATOR);
				for (int i = 0; i < RoundDatas.Length; i++)
				{
					debugText.text += roundDataPacks[i] + "\n";
					RoundDatas[i].UpdateData(roundDataPacks[i]);
				}
				CurRound = int.Parse(dataPack[1]);

				gameManager.OnGameDataChange();
			}
		}

		private string RoundDatasToString()
		{
			string s = string.Empty;

			for (int i = 0; i < RoundDatas.Length; i++)
				s += DATA_PACK_SEPARATOR.ToString() + RoundDatas[i].DataToString();

			s = s.Trim(new char[] { DATA_PACK_SEPARATOR });
			s += $"|{CurRound}";
			return s;
		}

		public void SyncGameData()
		{
			MDebugLog($"{nameof(SyncGameData)}");
			SetOwner();

			// CurRound = NONE_INT;
			GameData = RoundDatasToString();

			RequestSerialization();
		}

		public void Init()
		{
			MDebugLog($"{nameof(Init)}");
			SetOwner();

			CurRound = NONE_INT;
			foreach (TGameRoundData roundData in RoundDatas)
				roundData.Init();
			GameData = RoundDatasToString();

			RequestSerialization();
		}

		public void NextRound()
		{
			MDebugLog($"{nameof(NextRound)}");

			// 마지막 라운드
			if (CurRound == 5) return;

			if (CurRound == NONE_INT)
				InitOrder();

			// 프로퍼티를 통해 변경하면 GameData가 갱신되기 전에 OnRoundChange가 호출되기 때문에 필드를 변경하고
			// 이후 GameData를 갱신시켜, GameData의 Setter에서 CurRound 프로퍼티를 통해 변경하도록 함
			_curRound++;
			SyncGameData();
		}

		public void InitOrder()
		{
			string prevOrderString = "#######";

			for (int ri = 0; ri < RoundDatas.Length; ri++)
			{
				string orderString = string.Empty;

				// 7 플레이어의 순서를 모두 정할때 까지 반복
				while (orderString.Length < TGameManager.PLAYER_COUNT)
				{
					// 0 ~ 6 랜덤 플레이어 Index
					int randomIndex = Random.Range(0, 7);

					// 이미 뽑힌 플레이어라면 다시 뽑기
					if (orderString.Contains(randomIndex.ToString()))
						continue;

					// 이전 라운드와 순서가 같다면 다시 뽑기
					// 현재 뽑고자 하는 순서 -> orderString.Length
					// i.e. 첫 번째 순서, 아직 아무도 뽑지 않은 상태니까 orderString == string.Empty, orderString.Length == 0
					if (prevOrderString[orderString.Length].ToString() == randomIndex.ToString())
					{
						// 마지막 순서에서 겹친 상태라면, 다시 뽑아도 똑같은 플레이어만 나오기 때문에, 처음부터 다시 뽑음
						if (orderString.Length == TGameManager.PLAYER_COUNT - 1)
							orderString = string.Empty;
						continue;
					}

					orderString += randomIndex;
				}

				// if ((order.Length > PLAYER_COUNT)) // 순서가 UI를 통해 모두 설정되어있지 않다면
				// 	return;

				for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
					RoundDatas[ri].NumberByOrder[i] = int.Parse(orderString[i].ToString());

				prevOrderString = orderString;
			}
		}
	}
}