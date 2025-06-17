
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;
using WRC.Woodon.Chat;

namespace Mascari4615.Project.ISD.GSG.ImSoloSinceBirth
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ISSB_UIChat : UIChat
	{
		[SerializeField] private ISSB_UIChatBlock_Kakao[] chatBlocks_Left;
		[SerializeField] private ISSB_UIChatBlock_Kakao[] chatBlocks_Right;
		[SerializeField] private RectTransform chatBlockParent;

		public override void SetChatText(DataList chatDataList, string debugText)
		{
			WDebugLog($"{nameof(SetChatText)}: {debugText}");

			foreach (ISSB_UIChatBlock_Kakao chatBlock in chatBlocks_Left)
				chatBlock.gameObject.SetActive(false);
			foreach (ISSB_UIChatBlock_Kakao chatBlock in chatBlocks_Right)
				chatBlock.gameObject.SetActive(false);

			if (chatDataList == null || chatDataList.Count == 0)
				return;

			DataList chatDataCombo = new DataList();
			int chatBlockLeftIndex = 0;
			int chatBlockRightIndex = 0;

			for (int i = 0; i < chatDataList.Count; i++)
			{
				DataDictionary chatData = chatDataList[i].DataDictionary;
				// string name = chatData["Name"].String;
				// int udonIndex = chatData["UdonIndex"].Int();
				bool isLeft = chatData.GetChatAdditionalData() == "Left";

				if (chatDataCombo.Count == 0)
				{
					chatDataCombo.Add(chatDataList[i]);
				}
				else
				{
					DataDictionary lastChatData = chatDataCombo[0].DataDictionary;
					// string lastName = lastChatData["Name"].String;
					// int lastUdonIndex = lastChatData["UdonIndex"].Int();
					bool isLastLeft = chatDataCombo[0].DataDictionary.GetChatAdditionalData() == "Left";

					if (isLeft == isLastLeft)
					{
						chatDataCombo.Add(chatDataList[i]);
					}
					else
					{
						// bool isLeft = chatDataCombo[0].DataDictionary.GetChatAdditionalData() == "Left";
						WDebugLog($"isLeft: {isLeft} ({chatDataCombo[0].DataDictionary.GetChatAdditionalData()})");

						if (isLastLeft)
						{
							chatBlocks_Left[chatBlockLeftIndex].SetChatText(chatDataCombo);
							chatBlocks_Left[chatBlockLeftIndex].gameObject.SetActive(true);
							chatBlocks_Left[chatBlockLeftIndex].transform.SetAsLastSibling();
							chatDataCombo.Clear();
							chatDataCombo.Add(chatDataList[i]);
							chatBlockLeftIndex++;
						}
						else
						{
							chatBlocks_Right[chatBlockRightIndex].SetChatText(chatDataCombo);
							chatBlocks_Right[chatBlockRightIndex].gameObject.SetActive(true);
							chatBlocks_Right[chatBlockRightIndex].transform.SetAsLastSibling();
							chatDataCombo.Clear();
							chatDataCombo.Add(chatDataList[i]);
							chatBlockRightIndex++;
						}
					}
				}
			}

			if (chatDataCombo.Count > 0)
			{
				bool isLeft = chatDataCombo[0].DataDictionary.GetChatAdditionalData() == "Left";
				if (isLeft)
				{
					chatBlocks_Left[chatBlockLeftIndex].SetChatText(chatDataCombo);
					chatBlocks_Left[chatBlockLeftIndex].gameObject.SetActive(true);
					chatBlocks_Left[chatBlockLeftIndex].transform.SetAsLastSibling();
				}
				else
				{
					chatBlocks_Right[chatBlockRightIndex].SetChatText(chatDataCombo);
					chatBlocks_Right[chatBlockRightIndex].gameObject.SetActive(true);
					chatBlocks_Right[chatBlockRightIndex].transform.SetAsLastSibling();
				}
			}

			foreach (ISSB_UIChatBlock_Kakao chatBlock in chatBlocks_Left)
			{
				RectTransform[] rectTransforms = chatBlock.transform.GetComponentsInChildren<RectTransform>();
				foreach (RectTransform item in rectTransforms)
					LayoutRebuilder.ForceRebuildLayoutImmediate(item);
			}
			foreach (ISSB_UIChatBlock_Kakao chatBlock in chatBlocks_Right)
			{
				RectTransform[] rectTransforms = chatBlock.transform.GetComponentsInChildren<RectTransform>();
				foreach (RectTransform item in rectTransforms)
					LayoutRebuilder.ForceRebuildLayoutImmediate(item);
			}

			RectTransform[] rectTransformss = chatBlockParent.GetComponentsInChildren<RectTransform>();
			foreach (RectTransform item in rectTransformss)
				LayoutRebuilder.ForceRebuildLayoutImmediate(item);
		}
	}
}