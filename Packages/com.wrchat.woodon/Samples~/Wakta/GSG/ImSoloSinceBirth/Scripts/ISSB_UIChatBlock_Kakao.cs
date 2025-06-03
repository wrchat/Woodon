
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.ImSoloSinceBirth
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ISSB_UIChatBlock_Kakao : WBase
	{
		[SerializeField] private GameObject[] chatObjects;
		[SerializeField] private TextMeshProUGUI[] autoSizeTexts;
		[SerializeField] private TextMeshProUGUI[] overrideTexts;

		// 콤보용
		public void SetChatText(DataList chatDataList)
		{
			WDebugLog($"{nameof(SetChatText)}");

			if (chatDataList == null || chatDataList.Count == 0)
			{
				foreach (GameObject chatObject in chatObjects)
					chatObject.SetActive(false);

				return;
			}

			for (int i = 0; i < chatObjects.Length; i++)
			{
				if (i < chatDataList.Count)
				{
					DataDictionary chatData = chatDataList[i].DataDictionary;
					chatObjects[i].SetActive(true);
					autoSizeTexts[i].text = chatData["Message"].String;
					overrideTexts[i].text = chatData["Message"].String;
				}
				else
				{
					chatObjects[i].SetActive(false);
				}
			}

			foreach (GameObject chatObject in chatObjects)
			{
				RectTransform[] rectTransforms = chatObject.transform.GetComponentsInChildren<RectTransform>();
				foreach (RectTransform item in rectTransforms)
					LayoutRebuilder.ForceRebuildLayoutImmediate(item);
			}
		}
	}
}