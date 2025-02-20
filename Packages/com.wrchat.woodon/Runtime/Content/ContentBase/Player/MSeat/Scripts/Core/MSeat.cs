using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MSeat : MTarget
	{
		[Header("_" + nameof(MSeat))]
		[SerializeField] protected WJson seatData;

		public int GetData(string name, int defaultValue = NONE_INT) => seatData.GetData(name, defaultValue);

		public int IntData
		{
			get => seatData.GetData(ContentManager.IntDataString,
				contentManager != null ? IntDataOption.DefaultValue : default);
			set => seatData.SetData(ContentManager.IntDataString, value);
		}
		protected SeatDataOption IntDataOption => contentManager.GetSeatDataOption(ContentManager.IntDataString);

		public int TurnData
		{
			get => seatData.GetData(ContentManager.TurnDataString,
				contentManager != null ? TurnDataOption.DefaultValue : default);
			set => seatData.SetData(ContentManager.TurnDataString, value);
		}
		protected SeatDataOption TurnDataOption => contentManager.GetSeatDataOption(ContentManager.TurnDataString);

		protected ContentManager contentManager;
		public int Index { get; private set; }

		[SerializeField] private UIMSeat[] uis;

		public virtual void Init(ContentManager contentManager, int index)
		{
			this.contentManager = contentManager;
			Index = index;

			foreach (UIMSeat ui in uis)
				ui.Init(contentManager, this);

			if (seatData != null)
				seatData.RegisterListener(this, nameof(OnSeatDataChanged), WJsonEvent.OnDeserialization);

			if (Networking.IsMaster)
			{
				ResetSeat();
				SerializeData();
			}

			UpdateSeat_();
		}

		public virtual void OnSeatDataChanged()
		{
			MDebugLog($"{nameof(OnSeatDataChanged)}");

			if (seatData.HasDataChanged(ContentManager.IntDataString, out int originIntData, out int curIntData))
				OnDataChanged(DataChangeStateUtil.GetChangeState(originIntData, curIntData));

			if (seatData.HasDataChanged(ContentManager.TurnDataString, out int originTurnData, out int curTurnData))
				OnTurnDataChange(DataChangeStateUtil.GetChangeState(originTurnData, curTurnData));

			SendEvents();
		}

		public void SerializeData()
		{
			seatData.SerializeData();
		}

		public void UpdateSeat()
		{
			MDebugLog($"{nameof(UpdateSeat)}");

			if (contentManager != null)
				UpdateSeat_();
		}

		protected virtual void UpdateSeat_()
		{
			MDebugLog($"{nameof(UpdateSeat_)}");

			foreach (UIMSeat ui in uis)
				ui.UpdateUI();
		}

		protected override void OnTargetChanged(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnTargetChanged)} : {changeState}");

			base.OnTargetChanged(changeState);

			if (DataChangeStateUtil.IsDataChanged(changeState))
			{
				if (contentManager != null)
				{
					contentManager.OnSeatTargetChanged(this);

					if (TurnDataOption.ResetWhenOwnerChange)
						ResetTurnData();
				}
				UpdateSeat_();
			}
		}

		protected virtual void OnDataChanged(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnDataChanged)} : {IntData} ({changeState})");

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
			MDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");

			// UpdateCurTurnDataUI();

			// 직접 구현하도록

			// if (DataChangeStateUtil.IsDataChanged(changeState))
			// 	contentManager.UpdateContent();
		}

		public virtual void UseSeat()
		{
			foreach (MSeat seat in contentManager.Seats)
			{
				if (seat.IsTargetPlayer(Networking.LocalPlayer))
					seat.ResetSeat();
			}
			SetTargetLocalPlayer();
		}

		public virtual void ResetSeat()
		{
			MDebugLog($"{nameof(ResetSeat)}");
			ResetPlayer();
			ResetData();
			ResetTurnData();
		}

		public virtual void ResetData()
		{
			MDebugLog($"{nameof(ResetData)}");
			IntData = IntDataOption.DefaultValue;
			SerializeData();
		}

		public void ResetTurnData()
		{
			MDebugLog($"{nameof(ResetTurnData)}");
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
	}
}
