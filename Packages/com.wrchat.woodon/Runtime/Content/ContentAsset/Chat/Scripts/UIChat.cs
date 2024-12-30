
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public class UIChat : MBase
	{
		[Header("_" + nameof(UIChat))]
		[SerializeField] private TextMeshProUGUI[] chatTexts;
		[SerializeField] private MValue chatRoomIndex;

		// TODO: UIChat 인터페이스와 분리, UIChat을 상속 받는 별개의 구현으로
		[SerializeField] private GameObject[] chatObjects;
		[SerializeField] private TextMeshProUGUI[] autoSizeTexts;
		[SerializeField] private TextMeshProUGUI[] overrideTexts;

		public TeamType ChatRoom => (TeamType)chatRoomIndex.Value;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");
			SetChatText(null, string.Empty);
		}

		public virtual void SetChatText(DataList chatDataList, string debugText)
		{
			MDebugLog($"{nameof(SetChatText)}: {debugText}");

			if (chatDataList == null || chatDataList.Count == 0)
			{
				foreach (TextMeshProUGUI chatText in chatTexts)
					chatText.text = string.Empty;

				foreach (GameObject chatObject in chatObjects)
					chatObject.SetActive(false);

				return;
			}

			foreach (TextMeshProUGUI chatText in chatTexts)
				chatText.text = debugText;

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