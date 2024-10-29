using UdonSharp;
using UnityEngine;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public abstract class MTurnBaseManager : MEventSender
	{
		[UdonSynced, FieldChangeCallback(nameof(CurGameState))] private int _curGameState = 0;
		public int CurGameState
		{
			get => _curGameState;
			private set
			{
				// MDebugLog($"{nameof(CurGameState)} Changed, {CurGameState} to {value}");

				int origin = _curGameState;
				_curGameState = value;
				OnGameStateChange(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}

		public MTurnSeat[] TurnSeats { get; private set; }

		[field: Header("_" + nameof(MTurnBaseManager))]
		[field: SerializeField] public string[] StateToString { get; private set; }

		[field: Header("_" + nameof(MTurnBaseManager) + "_Data")]
		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; protected set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataElement { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites { get; protected set; }
		[field: SerializeField] public Sprite DataNoneSprite { get; protected set; }

		[field: Header("_" + nameof(MTurnBaseManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; protected set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataElement { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites { get; protected set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite { get; protected set; }

		public bool IsInited { get; protected set; } = false;

		public void SetGameState(int newGameState)
		{
			SetOwner();
			CurGameState = (newGameState + StateToString.Length) % StateToString.Length;
			RequestSerialization();
		}

		public bool IsCurGameState(int gameState) => CurGameState == gameState;

		public void NextGameState() => SetGameState(CurGameState + 1);
		public void PrevGameState() => SetGameState(CurGameState - 1);

		protected virtual void OnGameStateChange(DataChangeState changeState)
		{
			// MDebugLog($"{nameof(OnGameStateChange)}, {changeState}");

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

			if (IsInited == false)
				Init();

			foreach (MTurnSeat turnSeat in TurnSeats)
				turnSeat.UpdateStuff();
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
				if (turnSeat.IsTargetPlayer())
					return turnSeat;

			return null;
		}
	}
}
