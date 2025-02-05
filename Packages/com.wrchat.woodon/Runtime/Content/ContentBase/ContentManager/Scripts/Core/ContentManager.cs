using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	/// <summary>
	/// 상속 받는 경우, 해당 클래스에도 DefaultExecutionOrder 어트리뷰트를 달아줘야 함.
	/// </summary>
	[DefaultExecutionOrder(-10000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ContentManager : WEventPublisher
	{
		public MSeat[] MSeats { get; private set; }

		[field: Header("_" + nameof(ContentManager))]

		[SerializeField] protected WJson contentData;

		#region ContentState
		public int ContentState
		{
			get => contentData.GetData("ContentState", 0);
			private set => contentData.SetData("ContentState", value);
		}
		[SerializeField] private int contentStateMax = 1;

		protected virtual void OnContentStateChange(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnContentStateChange)}, {changeState}");

			// UpdateContent();
			SendEvents();
		}

		public bool IsContentState(int contentState) => ContentState == contentState;
		public bool IsContentState(Enum contentState) => ContentState == Convert.ToInt32(contentState);

		public void SetContentState(int newContentState)
		{
			ContentState = (newContentState + contentStateMax) % contentStateMax;
			contentData.SerializeData();
		}
		public void SetContentState(Enum newContentState) => SetContentState(Convert.ToInt32(newContentState));
		public void SetContentStateNext() => SetContentState(ContentState + 1);
		public void SetContentStatePrev() => SetContentState(ContentState - 1);
		#endregion

		protected virtual void Start()
		{
			Init();
			UpdateContent();
		}

		protected virtual void Init()
		{
			// MDebugLog($"{nameof(Init)}");

			MSeats = GetComponentsInChildren<MSeat>();
			contentData.RegisterListener(this, nameof(OnContentDataChanged), WJsonEvent.OnDeserialization);

			for (int i = 0; i < MSeats.Length; i++)
				MSeats[i].Init(this, i);

			if (Networking.IsMaster)
			{
				ContentState = 0;
				contentData.SerializeData();
			}
		}

		public virtual void OnContentDataChanged()
		{
			if (contentData.HasDataChanged("CurGameState", out int origin, out int cur))
				OnContentStateChange(DataChangeStateUtil.GetChangeState(origin, cur));

			UpdateContent();
		}

		public virtual void UpdateContent()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			foreach (MSeat seat in MSeats)
				seat.UpdateSeat();
		}

		public virtual void OnSeatTargetChanged(MSeat changedSeat)
		{
			// 중복 제거 (한 플레이어가 한 번에 하나의 자리에만 등록 가능하도록)
			{
				foreach (MSeat seat in MSeats)
				{
					if (seat == changedSeat)
						continue;

					if (seat.IsTargetPlayer() && seat.TargetPlayerID == changedSeat.TargetPlayerID)
						seat.SetTargetNone();
				}
			}
		}

		public MSeat GetLocalPlayerSeat()
		{
			foreach (MSeat seat in MSeats)
				if (seat.IsTargetPlayer())
					return seat;
			return null;
		}

		// ===================================

		[field: SerializeField] public int DefaultData { get; private set; } = 0;
		[field: SerializeField] public string[] DataToString { get; protected set; }
		[field: SerializeField] public bool ResetDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseDataSprites { get; private set; }
		[field: SerializeField] public bool IsDataElement { get; private set; }
		[field: SerializeField] public Sprite[] DataSprites { get; protected set; }
		[field: SerializeField] public Sprite DataNoneSprite { get; protected set; }

		[field: Header("_" + nameof(ContentManager) + "_TurnData")]
		[field: SerializeField] public int DefaultTurnData { get; private set; } = 0;
		[field: SerializeField] public string[] TurnDataToString { get; protected set; }
		[field: SerializeField] public bool ResetTurnDataWhenOwnerChange { get; private set; }
		[field: SerializeField] public bool UseTurnDataSprites { get; private set; }
		[field: SerializeField] public bool IsTurnDataElement { get; private set; }
		[field: SerializeField] public Sprite[] TurnDataSprites { get; protected set; }
		[field: SerializeField] public Sprite TurnDataNoneSprite { get; protected set; }

		public int GetMaxTurnData()
		{
			int maxTurnData = 0;

			foreach (MSeat mTurnSeat in MSeats)
				maxTurnData = Mathf.Max(maxTurnData, mTurnSeat.TurnData);

			return maxTurnData;
		}

		public MSeat[] GetMaxTurnDataSeats()
		{
			int maxTurnData = GetMaxTurnData();
			int maxTurnDataCount = 0;
			MSeat[] maxTurnDataSeats = new MSeat[MSeats.Length];

			foreach (MSeat mTurnSeat in MSeats)
			{
				if (mTurnSeat.TurnData == maxTurnData)
				{
					maxTurnDataSeats[maxTurnDataCount] = mTurnSeat;
					maxTurnDataCount++;
				}
			}

			WUtil.Resize(ref maxTurnDataSeats, maxTurnDataCount);

			return maxTurnDataSeats;
		}
	}
}