using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMSeat : MBase
	{
		[Header("_" + nameof(UIMSeat))]
		[SerializeField] private TextMeshProUGUI[] indexTexts;

		[Header("_" + nameof(UIMSeat) + " - CurData")]
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

		[Header("_" + nameof(UIMSeat) + " - CurTurnData")]
		[SerializeField] private TextMeshProUGUI[] curTurnDataTexts;
		[SerializeField] private Image[] curTurnDataImages;
		[SerializeField] private TextMeshProUGUI[] turnDataTexts;
		[SerializeField] private Image[] turnDataImages;

		private ContentManager contentManager;
		private MSeat mSeat;

		public void Init(ContentManager contentManager, MSeat mSeat)
		{
			this.contentManager = contentManager;
			this.mSeat = mSeat;
		}

		public void UpdateUI()
		{
			int index = mSeat.Index;
			foreach (TextMeshProUGUI seatIndexText in indexTexts)
				seatIndexText.text = index.ToString();

			UpdateDataUI();
			UpdateTurnDataUI();

			UpdateCurDataUI(mSeat.IntData);
			UpdateCurTurnDataUI(mSeat.TurnData);
		}

		private void UpdateCurDataUI(int IntData)
		{
			if (contentManager == null)
				return;

			ContentDataOption dataOption = contentManager.GetDataOption(ContentManager.IntDataString);
			if (dataOption.IsElement)
			{
				string curDataString = (IntData == NONE_INT) ? string.Empty :
										(dataOption.DataToString.Length > IntData) ? dataOption.DataToString[IntData] : IntData.ToString();
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = curDataString;

				Sprite[] dataSprites = dataOption.DataToSprites;
				Sprite noneSprite = dataOption.DataNoneSprite;
				foreach (Image curDataImage in curDataImages)
				{
					if (dataOption.UseDataSprites)
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

		private void UpdateCurTurnDataUI(int TurnData)
		{
			if (contentManager == null)
				return;

			ContentDataOption dataOption = contentManager.GetDataOption(ContentManager.TurnDataString);
			if (dataOption.IsElement)
			{
				string curTurnDataString = (TurnData == NONE_INT) ? string.Empty :
										(dataOption.DataToString.Length > TurnData) ? dataOption.DataToString[TurnData] : TurnData.ToString();
				foreach (TextMeshProUGUI turnDataText in curTurnDataTexts)
					turnDataText.text = curTurnDataString;

				Sprite[] turnDataSprites = dataOption.DataToSprites;
				Sprite noneSprite = dataOption.DataNoneSprite;
				foreach (Image curTurnDataImage in curTurnDataImages)
				{
					if (dataOption.UseDataSprites)
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

			ContentDataOption dataOption = contentManager.GetDataOption(ContentManager.IntDataString);
			if (dataOption.IsElement)
			{
				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (i >= dataOption.DataToString.Length)
					{
						dataTexts[i].text = i.ToString();
					}
					else
					{
						dataTexts[i].text = dataOption.DataToString[i];
					}
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (i >= dataOption.DataToString.Length)
					{
						dataImages[i].sprite = dataOption.DataNoneSprite;
					}
					else
					{
						dataImages[i].sprite = dataOption.DataToSprites[i];
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

			ContentDataOption dataOption = contentManager.GetDataOption(ContentManager.TurnDataString);
			if (dataOption.IsElement)
			{
				for (int i = 0; i < turnDataTexts.Length; i++)
				{
					if (i >= dataOption.DataToString.Length)
					{
						turnDataTexts[i].text = i.ToString();
					}
					else
					{
						turnDataTexts[i].text = dataOption.DataToString[i];
					}
				}

				for (int i = 0; i < turnDataImages.Length; i++)
				{
					if (i >= dataOption.DataToString.Length)
					{
						turnDataImages[i].sprite = dataOption.DataNoneSprite;
					}
					else
					{
						turnDataImages[i].sprite = dataOption.DataToSprites[i];
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