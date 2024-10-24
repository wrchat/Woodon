using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTurnSeat : MSeat
	{
		[UdonSynced, FieldChangeCallback(nameof(TurnData))]
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
		[SerializeField] private TextMeshProUGUI[] curTurnDataTexts;
		[SerializeField] private Image[] curTurnDataImages;
		[SerializeField] private TextMeshProUGUI[] turnDataTexts;
		[SerializeField] private Image[] turnDataImages;

		public override void Init(MTurnBaseManager turnBaseManager, int index)
		{
			base.Init(turnBaseManager, index);

			if (Networking.IsMaster)
				SetTurnData(turnBaseManager.DefaultTurnData);
		}

		public void SetTurnData(int newTurnData)
		{
			// MDebugLog($"{nameof(SetTurnData)}({newTurnData})");

			SetOwner();
			TurnData = newTurnData;
			RequestSerialization();
		}

		public void ResetTurnData()
		{
			SetTurnData(turnBaseManager.DefaultTurnData);
		}

		protected virtual void OnTurnDataChange(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");

			UpdateCurTurnDataUI();

			if (changeState != DataChangeState.None)
				turnBaseManager.UpdateStuff();
		}

		private void UpdateCurTurnDataUI()
		{
			if (turnBaseManager == null)
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

		private void UpdateTurnDataUI()
		{
			if (turnBaseManager == null)
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

		protected override void OnTargetChange(DataChangeState changeState)
		{
			if (changeState != DataChangeState.None)
			{
				if (turnBaseManager.ResetTurnDataWhenOwnerChange)
					ResetTurnData();
			}

			base.OnTargetChange(changeState);
		}
	}
}
