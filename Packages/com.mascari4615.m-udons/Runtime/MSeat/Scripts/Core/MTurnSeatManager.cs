using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTurnSeatManager : MEventSender
	{
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurGameState))]
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

		[Header("_" + nameof(MTurnSeatManager))]
		[SerializeField] private UISeatManagerController[] uis;
		[field: SerializeField] public string[] StateToString { get; private set; }

		[field: Header("_" + nameof(MTurnSeatManager) + "_Data")]
		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; private set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataState { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites{ get; private set; }
		[field: SerializeField] public Sprite DataNoneSprite{ get; private set; }

		[field: Header("_" + nameof(MTurnSeatManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; private set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataState { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites{ get; private set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite{ get; private set; }

		public void SetGameState(int newGameState)
		{
			SetOwner();
			CurGameState = newGameState % StateToString.Length;
			RequestSerialization();
		}

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
			TurnSeats = GetComponentsInChildren<MTurnSeat>();

			foreach (UISeatManagerController ui in uis)
				ui.Init(this);

			for (int i = 0; i < TurnSeats.Length; i++)
				TurnSeats[i].Init(this, i);
		}

		public virtual void UpdateStuff()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			foreach (UISeatManagerController ui in uis)
				ui.UpdateUI();
		}

		public int GetMaxData()
		{
			int maxData = 0;

			foreach (MTurnSeat seat in TurnSeats)
				maxData = Mathf.Max(maxData, seat.Data);

			return maxData;
		}

		public int GetMaxTurnData()
		{
			int maxTurnData = 0;

			foreach (MTurnSeat seat in TurnSeats)
				maxTurnData = Mathf.Max(maxTurnData, seat.TurnData);

			return maxTurnData;
		}

		public MTurnSeat FindLocalPlayerSeat()
		{
			foreach (MTurnSeat turnSeat in TurnSeats)
				if (IsLocalPlayerID(turnSeat.OwnerID))
					return turnSeat;

			return null;
		}
	}
}