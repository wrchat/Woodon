using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
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
		public const string IntDataString = "IntData";
		public const string TurnDataString = "TurnData";
		public const string ContentStateString = "ContentState";

		[Header("_" + nameof(ContentManager))]
		[SerializeField] protected WJson contentData;
		[SerializeField] private int defaultContentState = 0;
		[SerializeField] private SeatDataOption[] seatDataOptions;
		public WSeat[] Seats { get; private set; }

		public int ContentState
		{
			get => contentData.GetData(ContentStateString, 0);
			private set => contentData.SetData(ContentStateString, value);
		}
		[SerializeField] protected int contentStateMax = 1;

		protected virtual void Start()
		{
			Init();
			UpdateContent();
		}

		protected virtual void Init()
		{
			WDebugLog($"{nameof(Init)}");

			Seats = GetComponentsInChildren<WSeat>();
			contentData.RegisterListener(this, nameof(OnContentDataChanged), WJsonEvent.OnDeserialization);

			for (int i = 0; i < Seats.Length; i++)
				Seats[i].Init(this, i);

			if (Networking.IsMaster)
			{
				ContentState = defaultContentState;
				contentData.SerializeData();
			}
		}

		public virtual void UpdateContent()
		{
			// WDebugLog($"{nameof(UpdateStuff)}");

			foreach (WSeat seat in Seats)
				seat.UpdateSeat();
		}

		public virtual void OnContentDataChanged()
		{
			WDebugLog($"{nameof(OnContentDataChanged)}");

			if (contentData.HasDataChanged(ContentStateString, out int originState, out int curState))
				OnContentStateChange(DataChangeStateUtil.GetChangeState(originState, curState));
		}

		public SeatDataOption GetSeatDataOption(string dataName)
		{
			foreach (SeatDataOption option in seatDataOptions)
				if (option.Name == dataName)
					return option;

			WDebugLog($"Not Found DataOption: {dataName}", LogType.Error);
			return null;
		}

		protected virtual void OnContentStateChange(DataChangeState changeState)
		{
			WDebugLog($"{nameof(OnContentStateChange)}, {nameof(changeState)} = {changeState.ToFriendlyString()}");
			UpdateContent();
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

		public virtual void OnSeatTargetChanged(WSeat changedSeat)
		{
			// 중복 제거 (한 플레이어가 한 번에 하나의 자리에만 등록 가능하도록)
			{
				foreach (WSeat seat in Seats)
				{
					if (seat == changedSeat)
						continue;

					if (seat.IsTargetPlayer() && seat.TargetPlayerID == changedSeat.TargetPlayerID)
						seat.SetTargetPlayerNone();
				}
			}
		}

		public WSeat GetLocalPlayerSeat()
		{
			foreach (WSeat seat in Seats)
				if (seat.IsTargetPlayer())
					return seat;
			return null;
		}

		public virtual string GetContentStateString()
		{
			return ContentState.ToString();
		}
	}
}