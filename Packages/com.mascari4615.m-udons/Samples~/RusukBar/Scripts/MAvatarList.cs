

using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MAvatarList : MBase
	{
		[SerializeField] private GameObject loadingPanel;
		[SerializeField] private VRCUrl jsonLink;
		[SerializeField] private MScore categoryIndex_MScore;
		[SerializeField] private TextMeshProUGUI categoryText;

		[SerializeField] private RawImage asdd;
		[SerializeField] private VRCAvatarPedestal fff;

		public int selectedAvatarType;
		private DataList data;

		private void Start()
		{
			RefeshLoop();
		}

		public void RefeshLoop()
		{
			Refresh();
			SendCustomEventDelayedSeconds(nameof(Refresh), 300f);
		}

		public void Refresh()
		{
			loadingPanel.SetActive(true);
			VRCStringDownloader.LoadUrl(jsonLink);
		}

		public override void OnStringLoadSuccess(IVRCStringDownload result)
		{
			if (!VRCJson.TryDeserializeFromJson(result.Result, out DataToken dataToken) ||
				(dataToken.TokenType != TokenType.DataDictionary))
			{
				MDebugLog($"Failed to Deserialize json : {result.Result} - {result}");
				return;
			}

			DataDictionary tempDictionary = dataToken.DataDictionary;
			data = tempDictionary.GetKeys();

			loadingPanel.SetActive(false);
			UpdateUI();
		}

		private void UpdateUI()
		{
			// UpdateCatrgory
			/*{
				categoryText.text = $"{data[categoryIndex_MScore.Score]} {categoryIndex_MScore.Score}/{data.Count}";

				categoryCount = ;
				MDebugLog($"All Categorys : {categoryCount}");
				for (int i = 0; i < categoryButtons.Length; i++)
				{
					if (i < categoryCount - (categoryButtons.Length * categoryPage))
					{
						int categoryId = i + (categoryButtons.Length * categoryPage);
						categoryButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data[categoryId].ToString();
						categoryButtons[i].GetComponent<UdonBehaviour>().SetProgramVariable("avatarType", categoryId);
					}
				}
				selectedAvatarType = 0 + (categoryButtons.Length * categoryPage);
				// MDebugLog($"Category Page : {categoryPage + 1}/{(categoryCount / categoryButtons.Length) + ((categoryCount % categoryButtons.Length != 0) ? 1 : 0)}");
			}

			DataToken selectedList = data[selectedAvatarType];
			DataList avatarList = tempDictionary[selectedList].DataDictionary.GetValues();

			int curCategoryAvatarCount = avatarList.Count;
			//pageCounter.text = ($"{page + 1}/{((itemCount / 6) + ((itemCount % 6 != 0) ? 1 : 0))}");

			Debug.Log($"Successfully loaded Avatar Dictionary with {curCategoryAvatarCount} items in {selectedList} list.");

			for (int i = 0; i < 6; i++)
			{
				if (i < curCategoryAvatarCount - (6 * page))
				{
					string id = avatarList[i + (6 * page)].ToString();
					avatarButtons[i].SetActive(true);
					avatarButtons[i].transform.GetChild(0).GetComponent<VRCAvatarPedestal>().blueprintId = id;
					avatarButtons[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Trigger");
				}
				else
					avatarButtons[i].SetActive(false);
			}*/
		}
	}
}