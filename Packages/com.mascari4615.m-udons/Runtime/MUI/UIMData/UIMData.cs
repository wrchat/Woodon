using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
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
				return;

			foreach (TextMeshProUGUI nameText in nameTexts)
				nameText.text = mData.Name;

			foreach (TextMeshProUGUI valueText in valueTexts)
				valueText.text = mData.Value;

			foreach (Image image in images)
				image.sprite = mData.Sprite;
			
			for (int i = 0; i < dataTexts.Length; i++)
				dataTexts[i].text = mData.StringData[i].ToString();

			for (int i = 0; i < dataImages.Length; i++)
				dataImages[i].sprite = mData.Sprites[i];

			for (int i = 0; i < syncedDataTexts.Length; i++)
				syncedDataTexts[i].text = mData.SyncData;
		}
	}
}