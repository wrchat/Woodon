using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MSeat : MTarget
	{
		[Header("_" + nameof(MSeat))]
		[SerializeField] private MValue dataMValue;
		public int Data => dataMValue.Value;
		public int Index { get; private set; }
		protected MTurnBaseManager turnBaseManager;
	
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

		public virtual void Init(MTurnBaseManager turnBaseManager, int index)
		{
			this.turnBaseManager = turnBaseManager;
		
			Index = index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			SetData(turnBaseManager.DefaultData);
			dataMValue.RegisterListener(this, nameof(OnDataChange_Greater), (int)MValueEvent.OnValueIncreased);
			dataMValue.RegisterListener(this, nameof(OnDataChange_Less), (int)MValueEvent.OnValueDecreased);

			UpdateStuff();
		}

		public virtual void UpdateStuff()
		{
			MDebugLog($"{nameof(UpdateStuff)}");

			UpdateCurDataUI();
			UpdateDataUI();

			// OnTargetChange(DataChangeState.None);
			// OnDataChange(DataChangeState.None);
		}

		protected override void OnTargetChange(DataChangeState changeState)
		{
			if (turnBaseManager == null)
				return;

			base.OnTargetChange(changeState);
		
			if (changeState != DataChangeState.None)
				turnBaseManager.UpdateStuff();
		}

		public void OnDataChange_Greater() => OnDataChange(DataChangeState.Greater);
		public void OnDataChange_Less() => OnDataChange(DataChangeState.Less);
		protected virtual void OnDataChange(DataChangeState changeState)
		{
			// MDebugLog($"{nameof(OnDataChange)}, {Data}");
			
			if (turnBaseManager == null)
				return;

			UpdateCurDataUI();

			if (changeState != DataChangeState.None)
				turnBaseManager.UpdateStuff();
		}

		private void UpdateCurDataUI()
		{
			if (turnBaseManager == null)
				return;

			if (turnBaseManager.IsDataElement)
			{
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = (Data != NONE_INT) ? turnBaseManager.DataToString[Data] : string.Empty;
			
				foreach (Image curDataImage in curDataImages)
				{
					if (turnBaseManager.UseDataSprites)
					{
						curDataImage.sprite = (Data != NONE_INT) ? turnBaseManager.DataSprites[Data] : turnBaseManager.DataNoneSprite;
					}
					else
					{
						curDataImage.sprite = (Data != NONE_INT) ? null : turnBaseManager.DataNoneSprite;
					}
				}
			}
			else
			{
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = Data.ToString();
			}
		}

		private void UpdateDataUI()
		{
			if (turnBaseManager == null)
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
					if (i >= turnBaseManager.DataSprites.Length)
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

		public void SetData(int newData)
		{
			dataMValue.SetValue(newData);
		}

		public void ResetData()
		{
			SetData(turnBaseManager.DefaultData);
		}

		public void UseSeat()
		{
			if (turnBaseManager.ResetDataWhenOwnerChange)
				ResetData();
			
			foreach (MSeat seat in turnBaseManager.TurnSeats)
			{
				if (seat.IsTargetPlayer(Networking.LocalPlayer))
					seat.ResetSeat();
			}
			SetTargetLocalPlayer();
		}

		public void ResetSeat()
		{
			ResetPlayer();
			if (turnBaseManager.ResetDataWhenOwnerChange)
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
