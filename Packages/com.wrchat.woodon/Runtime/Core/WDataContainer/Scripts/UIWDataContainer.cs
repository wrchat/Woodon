using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWDataContainer : WBase
	{
		[field: Header("_" + nameof(UIWDataContainer))]
		[field: SerializeField] public WDataContainer TargetWDataContainer { get; protected set; }

		[SerializeField] protected TextMeshProUGUI[] nameTexts;
		[SerializeField] protected TextMeshProUGUI[] valueTexts;
		[SerializeField] protected Image[] images;

		[SerializeField] protected TextMeshProUGUI[] dataTexts;
		[SerializeField] protected Image[] dataImages;

		[SerializeField] protected TextMeshProUGUI[] runtimeStringTexts;
		[SerializeField] protected TextMeshProUGUI[] runtimeIntTexts;

		[Header("_" + nameof(UIWDataContainer) + " - Options")]
		[SerializeField] protected Transform wDataContainerParent;
		[SerializeField] protected WDataContainer[] wDataContainers;
		[SerializeField] protected WInt wDataContainerIndex;

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			WDebugLog($"{nameof(Init)}");

			if (wDataContainers == null || wDataContainers.Length == 0)
			{
				if (wDataContainerParent != null)
				{
					wDataContainers = wDataContainerParent.GetComponentsInChildren<WDataContainer>();
				}
			}

			if (wDataContainerIndex != null)
				wDataContainerIndex.RegisterListener(this, nameof(UpdateUIByWIntByIndex));
		}

		public void UpdateUIByWIntByIndex()
		{
			if (wDataContainers == null || wDataContainers.Length == 0)
				return;

			if (wDataContainerIndex == null)
				return;

			if ((wDataContainerIndex.Value < 0) || (wDataContainerIndex.Value >= wDataContainers.Length))
			{
				UpdateUI(null);
				return;
			}

			UpdateUI(wDataContainers[wDataContainerIndex.Value]);
		}

		public virtual void UpdateUI(WDataContainer wDataContainer)
		{
			this.TargetWDataContainer = wDataContainer;

			WDebugLog($"{nameof(UpdateUI)}");

			if (wDataContainer == null)
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
					if (dataText != null)
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
						nameText.text = wDataContainer.Name;

				foreach (TextMeshProUGUI valueText in valueTexts)
					if (valueText != null)
						valueText.text = wDataContainer.Value;

				foreach (Image image in images)
				{
					if (image == null)
						continue;

					image.enabled = wDataContainer.Sprite != null;
					image.sprite = wDataContainer.Sprite;
				}

				for (int i = 0; i < dataTexts.Length; i++)
				{
					if (dataTexts[i] == null)
						continue;

					if (i >= wDataContainer.StringData.Length)
					{
						dataTexts[i].text = string.Empty;
						continue;
					}

					dataTexts[i].text = wDataContainer.StringData[i];
				}

				for (int i = 0; i < dataImages.Length; i++)
				{
					if (dataImages[i] == null)
						continue;

					if (i >= wDataContainer.Sprites.Length)
					{
						dataImages[i].enabled = false;
						continue;
					}

					dataImages[i].enabled = true;
					dataImages[i].sprite = wDataContainer.Sprites[i];
				}

				foreach (TextMeshProUGUI runtimeIntText in runtimeIntTexts)
					if (runtimeIntText != null)
						runtimeIntText.text = wDataContainer.RuntimeInt.ToString();

				foreach (TextMeshProUGUI runtimeStringText in runtimeStringTexts)
					if (runtimeStringText != null)
						runtimeStringText.text = wDataContainer.RuntimeString;
			}
		}
	}
}