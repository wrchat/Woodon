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
		protected DataDictionary SeatData => seatData.DataDictionary;

		public int IntData
		{
			get => SeatData.TryGetValue("IntData", out DataToken dataToken) ? (int)dataToken.Double : default;
			set => SeatData["IntData"] = value;
		}

		public int TurnData
		{
			get => SeatData.TryGetValue("TurnData", out DataToken dataToken) ? (int)dataToken.Double : default;
			set => SeatData["TurnData"] = value;
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
				seatData.RegisterListener(this, nameof(OnDataDeserialization), WJsonEvent.OnDeserialization);

			if (Networking.IsMaster)
			{
				ResetSeat();
				SerializeData();
			}

			UpdateSeat_();
		}

		public virtual void OnDataDeserialization()
		{
			MDebugLog($"{nameof(OnDataDeserialization)}");

			DataDictionary change = seatData.ChangedData;
			DataList keys = change.GetKeys();
			for (int i = 0; i < keys.Count; i++)
			{
				DataToken key = keys[i];

				DataDictionary block = change[key].DataDictionary;
				DataToken origin = block["origin"];
				DataToken cur = block["cur"];

				if (key.String == "IntData")
				{
					OnDataChanged(DataChangeStateUtil.GetChangeState((int)origin.Double, (int)cur.Double));
				}
				else if (key.String == "TurnData")
				{
					OnTurnDataChange(DataChangeStateUtil.GetChangeState((int)origin.Double, (int)cur.Double));
				}
			}
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
		}

		public void ResetTurnData()
		{
			MDebugLog($"{nameof(ResetTurnData)}");
			TurnData = contentManager.DefaultTurnData;
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
