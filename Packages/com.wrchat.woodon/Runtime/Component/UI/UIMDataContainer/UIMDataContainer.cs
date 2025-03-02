using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMDataContainer : WBase
	{
		[field: Header("_" + nameof(UIMDataContainer))]
		[field: SerializeField] public MDataContainer TargetMDataContainer { get; protected set; }

		[SerializeField] protected TextMeshProUGUI[] nameTexts;
		[SerializeField] protected TextMeshProUGUI[] valueTexts;
		[SerializeField] protected Image[] images;

		[SerializeField] protected TextMeshProUGUI[] dataTexts;
		[SerializeField] protected Image[] dataImages;

		[SerializeField] protected TextMeshProUGUI[] runtimeStringTexts;
		[SerializeField] protected TextMeshProUGUI[] runtimeIntTexts;

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
			WDebugLog($"{nameof(Init)}");

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

			if ((mDataContainerIndex.Value < 0) || (mDataContainerIndex.Value >= mDataContainers.Length))
			{
				UpdateUI(null);
				return;
			}

			UpdateUI(mDataContainers[mDataContainerIndex.Value]);
		}

		public virtual void UpdateUI(MDataContainer mDataContainer)
		{
			this.TargetMDataContainer = mDataContainer;

			WDebugLog($"{nameof(UpdateUI)}");

			if (mDataContainer == null)
			{
				foreach (TextMeshProUGUI nameText in nameTexts)
					if (nameText != null)
						nameText.text = string.Empty;

				foreach (TextMeshProUGUI valueText in valueTexts)
					if (valueText != null)
						valueText.text = string.Empty;

				foreach (Image image in images)
					if (image != null)
						image.enabled = false;

				foreach (TextMeshProUGUI dataText in dataTexts)
					dataText.text = string.Empty;

				foreach (Image dataImage in dataImages)
					if (dataImage != null)
						dataImage.enabled = false;

				foreach (TextMeshProUGUI runtimeIntText in runtimeIntTexts)
					if (runtimeIntText != null)
						runtimeIntText.text = string.Empty;

				foreach (TextMeshProUGUI runtimeStringText in runtimeStringTexts)
					if (runtimeStringText != null)
						runtimeStringText.text = string.Empty;
			}
			else
			{
				foreach (TextMeshProUGUI nameText in nameTexts)
					if (nameText != null)
						nameText.text = mDataContainer.Name;

				foreach (TextMeshProUGUI valueText in valueTexts)
					if (valueText != null)
						valueText.text = mDataContainer.Value;

				foreach (Image image in images)
				{
					if (image == null)
						continue;

					image.enabled = mDataContainer.Sprite != null;
					image.sprite = mDataContainer.Sprite;
				}

				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (dataTexts[i] == null)
						continue;

					if (i >= mDataContainer.StringData.Length)
					{
						dataTexts[i].text = string.Empty;
						continue;
					}

					dataTexts[i].text = mDataContainer.StringData[i];
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (dataImages[i] == null)
						continue;

					if (i >= mDataContainer.Sprites.Length)
					{
						dataImages[i].enabled = false;
						continue;
					}

					dataImages[i].enabled = true;
					dataImages[i].sprite = mDataContainer.Sprites[i];
				}

				foreach (TextMeshProUGUI runtimeIntText in runtimeIntTexts)
					if (runtimeIntText != null)
						runtimeIntText.text = mDataContainer.RuntimeInt.ToString();

				foreach (TextMeshProUGUI runtimeStringText in runtimeStringTexts)
					if (runtimeStringText != null)
						runtimeStringText.text = mDataContainer.RuntimeString;
			}
		}
	}
}