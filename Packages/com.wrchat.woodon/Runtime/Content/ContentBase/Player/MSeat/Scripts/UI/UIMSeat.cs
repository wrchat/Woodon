using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMSeat : MBase
	{
		[SerializeField] private TextMeshProUGUI[] indexTexts;
	
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

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

		private void UpdateCurTurnDataUI(int TurnData)
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