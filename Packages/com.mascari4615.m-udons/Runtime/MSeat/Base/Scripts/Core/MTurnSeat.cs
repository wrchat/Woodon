using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTurnSeat : MSeat
	{
		[UdonSynced(), FieldChangeCallback(nameof(TurnData))]
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
		[SerializeField] private TextMeshProUGUI[] turnDataTexts;
		[SerializeField] private Image[] turnDataImages;

		public override void Init(MTurnSeatManager seatManager, int index)
		{
			base.Init(seatManager, index);

			SetTurnData(seatManager.DefaultTurnData);
			OnTurnDataChange(DataChangeState.None);
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
			// MDebugLog($"{nameof(OnTurnDataChange)}, {TurnData}");

			if (seatManager.IsTurnDataState)
			{
				foreach (TextMeshProUGUI turnDataText in turnDataTexts)
					turnDataText.text = (TurnData != NONE_INT) ? seatManager.TurnDataToString[TurnData] : string.Empty;

				Sprite[] turnDataSprites = seatManager.TurnDataSprites;
				Sprite noneSprite = seatManager.TurnDataNoneSprite;
				foreach (Image turnDataImage in turnDataImages)
				{
					if (seatManager.UseTurnDataSprites)
					{
						turnDataImage.sprite = (TurnData != NONE_INT) ? turnDataSprites[TurnData] : noneSprite;
					}
					else
					{
						turnDataImage.sprite = (TurnData != NONE_INT) ? null : noneSprite;
					}
				}
			}
			else
			{
				foreach (TextMeshProUGUI turnDataText in turnDataTexts)
					turnDataText.text = TurnData.ToString();
			}

			if (seatManager)
				seatManager.UpdateStuff();
		}

		public virtual void UpdateStuff()
		{
			// MDebugLog($"{nameof(UpdateStuff)}");

			OnTurnDataChange(DataChangeState.None);
		}

		protected override void OnOwnerChange()
		{
			if (seatManager.ResetTurnDataWhenOwnerChange)
				ResetTurnData();
			base.OnOwnerChange();
		}
	}
}
