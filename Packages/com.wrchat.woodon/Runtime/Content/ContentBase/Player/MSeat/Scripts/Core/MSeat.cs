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

		public int IntData
		{
			get => seatData.GetData("IntData",
				contentManager != null ? contentManager.DefaultData : default);
			set => seatData.SetData("CardList", value);
		}

		public int TurnData
		{
			get => seatData.GetData("TurnData",
				contentManager != null ? contentManager.DefaultTurnData : default);
			set => seatData.SetData("TurnData", value);
		}

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

			if (seatData.HasDataChanged("IntData", out int originIntData, out int curIntData))
				OnDataChanged(DataChangeStateUtil.GetChangeState(seatData.GetData("IntData", 0), IntData));

			if (seatData.HasDataChanged("TurnData", out int originTurnData, out int curTurnData))
				OnTurnDataChange(DataChangeStateUtil.GetChangeState(originTurnData, curTurnData));
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

					if (contentManager.ResetTurnDataWhenOwnerChange)
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
			foreach (MSeat seat in contentManager.MSeats)
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
			IntData = contentManager.DefaultData;
			SerializeData();
		}

		public void ResetTurnData()
		{
			MDebugLog($"{nameof(ResetTurnData)}");
			TurnData = contentManager.DefaultTurnData;
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
