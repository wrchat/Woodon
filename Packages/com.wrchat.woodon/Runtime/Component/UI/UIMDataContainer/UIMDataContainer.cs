using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class UIMDataContainer : MBase
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

			MDebugLog($"{nameof(UpdateUI)}");

			if (mDataContainer == null)
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

				foreach (TextMeshProUGUI runtimeIntText in runtimeIntTexts)
					runtimeIntText.text = string.Empty;

				foreach (TextMeshProUGUI runtimeStringText in runtimeStringTexts)
					runtimeStringText.text = string.Empty;
			}
			else
			{
				foreach (TextMeshProUGUI nameText in nameTexts)
					nameText.text = mDataContainer.Name;

				foreach (TextMeshProUGUI valueText in valueTexts)
					valueText.text = mDataContainer.Value;

				foreach (Image image in images)
				{
					image.enabled = mDataContainer.Sprite != null;
					image.sprite = mDataContainer.Sprite;
				}

				for (int i = 0; i < dataTexts.Length; i++)
					dataTexts[i].text = mDataContainer.StringData[i];

				for (int i = 0; i < dataImages.Length; i++)
				{
					dataImages[i].enabled = mDataContainer.Sprites[i] != null;
					dataImages[i].sprite = mDataContainer.Sprites[i];
				}

				foreach (TextMeshProUGUI runtimeIntText in runtimeIntTexts)
					runtimeIntText.text = mDataContainer.RuntimeInt.ToString();

				foreach (TextMeshProUGUI runtimeStringText in runtimeStringTexts)
					runtimeStringText.text = mDataContainer.RuntimeString;
			}
		}
	}
}