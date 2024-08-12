using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
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

		public override void Init(MTurnSeatManager seatManager, int index)
		{
			base.Init(seatManager, index);

			if (Networking.IsMaster)
				SetTurnData(seatManager.DefaultTurnData);
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
			SetTurnData(seatManager.DefaultTurnData);
		}

		protected virtual void OnTurnDataChange(DataChangeState changeState)
		{
			MDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");

			UpdateCurTurnDataUI();

			if (changeState != DataChangeState.None)
				seatManager.UpdateStuff();
		}

		private void UpdateCurTurnDataUI()
		{
			if (seatManager == null)
				return;

			if (seatManager.IsTurnDataElement)
			{
				string curTurnDataString = (TurnData == NONE_INT) ? string.Empty :
										(seatManager.TurnDataToString.Length > TurnData) ? seatManager.TurnDataToString[TurnData] : TurnData.ToString();
				foreach (TextMeshProUGUI turnDataText in curTurnDataTexts)
					turnDataText.text = curTurnDataString;

				Sprite[] turnDataSprites = seatManager.TurnDataSprites;
				Sprite noneSprite = seatManager.TurnDataNoneSprite;
				foreach (Image curTurnDataImage in curTurnDataImages)
				{
					if (seatManager.UseTurnDataSprites)
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
			if (seatManager == null)
				return;

			if (seatManager.IsTurnDataElement)
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
				{
					if (i >= seatManager.TurnDataToString.Length)
					{
						turnDataTexts[i].text = i.ToString();
					}
					else
					{
						turnDataTexts[i].text = seatManager.TurnDataToString[i];
					}
				}

				for (int i = 0; i < turnDataImages.Length; i++)
				{
					if (i >= seatManager.TurnDataToString.Length)
					{
						turnDataImages[i].sprite = seatManager.TurnDataNoneSprite;
					}
					else
					{
						turnDataImages[i].sprite = seatManager.TurnDataSprites[i];
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

			// SeatManager.UpdateStuff에서 각 Seat.UpdateStuff를 호출
			// OnTurnDataChange에서는 역으로 SeatManager.UpdateStuff를 호출
			// 때문에 무한 루프를 방지하기 위해,
			// TurnData가 변경되어 Setter에서 OnTurnDataChange가 호출된 것인지,
			// SeatManager.UpdateStuff가 호출되어 OnTurnDataChange가 호출된 것인지 구분시켜줄 필요가 있음.
			// OnTurnDataChange(DataChangeState.None);
			
			// 240801 → OnTurnDataChange에서 UI 갱신 코드를 분리
		}

		protected override void OnOwnerChange(DataChangeState changeState)
		{
			if (changeState != DataChangeState.None)
			{
				if (seatManager.ResetTurnDataWhenOwnerChange)
					ResetTurnData();
			}

			base.OnOwnerChange(changeState);
		}
	}
}
