using UdonSharp;
using UnityEngine;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTurnSeatManager : MEventSender
	{
		[UdonSynced(), FieldChangeCallback(nameof(CurGameState))]
		private int _curGameState = 0;
		public int CurGameState
		{
			get => _curGameState;
			set
			{
				// MDebugLog($"{nameof(CurGameState)} Changed, {CurGameState} to {value}");

				int origin = _curGameState;
				_curGameState = value;
				OnGameStateChange(origin, value);
			}
		}

		public MTurnSeat[] TurnSeats { get; private set; }

		[field: Header("_" + nameof(MTurnSeatManager))]
		[field: SerializeField] public string[] StateToString { get; private set; }

		[field: Header("_" + nameof(MTurnSeatManager) + "_Data")]
		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; private set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataState { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites { get; private set; }
		[field: SerializeField] public Sprite DataNoneSprite { get; private set; }

		[field: Header("_" + nameof(MTurnSeatManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; private set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataState { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites { get; private set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite { get; private set; }

		public bool IsInited { get; protected set; } = false;

		public void SetGameState(int newGameState)
		{
			SetOwner();
			CurGameState = (newGameState + StateToString.Length) % StateToString.Length;
			RequestSerialization();
		}

		public void NextGameState() => SetGameState(CurGameState + 1);
		public void PrevGameState() => SetGameState(CurGameState - 1);

		protected virtual void OnGameStateChange(int origin, int value)
		{
			// MDebugLog($"{nameof(OnGameStateChange)}, {origin} to {value}");

			UpdateStuff();
			SendEvents();
		}

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			// MDebugLog($"{nameof(Init)}");

			if (IsInited)
				return;
			IsInited = true;

			TurnSeats = GetComponentsInChildren<MTurnSeat>();

			for (int i = 0; i < TurnSeats.Length; i++)
				TurnSeats[i].Init(this, i);
		}

		public virtual void UpdateStuff()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");
		}

		public int GetMaxData()
		{
			int maxData = 0;

			foreach (MTurnSeat seat in TurnSeats)
				maxData = Mathf.Max(maxData, seat.Data);

			return maxData;
		}

		public MTurnSeat[] GetMaxDataSeats()
		{
			int maxData = GetMaxData();
			int maxDataCount = 0;
			MTurnSeat[] maxDataSeats = new MTurnSeat[TurnSeats.Length];

			foreach (MTurnSeat seat in TurnSeats)
			{
				if (seat.Data == maxData)
				{
					maxDataSeats[maxDataCount] = seat;
					maxDataCount++;
				}
			}

			MDataUtil.ResizeArr(ref maxDataSeats, maxDataCount);

			return maxDataSeats;
		}

		public int GetMaxTurnData()
		{
			int maxTurnData = 0;

			foreach (MTurnSeat seat in TurnSeats)
				maxTurnData = Mathf.Max(maxTurnData, seat.TurnData);

			return maxTurnData;
		}

		public MTurnSeat[] GetMaxTurnDataSeats()
		{
			int maxTurnData = GetMaxTurnData();
			int maxTurnDataCount = 0;
			MTurnSeat[] maxTurnDataSeats = new MTurnSeat[TurnSeats.Length];

			foreach (MTurnSeat seat in TurnSeats)
			{
				if (seat.TurnData == maxTurnData)
				{
					maxTurnDataSeats[maxTurnDataCount] = seat;
					maxTurnDataCount++;
				}
			}

			MDataUtil.ResizeArr(ref maxTurnDataSeats, maxTurnDataCount);

			return maxTurnDataSeats;
		}

		public MTurnSeat GetLocalPlayerSeat()
		{
			foreach (MTurnSeat turnSeat in TurnSeats)
				if (IsLocalPlayerID(turnSeat.OwnerID))
					return turnSeat;

			return null;
		}
	}
}
