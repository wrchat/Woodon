using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class UIMData : MBase
	{
		[Header("_" + nameof(UIMData))]
		[SerializeField] protected MData mData;

		[SerializeField] protected TextMeshProUGUI[] nameTexts;
		[SerializeField] protected TextMeshProUGUI[] valueTexts;
		[SerializeField] protected Image[] images;

		[SerializeField] protected TextMeshProUGUI[] dataTexts;
		[SerializeField] protected Image[] dataImages;

		[SerializeField] protected TextMeshProUGUI[] syncedDataTexts;

		public virtual void UpdateUI(MData mData)
		{
			this.mData = mData;

			MDebugLog($"{nameof(UpdateUI)}");

			if (mData == null)
			{
				foreach (TextMeshProUGUI nameText in nameTexts)
					nameText.text = string.Empty;

				foreach (TextMeshProUGUI valueText in valueTexts)
					valueText.text = string.Empty;

				foreach (Image image in images)
					image.enabled = false;

				foreach (TextMeshProUGUI dataText in dataTexts)
					dataText.text = string.Empty;

				foreach (Image dataImage in dataImages)
					dataImage.enabled = false;

				foreach (TextMeshProUGUI syncedDataText in syncedDataTexts)
					syncedDataText.text = string.Empty;
			}
			else
			{
				foreach (TextMeshProUGUI nameText in nameTexts)
					nameText.text = mData.Name;

				foreach (TextMeshProUGUI valueText in valueTexts)
					valueText.text = mData.Value;

				foreach (Image image in images)
				{
					image.enabled = true;
					image.sprite = mData.Sprite;
				}

				for (int i = 0; i < dataTexts.Length; i++)
					dataTexts[i].text = mData.StringData[i];

				for (int i = 0; i < dataImages.Length; i++)
				{
					dataImages[i].enabled = true;
					dataImages[i].sprite = mData.Sprites[i];
				}

				foreach (TextMeshProUGUI syncedDataText in syncedDataTexts)
					syncedDataText.text = mData.RuntimeData;
			}
		}
	}
}