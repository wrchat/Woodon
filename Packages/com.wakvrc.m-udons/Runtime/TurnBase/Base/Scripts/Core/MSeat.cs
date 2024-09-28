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

		protected MTurnBaseManager turnBaseManager;

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

		public virtual void Init(MTurnBaseManager turnBaseManager, int index)
		{
			this.turnBaseManager = turnBaseManager;
		
			Index = index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			SetData(turnBaseManager.DefaultData);

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
			if (turnBaseManager == null)
				return;

			if (ownerMBool)
				ownerMBool.SetValue(IsLocalPlayerID(OwnerID));

			UpdateOwnerUI();

			if (changeState != DataChangeState.None)
				turnBaseManager.UpdateStuff();
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
			SetOwner();
			Data = newData;
			RequestSerialization();
		}

		public void ResetData()
		{
			SetData(turnBaseManager.DefaultData);
		}

		public void UseSeat()
		{
			if (turnBaseManager.ResetDataWhenOwnerChange)
				ResetData();
			
			SetOwner();
			foreach (MSeat seat in turnBaseManager.TurnSeats)
			{
				if (Networking.LocalPlayer.playerId == seat.OwnerID)
					seat.ResetSeat();
			}
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();
		}

		public void ResetSeat()
		{
			if (turnBaseManager.ResetDataWhenOwnerChange)
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
