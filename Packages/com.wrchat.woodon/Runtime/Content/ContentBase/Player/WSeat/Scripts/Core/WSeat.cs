using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WSeat : WPlayer
	{
		[Header("_" + nameof(WSeat))]
		[SerializeField] protected WJson seatData;
		public WJson SeatData => seatData;

		public int GetData(string name, int defaultValue = NONE_INT) => seatData.GetData(name, defaultValue);

		public int IntData
		{
			get => seatData.GetData(nameof(IntData),
				mainContentManager != null ? IntDataOption.DefaultValue : default);
			set => seatData.SetData(nameof(IntData), value);
		}
		protected SeatDataOption IntDataOption => mainContentManager.GetSeatDataOption(nameof(IntData));

		public int TurnData
		{
			get => seatData.GetData(nameof(TurnData),
				mainContentManager != null ? TurnDataOption.DefaultValue : default);
			set => seatData.SetData(nameof(TurnData), value);
		}
		protected SeatDataOption TurnDataOption => mainContentManager.GetSeatDataOption(nameof(TurnData));

		protected ContentManager mainContentManager = null;
		protected ContentManager[] subContentManagers = new ContentManager[0];

		public int Index { get; private set; }

		[SerializeField] private UIWSeat[] uis;

		public virtual void Init(ContentManager contentManager, int index)
		{
			if (contentManager.ContentMode == ContentMode.Sub)
			{
				Add(ref subContentManagers, contentManager);
				return;
			}

			mainContentManager = contentManager;
			Index = index;

			foreach (UIWSeat ui in uis)
				ui.Init(mainContentManager, this);

			if (seatData != null)
				seatData.RegisterListener(this, nameof(OnSeatDataChanged), WJsonEvent.OnDeserialization);

			if (Networking.IsMaster)
			{
				ResetSeat();
				SerializeData();
			}

			UpdateSeat();
		}

		// 콜백으로 호출 하는 함수라 Public 이여야 합니다.
		public virtual void OnSeatDataChanged()
		{
			WDebugLog($"{nameof(OnSeatDataChanged)}");

			if (seatData.HasDataChanged(nameof(IntData), out int originIntData, out int curIntData))
				OnDataChanged(DataChangeStateUtil.GetChangeState(originIntData, curIntData));

			if (seatData.HasDataChanged(nameof(TurnData), out int originTurnData, out int curTurnData))
				OnTurnDataChange(DataChangeStateUtil.GetChangeState(originTurnData, curTurnData));

			SendEvents();
		}

		public void SerializeData()
		{
			seatData.SerializeData();
		}

		public virtual void UpdateSeat()
		{
			WDebugLog($"{nameof(UpdateSeat)}");

			mainContentManager.OnSeatUpdate();
			foreach (ContentManager subContentManager in subContentManagers)
				subContentManager.OnSeatUpdate();
		
			foreach (UIWSeat ui in uis)
				ui.UpdateUI();
		}

		protected override void OnTargetPlayerChanged(DataChangeState changeState)
		{
			WDebugLog($"{nameof(OnTargetPlayerChanged)} : {changeState}");

			base.OnTargetPlayerChanged(changeState);

			if (DataChangeStateUtil.IsDataChanged(changeState))
			{
				mainContentManager.OnSeatTargetChanged(this);

				if (TurnDataOption.ResetWhenOwnerChange)
					ResetTurnData();
			
				UpdateSeat();
			}
		}

		protected virtual void OnDataChanged(DataChangeState changeState)
		{
			WDebugLog($"{nameof(OnDataChanged)} : {IntData} ({changeState})");

			// UpdateCurDataUI();

			// 직접 구현하도록

			// if (DataChangeStateUtil.IsDataChanged(changeState))
			// {
			// 	if (contentManager != null)
			// 		contentManager.UpdateContent();
			// }
		}

		protected virtual void OnTurnDataChange(DataChangeState changeState)
		{
			WDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");
			UpdateSeat();
			// UpdateCurTurnDataUI();

			// 직접 구현하도록

			// if (DataChangeStateUtil.IsDataChanged(changeState))
			// 	contentManager.UpdateContent();
		}

		public virtual void UseSeat()
		{
			foreach (WSeat seat in mainContentManager.Seats)
			{
				if (seat.IsTargetPlayer(Networking.LocalPlayer))
					seat.ResetSeat();
			}
			SetTargetPlayerLocalPlayer();
		}

		public virtual void ResetSeat()
		{
			WDebugLog($"{nameof(ResetSeat)}");
			ResetTargetPlayer();
			ResetData();
			ResetTurnData();
		}

		public virtual void ResetData()
		{
			WDebugLog($"{nameof(ResetData)}");
			IntData = IntDataOption.DefaultValue;
			SerializeData();
		}

		public void ResetTurnData()
		{
			WDebugLog($"{nameof(ResetTurnData)}");
			TurnData = TurnDataOption.DefaultValue;
			SerializeData();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner() && (player.playerId == TargetPlayerID))
			{
				ResetSeat();
			}
		}

		// 추가, 이미 인스펙터에서 등록된 UI랑 중복될 수 있어서 Contain 확인 - KarmoDDrine 2025.11.15
		public void RegisterUI(UIWSeat uiWSeat)
		{
			if (Contains(uis, uiWSeat))
				return;

			Add(ref uis, uiWSeat);
		}
		
		public void UnregisterUI(UIWSeat uiWSeat)
		{
			if (Contains(uis, uiWSeat) == false)
				return;

			Remove(ref uis, uiWSeat);
		}
	}
}
