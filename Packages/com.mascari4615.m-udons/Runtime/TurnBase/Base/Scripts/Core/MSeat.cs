using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MSeat : MBase
	{
		public int OwnerID
		{
			get => _ownerID;
			set
			{
				int origin = _ownerID;
				_ownerID = value;
				OnOwnerChange(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(OwnerID))] private int _ownerID = NONE_INT;

		public int Data
		{
			get => _data;
			set
			{
				int origin = _data;
				_data = value;
				OnDataChange(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(Data))] private int _data = NONE_INT;

		public int Index { get; private set; }

		protected MTurnBaseManager seatManager;

		[Header("_" + nameof(MSeat))]
		[SerializeField] private MBool ownerMBool;
		[SerializeField] private TextMeshProUGUI[] ownerNameTexts;
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;
		
		public bool IsSeatOwner(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			return OwnerID == targetPlayer.playerId;
		}

		public virtual void Init(MTurnBaseManager seatManager, int index)
		{
			this.seatManager = seatManager;
		
			Index = index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			SetData(seatManager.DefaultData);

			UpdateStuff();
		}

		public virtual void UpdateStuff()
		{
			MDebugLog($"{nameof(UpdateStuff)}");

			UpdateOwnerUI();
			UpdateCurDataUI();
			UpdateDataUI();

			// OnOwnerChange(DataChangeState.None);
			// OnDataChange(DataChangeState.None);
		}

		protected virtual void OnOwnerChange(DataChangeState changeState)
		{
			if (seatManager == null)
				return;

			if (ownerMBool)
				ownerMBool.SetValue(IsLocalPlayerID(OwnerID));

			UpdateOwnerUI();

			if (changeState != DataChangeState.None)
				seatManager.UpdateStuff();
		}

		private void UpdateOwnerUI()
		{
			foreach (TextMeshProUGUI ownerNameText in ownerNameTexts)
			{
				string ownerName;
				VRCPlayerApi ownerPlayerAPI = VRCPlayerApi.GetPlayerById(OwnerID);

				if (ownerPlayerAPI == null)
					ownerName = "-";
				else
					ownerName = ownerPlayerAPI.displayName;

				ownerNameText.text = ownerName;
			}
		}

		protected virtual void OnDataChange(DataChangeState changeState)
		{
			// MDebugLog($"{nameof(OnDataChange)}, {Data}");
			
			if (seatManager == null)
				return;

			UpdateCurDataUI();

			if (changeState != DataChangeState.None)
				seatManager.UpdateStuff();
		}

		private void UpdateCurDataUI()
		{
			if (seatManager == null)
				return;

			if (seatManager.IsDataElement)
			{
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = (Data != NONE_INT) ? seatManager.DataToString[Data] : string.Empty;
			
				foreach (Image curDataImage in curDataImages)
				{
					if (seatManager.UseDataSprites)
					{
						curDataImage.sprite = (Data != NONE_INT) ? seatManager.DataSprites[Data] : seatManager.DataNoneSprite;
					}
					else
					{
						curDataImage.sprite = (Data != NONE_INT) ? null : seatManager.DataNoneSprite;
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
			if (seatManager == null)
				return;

			if (seatManager.IsDataElement)
			{
				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (i >= seatManager.DataToString.Length)
					{
						dataTexts[i].text = i.ToString();
					}
					else
					{
						dataTexts[i].text = seatManager.DataToString[i];
					}
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (i >= seatManager.DataSprites.Length)
					{
						dataImages[i].sprite = seatManager.DataNoneSprite;
					}
					else
					{
						dataImages[i].sprite = seatManager.DataSprites[i];
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
			SetOwner();
			Data = newData;
			RequestSerialization();
		}

		public void ResetData()
		{
			SetData(seatManager.DefaultData);
		}

		public void UseSeat()
		{
			if (seatManager.ResetDataWhenOwnerChange)
				ResetData();
			
			SetOwner();
			foreach (MSeat seat in seatManager.TurnSeats)
			{
				if (Networking.LocalPlayer.playerId == seat.OwnerID)
					seat.ResetSeat();
			}
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();
		}

		public void ResetSeat()
		{
			if (seatManager.ResetDataWhenOwnerChange)
				ResetData();

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (player.playerId == OwnerID)
			{
				if (Networking.IsMaster)
					ResetSeat();
			}
		}
	}
}
