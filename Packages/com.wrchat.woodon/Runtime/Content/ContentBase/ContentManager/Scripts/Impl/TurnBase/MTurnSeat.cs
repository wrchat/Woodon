using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public abstract class MTurnSeat : MSeat
	{
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

		[Header("_" + nameof(MTurnSeat))]
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

		[SerializeField] private TextMeshProUGUI[] curTurnDataTexts;
		[SerializeField] private Image[] curTurnDataImages;
		[SerializeField] private TextMeshProUGUI[] turnDataTexts;
		[SerializeField] private Image[] turnDataImages;

		public MTurnBaseManager turnBaseManager { get; private set; }

		public override void Init(ContentManager contentManager, int index)
		{
			turnBaseManager = (MTurnBaseManager)contentManager;
			base.Init(contentManager, index);

			if (Networking.IsMaster)
				ResetData();
		}

		public void SetTurnData(int newTurnData)
		{
			// MDebugLog($"{nameof(SetTurnData)}({newTurnData})");

			mData.SetData(TURN_DATA, newTurnData);
			// TurnData = newTurnData;
		}

		public void ResetTurnData()
		{
			MDebugLog($"{nameof(ResetTurnData)}");

			SetTurnData(turnBaseManager.DefaultTurnData);
		}

		protected override void OnDataChanged(DataChangeState changeState)
		{
			if (contentManager == null)
			{
				MDebugLog($"{nameof(OnDataChanged)} : {nameof(contentManager)} is null", LogType.Warning);
				return;
			}

			// UpdateCurDataUI();

			if (DataChangeStateUtil.IsDataChanged(changeState))
				contentManager.UpdateStuff();
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

			if (turnBaseManager.IsDataElement)
			{
				string curDataString = (IntData == NONE_INT) ? string.Empty :
										(turnBaseManager.DataToString.Length > IntData) ? turnBaseManager.DataToString[IntData] : IntData.ToString();
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = curDataString;

				Sprite[] dataSprites = turnBaseManager.DataSprites;
				Sprite noneSprite = turnBaseManager.DataNoneSprite;
				foreach (Image curDataImage in curDataImages)
				{
					if (turnBaseManager.UseDataSprites)
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

			if (turnBaseManager.IsTurnDataElement)
			{
				string curTurnDataString = (TurnData == NONE_INT) ? string.Empty :
										(turnBaseManager.TurnDataToString.Length > TurnData) ? turnBaseManager.TurnDataToString[TurnData] : TurnData.ToString();
				foreach (TextMeshProUGUI turnDataText in curTurnDataTexts)
					turnDataText.text = curTurnDataString;

				Sprite[] turnDataSprites = turnBaseManager.TurnDataSprites;
				Sprite noneSprite = turnBaseManager.TurnDataNoneSprite;
				foreach (Image curTurnDataImage in curTurnDataImages)
				{
					if (turnBaseManager.UseTurnDataSprites)
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

			if (turnBaseManager.IsDataElement)
			{
				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (i >= turnBaseManager.DataToString.Length)
					{
						dataTexts[i].text = i.ToString();
					}
					else
					{
						dataTexts[i].text = turnBaseManager.DataToString[i];
					}
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (i >= turnBaseManager.DataToString.Length)
					{
						dataImages[i].sprite = turnBaseManager.DataNoneSprite;
					}
					else
					{
						dataImages[i].sprite = turnBaseManager.DataSprites[i];
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

			if (turnBaseManager.IsTurnDataElement)
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
				{
					if (i >= turnBaseManager.TurnDataToString.Length)
					{
						turnDataTexts[i].text = i.ToString();
					}
					else
					{
						turnDataTexts[i].text = turnBaseManager.TurnDataToString[i];
					}
				}

				for (int i = 0; i < turnDataImages.Length; i++)
				{
					if (i >= turnBaseManager.TurnDataToString.Length)
					{
						turnDataImages[i].sprite = turnBaseManager.TurnDataNoneSprite;
					}
					else
					{
						turnDataImages[i].sprite = turnBaseManager.TurnDataSprites[i];
					}
				}
			}
			else
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
					turnDataTexts[i].text = i.ToString();
			}
		}

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			UpdateDataUI();
			UpdateCurDataUI();

			UpdateTurnDataUI();
			UpdateCurTurnDataUI();

			// TurnBaseManager.UpdateStuff에서 각 Seat.UpdateStuff를 호출
			// OnTurnDataChange에서는 역으로 TurnBaseManager.UpdateStuff를 호출
			// 때문에 무한 루프를 방지하기 위해,
			// TurnData가 변경되어 Setter에서 OnTurnDataChange가 호출된 것인지,
			// TurnBaseManager.UpdateStuff가 호출되어 OnTurnDataChange가 호출된 것인지 구분시켜줄 필요가 있음.
			// OnTurnDataChange(DataChangeState.None);

			// 240801 → OnTurnDataChange에서 UI 갱신 코드를 분리
		}

		protected override void OnTargetChanged(DataChangeState changeState)
		{
			if (DataChangeStateUtil.IsDataChanged(changeState))
			{
				if (contentManager == null)
					return;

				if (turnBaseManager.ResetTurnDataWhenOwnerChange)
					ResetTurnData();
			}

			base.OnTargetChanged(changeState);
		}

		protected override void ParseData(DataDictionary dataDict)
		{
			base.ParseData(dataDict);
			TurnData = dataDict.TryGetValue(TURN_DATA, out DataToken turnDataToken) ? (int)turnDataToken.Double : NONE_INT;
		}
	}
}
