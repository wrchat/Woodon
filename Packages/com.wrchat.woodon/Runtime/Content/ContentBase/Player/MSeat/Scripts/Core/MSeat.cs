using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
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

			if (mData != null)
				mData.RegisterListener(this, nameof(OnDataDeserialization), MDataEvent.OnDeserialization);

			if (Networking.IsMaster)
			{
				ResetSeat();
				SerializeData();
			}

			UpdateStuff_();
		}

		public virtual void OnDataDeserialization()
		{
			MDebugLog($"{nameof(ParseData)}");

			ParseData(mData.DataDictionary);
			UpdateStuff_();
		}

		protected virtual void ParseData(DataDictionary dataDict)
		{
			if (mData == null)
				return;

			IntData = dataDict.TryGetValue(INT_DATA, out DataToken dataToken) ? (int)dataToken.Double : NONE_INT;
			TurnData = dataDict.TryGetValue(TURN_DATA, out DataToken turnDataToken) ? (int)turnDataToken.Double : NONE_INT;
		}

		public virtual void SerializeData()
		{
			if (mData == null)
				return;

			mData.SetData(INT_DATA, IntData);
			mData.SetData(TURN_DATA, TurnData);
			mData.SerializeData();
		}

		public void UpdateSeat()
		{
			MDebugLog($"{nameof(UpdateSeat)}");

			OnDataChanged(DataChangeState.None);

			UpdateDataUI();
			UpdateCurDataUI();

			UpdateTurnDataUI();
			UpdateCurTurnDataUI();

			if (contentManager != null)
				UpdateStuff_();

			// TurnBaseManager.UpdateStuff에서 각 Seat.UpdateStuff를 호출
			// OnTurnDataChange에서는 역으로 TurnBaseManager.UpdateStuff를 호출
			// 때문에 무한 루프를 방지하기 위해,
			// TurnData가 변경되어 Setter에서 OnTurnDataChange가 호출된 것인지,
			// TurnBaseManager.UpdateStuff가 호출되어 OnTurnDataChange가 호출된 것인지 구분시켜줄 필요가 있음.
			// OnTurnDataChange(DataChangeState.None);

			// 240801 → OnTurnDataChange에서 UI 갱신 코드를 분리
		}

		protected virtual void UpdateStuff_()
		{
			MDebugLog($"{nameof(UpdateStuff_)}");

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
				UpdateStuff_();
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

		public virtual void ResetData()
		{
			if (mData == null)
				return;

			mData.SetData(INT_DATA, contentManager.DefaultData);
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
			ResetPlayer();
			
			ResetData();
			ResetTurnData();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner() && (player.playerId == TargetPlayerID))
			{
				ResetSeat();
			}
		}

		// ==================================

		public const string TURN_DATA = "TurnData";
		private int _turnData = NONE_INT;
		public int TurnData
		{
			get => _turnData;
			set
			{
				int origin = _turnData;
				_turnData = value;
				OnTurnDataChange(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}

		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

		[SerializeField] private TextMeshProUGUI[] curTurnDataTexts;
		[SerializeField] private Image[] curTurnDataImages;
		[SerializeField] private TextMeshProUGUI[] turnDataTexts;
		[SerializeField] private Image[] turnDataImages;

		public void ResetTurnData()
		{
			MDebugLog($"{nameof(ResetTurnData)}");
			TurnData = contentManager.DefaultTurnData;
		}

		protected virtual void OnTurnDataChange(DataChangeState changeState)
		{	
			MDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");

			// UpdateCurTurnDataUI();

			if (DataChangeStateUtil.IsDataChanged(changeState))
				contentManager.UpdateStuff();
		}

		private void UpdateCurDataUI()
		{
			if (contentManager == null)
				return;

			if (contentManager.IsDataElement)
			{
				string curDataString = (IntData == NONE_INT) ? string.Empty :
										(contentManager.DataToString.Length > IntData) ? contentManager.DataToString[IntData] : IntData.ToString();
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = curDataString;

				Sprite[] dataSprites = contentManager.DataSprites;
				Sprite noneSprite = contentManager.DataNoneSprite;
				foreach (Image curDataImage in curDataImages)
				{
					if (contentManager.UseDataSprites)
					{
						curDataImage.sprite = (IntData != NONE_INT) ? dataSprites[IntData] : noneSprite;
					}
					else
					{
						curDataImage.sprite = (IntData != NONE_INT) ? null : noneSprite;
					}
				}
			}
			else
			{
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = IntData.ToString();
			}
		}

		private void UpdateCurTurnDataUI()
		{
			if (contentManager == null)
				return;

			if (contentManager.IsTurnDataElement)
			{
				string curTurnDataString = (TurnData == NONE_INT) ? string.Empty :
										(contentManager.TurnDataToString.Length > TurnData) ? contentManager.TurnDataToString[TurnData] : TurnData.ToString();
				foreach (TextMeshProUGUI turnDataText in curTurnDataTexts)
					turnDataText.text = curTurnDataString;

				Sprite[] turnDataSprites = contentManager.TurnDataSprites;
				Sprite noneSprite = contentManager.TurnDataNoneSprite;
				foreach (Image curTurnDataImage in curTurnDataImages)
				{
					if (contentManager.UseTurnDataSprites)
					{
						curTurnDataImage.sprite = (TurnData != NONE_INT) ? turnDataSprites[TurnData] : noneSprite;
					}
					else
					{
						curTurnDataImage.sprite = (TurnData != NONE_INT) ? null : noneSprite;
					}
				}
			}
			else
			{
				foreach (TextMeshProUGUI curTurnDataText in curTurnDataTexts)
					curTurnDataText.text = TurnData.ToString();
			}
		}

		private void UpdateDataUI()
		{
			if (contentManager == null)
				return;

			if (contentManager.IsDataElement)
			{
				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (i >= contentManager.DataToString.Length)
					{
						dataTexts[i].text = i.ToString();
					}
					else
					{
						dataTexts[i].text = contentManager.DataToString[i];
					}
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (i >= contentManager.DataToString.Length)
					{
						dataImages[i].sprite = contentManager.DataNoneSprite;
					}
					else
					{
						dataImages[i].sprite = contentManager.DataSprites[i];
					}
				}
			}
			else
			{
				for (int i = 0; i < dataTexts.Length; i++)
					dataTexts[i].text = i.ToString();
			}
		}

		private void UpdateTurnDataUI()
		{
			if (contentManager == null)
				return;

			if (contentManager.IsTurnDataElement)
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
				{
					if (i >= contentManager.TurnDataToString.Length)
					{
						turnDataTexts[i].text = i.ToString();
					}
					else
					{
						turnDataTexts[i].text = contentManager.TurnDataToString[i];
					}
				}

				for (int i = 0; i < turnDataImages.Length; i++)
				{
					if (i >= contentManager.TurnDataToString.Length)
					{
						turnDataImages[i].sprite = contentManager.TurnDataNoneSprite;
					}
					else
					{
						turnDataImages[i].sprite = contentManager.TurnDataSprites[i];
					}
				}
			}
			else
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
					turnDataTexts[i].text = i.ToString();
			}
		}
	}
}
