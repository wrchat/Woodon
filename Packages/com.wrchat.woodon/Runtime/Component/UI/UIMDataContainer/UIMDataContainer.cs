using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class UIMDataContainer : MBase
	{
		[Header("_" + nameof(UIMDataContainer))]
		[SerializeField] protected MDataContainer targetMDataContainer;

		[SerializeField] protected TextMeshProUGUI[] nameTexts;
		[SerializeField] protected TextMeshProUGUI[] valueTexts;
		[SerializeField] protected Image[] images;

		[SerializeField] protected TextMeshProUGUI[] dataTexts;
		[SerializeField] protected Image[] dataImages;

		[SerializeField] protected TextMeshProUGUI[] syncedDataTexts;

		[Header("_" + nameof(UIMDataContainer) + " - Options")]
		[SerializeField] protected Transform mDataContainerParent;
		[SerializeField] protected MDataContainer[] mDataContainers;
		[SerializeField] protected MValue mDataContainerIndex;

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (mDataContainers == null || mDataContainers.Length == 0)
			{
				if (mDataContainerParent != null)
				{
					mDataContainers = mDataContainerParent.GetComponentsInChildren<MDataContainer>();
				}
			}

			if (mDataContainerIndex != null)
				mDataContainerIndex.RegisterListener(this, nameof(UpdateUIByMValueIndex));
		}

		public void UpdateUIByMValueIndex()
		{
			if (mDataContainers == null || mDataContainers.Length == 0)
				return;

			if (mDataContainerIndex == null)
				return;

			if (mDataContainerIndex.Value < 0 || mDataContainerIndex.Value >= mDataContainers.Length)
				return;

			UpdateUI(mDataContainers[mDataContainerIndex.Value]);
		}

		public virtual void UpdateUI(MDataContainer mData)
		{
			this.targetMDataContainer = mData;

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
					image.enabled = mData.Sprite != null;
					image.sprite = mData.Sprite;
				}

				for (int i = 0; i < dataTexts.Length; i++)
					dataTexts[i].text = mData.StringData[i];

				for (int i = 0; i < dataImages.Length; i++)
				{
					dataImages[i].enabled = mData.Sprites[i] != null;
					dataImages[i].sprite = mData.Sprites[i];
				}

				foreach (TextMeshProUGUI syncedDataText in syncedDataTexts)
					syncedDataText.text = mData.RuntimeData;
			}
		}
	}
}