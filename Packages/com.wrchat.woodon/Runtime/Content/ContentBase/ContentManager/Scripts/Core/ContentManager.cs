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
		public MSeat[] Seats { get; private set; }

		[field: Header("_" + nameof(ContentManager))]

		[SerializeField] protected WJson contentData;
		[field: SerializeField] public ContentDataOption[] ContentDataOptions { get; private set; }
		public const string IntDataString = "IntData";
		public const string TurnDataString = "TurnData";

		public ContentDataOption GetDataOption(string name)
		{
			foreach (ContentDataOption option in ContentDataOptions)
				if (option.Name == name)
					return option;
			
			MDebugLog($"Not Found DataOption: {name}", LogType.Error);
			return null;
		}

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
		#endregion

		protected virtual void Start()
		{
			Init();
			UpdateContent();
		}

		protected virtual void Init()
		{
			// MDebugLog($"{nameof(Init)}");

			Seats = GetComponentsInChildren<MSeat>();
			contentData.RegisterListener(this, nameof(OnContentDataChanged), WJsonEvent.OnDeserialization);

			for (int i = 0; i < Seats.Length; i++)
				Seats[i].Init(this, i);

			if (Networking.IsMaster)
			{
				ContentState = 0;
				contentData.SerializeData();
			}
		}

		public virtual void OnContentDataChanged()
		{
			MDebugLog($"{nameof(OnContentDataChanged)}");

			if (contentData.HasDataChanged("ContentState", out int origin, out int cur))
				OnContentStateChange(DataChangeStateUtil.GetChangeState(origin, cur));
		}

		public virtual void UpdateContent()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			foreach (MSeat seat in Seats)
				seat.UpdateSeat();
		}

		public virtual void OnSeatTargetChanged(MSeat changedSeat)
		{
			// 중복 제거 (한 플레이어가 한 번에 하나의 자리에만 등록 가능하도록)
			{
				foreach (MSeat seat in Seats)
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
			foreach (MSeat seat in Seats)
				if (seat.IsTargetPlayer())
					return seat;
			return null;
		}
	}
}