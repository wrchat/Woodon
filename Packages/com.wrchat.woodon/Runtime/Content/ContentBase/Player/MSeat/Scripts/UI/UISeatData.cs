using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatData : MBase
	{
		[Header("_" + nameof(UISeatData))]
		[SerializeField] private string dataName = NONE_STRING;

		[Header("_" + nameof(UISeatData) + "_Data")]
		[SerializeField] private TextMeshProUGUI[] dataTexts;
		[SerializeField] private Image[] dataImages;

		[Header("_" + nameof(UISeatData) + "_CurData")]
		[SerializeField] private TextMeshProUGUI[] curDataTexts;
		[SerializeField] private Image[] curDataImages;

		public void UpdateUI(ContentManager contentManager, MSeat mSeat)
		{
			SeatDataOption dataOption = contentManager.GetSeatDataOption(dataName);
			int data = mSeat.GetData(dataName);

			if (dataOption == null)
			{
				MDebugLog($"{nameof(UpdateUI)} - {dataName} - {dataOption} is null. Please check the dataName.", LogType.Error);
				return;
			}

			UpdateDataUI(dataOption);
			UpdateCurDataUI(dataOption, data);
		}

		// 모든 데이터의 경우를 표현합니다
		// 퀴즈쇼 컨텐츠로 예를 든다면, 모든 선택지를 표현합니다.
		private void UpdateDataUI(SeatDataOption dataOption)
		{
			if (dataOption.IsElement)
			{
				for (int i = 0; i < dataTexts.Length; i++)
				{
					// 나머지는 숫자로 표시
					if (i >= dataOption.DataToString.Length)
					{
						dataTexts[i].text = i.ToString();
						continue;
					}

					dataTexts[i].text = dataOption.DataToString[i];
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					// 나머지는 NonSprite로 표시
					if (i >= dataOption.DataToString.Length)
					{
						dataImages[i].sprite = dataOption.DataNoneSprite;
						continue;
					}

					dataImages[i].sprite = dataOption.DataToSprites[i];
				}
			}
			else
			{
				for (int i = 0; i < dataTexts.Length; i++)
					dataTexts[i].text = i.ToString();
			}
		}

		// 현재 데이터를 표현합니다
		// 퀴즈쇼 컨텐츠로 예를 든다면, 현재 선택한 선택지를 표현합니다.
		private void UpdateCurDataUI(SeatDataOption dataOption, int data)
		{
			if (dataOption.IsElement)
			{
				string curDataString;
				{
					if (data == NONE_INT)
						curDataString = string.Empty;
					else if (dataOption.DataToString.Length > data)
						curDataString = dataOption.DataToString[data];
					else
						curDataString = data.ToString();
				}
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = curDataString;

				Sprite sprite;
				{
					if (data == NONE_INT)
						sprite = dataOption.DataNoneSprite;
					else if (dataOption.DataToSprites.Length > data)
						sprite = dataOption.DataToSprites[data];
					else
						sprite = dataOption.DataNoneSprite;
				}
				foreach (Image curDataImage in curDataImages)
					curDataImage.sprite = sprite;
			}
			else
			{
				foreach (TextMeshProUGUI curDataText in curDataTexts)
					curDataText.text = data.ToString();
			}
		}
	}
}