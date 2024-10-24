using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MAvatarList : MBase
	{
		[SerializeField] private GameObject loadingPanel;
		[SerializeField] private VRCUrl jsonLink;
		[SerializeField] private MValue mScore_Type;
		[SerializeField] private MValue mScore_Page;
		[SerializeField] private TextMeshProUGUI typeText;

		private int maxAvatarPerPage;

		private UIAvatarCategoryButton[] categoryButtons;
		private UIAvatarButton[] avatarButtons;

		private DataDictionary data;

		private void Start()
		{
			Init();

			RefeshLoop();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");
		
			categoryButtons = GetComponentsInChildren<UIAvatarCategoryButton>(true);
			avatarButtons = GetComponentsInChildren<UIAvatarButton>(true);

			maxAvatarPerPage = avatarButtons.Length;

			mScore_Type.RegisterListener(this, nameof(UpdateUI));
			mScore_Type.RegisterListener(this, nameof(RecalcPageMinMax));

			mScore_Page.RegisterListener(this, nameof(UpdateUI));
		}

		public void RecalcPageMinMax()
		{
			DataList types = data.GetKeys();
			DataToken selectedType = types[mScore_Type.Value];
			DataDictionary selectedTypeData = data[selectedType].DataDictionary;

			mScore_Page.SetMinMaxValue(0, Mathf.CeilToInt(selectedTypeData.Count / (float)maxAvatarPerPage) - 1, true);
			mScore_Page.SetValue(0);
		}

		public void RefeshLoop()
		{
			Refresh();
			SendCustomEventDelayedSeconds(nameof(RefeshLoop), 300f);
		}

		public void Refresh()
		{
			loadingPanel.SetActive(true);

			IUdonEventReceiver receiver = (IUdonEventReceiver)this;
			VRCStringDownloader.LoadUrl(jsonLink, receiver);
		}

		public override void OnStringLoadSuccess(IVRCStringDownload result)
		{
			MDebugLog($"{nameof(OnStringLoadSuccess)} : {result.Result} - {result}");

			if (!VRCJson.TryDeserializeFromJson(result.Result, out DataToken dataToken) ||
				(dataToken.TokenType != TokenType.DataDictionary))
			{
				MDebugLog($"Failed to Deserialize json : {result.Result} - {result}");
				return;
			}

			data = dataToken.DataDictionary;

			mScore_Type.SetMinMaxValue(0, data.GetKeys().Count - 1, true);
			mScore_Type.SetValue(0);

			loadingPanel.SetActive(false);
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)}");

			// Type
			DataList types = data.GetKeys();
			MDebugLog($"All Categorys : {types.Count}");
			typeText.text = $"{types[mScore_Type.Value]}";

			for (int i = 0; i < categoryButtons.Length; i++)
			{
				if (i < types.Count)
				{
					categoryButtons[i].SetData(i, types[i].ToString());
					categoryButtons[i].gameObject.SetActive(true);
				}
				else
				{
					categoryButtons[i].gameObject.SetActive(false);
				}
			}

			// Page
			DataToken selectedType = types[mScore_Type.Value];
			DataDictionary selectedTypeData = data[selectedType].DataDictionary;

			DataList curTypeAvatarIDs = selectedTypeData.GetValues();
			DataList curTypeAvatarNames = selectedTypeData.GetKeys();

			for (int buttonIndex = 0; buttonIndex < maxAvatarPerPage; buttonIndex++)
			{
				UIAvatarButton avatarButton = avatarButtons[buttonIndex];

				if (buttonIndex < selectedTypeData.Count - (maxAvatarPerPage * mScore_Page.Value))
				{
					avatarButton.gameObject.SetActive(true);

					string id = curTypeAvatarIDs[buttonIndex + (maxAvatarPerPage * mScore_Page.Value)].ToString();
					string name = curTypeAvatarNames[buttonIndex + (maxAvatarPerPage * mScore_Page.Value)].ToString();
					avatarButton.SetData(id, name);
				}
				else
				{
					avatarButton.gameObject.SetActive(false);
				}
			}
		}

		public void SelectType(int type)
		{
			mScore_Type.SetValue(type);
			mScore_Page.SetValue(0);

			UpdateUI();
		}
	}
}