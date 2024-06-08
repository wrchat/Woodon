using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MSeat : MBase
	{
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(OwnerID))]
		private int _ownerID = NONE_INT;
		public int OwnerID
		{
			get => _ownerID;
			set
			{
				_ownerID = value;
				OnOwnerChange();
			}
		}

		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(Data))]
		private int _data = NONE_INT;
		public int Data
		{
			get => _data;
			set
			{
				_data = value;
				OnDataChange();
			}
		}

		public int Index { get; private set; }

		protected MTurnSeatManager seatManager;

		[Header("_" + nameof(MSeat))]
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private ObjectActive ownerObjectActive;
		[SerializeField] private TextMeshProUGUI[] ownerNameTexts;

		public virtual void Init(MTurnSeatManager seatManager, int index)
		{
			this.seatManager = seatManager;
		
			Index = index;
			foreach (var seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			SetData(seatManager.DefaultData);

			OnOwnerChange();
			OnDataChange();
		}

		protected virtual void OnOwnerChange()
		{
			if (ownerObjectActive)
				ownerObjectActive.SetActive(IsLocalPlayerID(OwnerID));

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

			if (seatManager)
				seatManager.UpdateStuff();
		}

		protected virtual void OnDataChange()
		{
			// MDebugLog($"{nameof(OnDataChange)}, {Data}");

			if (seatManager.IsDataState)
			{
				foreach (TextMeshProUGUI dataText in dataTexts)
					dataText.text = (Data != NONE_INT) ? seatManager.DataToString[Data] : string.Empty;
			
				Sprite[] dataSprites = seatManager.DataSprites;
				Sprite noneSprite = seatManager.DataNoneSprite;
				foreach (Image dataImage in dataImages)
				{
					if (seatManager.UseDataSprites)
					{
						dataImage.sprite = (Data != NONE_INT) ? dataSprites[Data] : noneSprite;
					}
					else
					{
						dataImage.sprite = (Data != NONE_INT) ? null : noneSprite;
					}
				}
			}
			else
			{
				foreach (TextMeshProUGUI dataText in dataTexts)
					dataText.text = Data.ToString();
			}

			if (seatManager)
				seatManager.UpdateStuff();
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
