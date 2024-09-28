using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawElementBlock : MBase
	{
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Image spriteImage;
		[SerializeField] private TextMeshProUGUI[] syncDataTexts;

		[SerializeField] private Image hider;

		public void UpdateUI(DrawElementData drawElementData)
		{
			nameText.text = drawElementData.Name;
			spriteImage.sprite = drawElementData.Sprite;
			foreach (TextMeshProUGUI syncDataText in syncDataTexts)
			{
				if (drawElementData.SyncData == NONE_STRING)
					syncDataText.text = string.Empty;
				else
					syncDataText.text = drawElementData.SyncData;
			}
			hider.gameObject.SetActive(drawElementData.IsShowing == false);
		}
	}
}