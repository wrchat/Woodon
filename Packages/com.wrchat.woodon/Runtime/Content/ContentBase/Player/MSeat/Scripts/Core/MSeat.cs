using TMPro;
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
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] protected MData mData;

		public const string INT_DATA = "IntData";
		private int _intData = NONE_INT;
		public int IntData
		{
			get => _intData;
			set
			{
				int origin = _intData;
				_intData = value;
				OnDataChanged(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}

		public int Index { get; private set; }

		protected ContentManager contentManager;

		public virtual void Init(ContentManager contentManager, int index)
		{
			this.contentManager = contentManager;
			Index = index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			// SetData(turnBaseManager.DefaultData);

			if (mData != null)
				mData.RegisterListener(this, nameof(ParseData), MDataEvent.OnDeserialization);

			UpdateStuff();
		}

		public void ParseData()
		{
			MDebugLog($"{nameof(ParseData)}");

			ParseData(mData.DataDictionary);
			UpdateStuff();
		}

		protected virtual void ParseData(DataDictionary dataDict)
		{
			if (mData == null)
				return;

			IntData = dataDict.TryGetValue(INT_DATA, out DataToken dataToken) ? (int)dataToken.Double : NONE_INT;
		}

		public void SerializeData()
		{
			if (mData == null)
				return;

			mData.SetData(INT_DATA, IntData);

			mData.SerializeData();
		}
		
		public virtual void UpdateStuff()
		{
			MDebugLog($"{nameof(UpdateStuff)}");

			OnDataChanged(DataChangeState.None);
		}

		protected override void OnTargetChanged(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnTargetChanged)} : {changeState}");

			base.OnTargetChanged(changeState);

			if (DataChangeStateUtil.IsDataChanged(changeState))
			{
				UpdateStuff();
				if (contentManager != null)
					contentManager.OnSeatTargetChanged(this);
			}
		}

		protected virtual void OnDataChanged(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnDataChanged)} : {IntData} {changeState}");
			
			// UpdateCurDataUI();

			if (DataChangeStateUtil.IsDataChanged(changeState))
			{
				if (contentManager != null)
					contentManager.UpdateStuff();
			}
		}

		public void ResetData()
		{
			if (mData == null)
				return;

			// SetData(turnBaseManager.DefaultData);
			mData.SetData(INT_DATA, 9);
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

		public void ResetSeat()
		{
			ResetPlayer();
			ResetData();
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
