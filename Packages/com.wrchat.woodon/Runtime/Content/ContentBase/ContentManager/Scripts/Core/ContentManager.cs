
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ContentManager : MEventSender
	{
		public MSeat[] MSeats { get; private set; }

		[field: Header("_" + nameof(ContentManager))]
		[SerializeField] private int stateMax = 1;

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

		protected virtual void OnGameStateChange(DataChangeState changeState)
		{
			// MDebugLog($"{nameof(OnGameStateChange)}, {changeState}");

			UpdateStuff();
			SendEvents();
		}

		public void SetGameState(int newGameState)
		{
			SetOwner();
			CurGameState = (newGameState + stateMax) % stateMax;
			RequestSerialization();
		}

		public bool IsCurGameState(int gameState) => CurGameState == gameState;

		public void NextGameState() => SetGameState(CurGameState + 1);
		public void PrevGameState() => SetGameState(CurGameState - 1);

		public bool IsInited { get; protected set; } = false;

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

			MSeats = GetComponentsInChildren<MSeat>();

			for (int i = 0; i < MSeats.Length; i++)
				MSeats[i].Init(this, i);
		}

		public virtual void UpdateStuff()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			if (IsInited == false)
				Init();

			foreach (MSeat seat in MSeats)
				seat.UpdateStuff();
		}

		public void OnSeatTargetChanged(MSeat changedSeat)
		{
			// 중복 제거 (한 플레이어가 한 번에 하나의 자리에만 등록 가능하도록)
			foreach (MSeat seat in MSeats)
			{
				if (seat == changedSeat)
					continue;

				if (seat.IsTargetPlayer() && seat.TargetPlayerID == changedSeat.TargetPlayerID)
					seat.SetTargetNone();
			}
		}
		
		public MSeat GetLocalPlayerSeat()
		{
			foreach (MSeat seat in MSeats)
				if (seat.IsTargetPlayer())
					return seat;
			return null;
		}
	}
}