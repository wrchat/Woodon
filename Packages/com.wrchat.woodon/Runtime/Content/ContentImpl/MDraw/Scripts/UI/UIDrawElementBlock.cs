using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
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
				if (drawElementData.RuntimeString == NONE_STRING)
					syncDataText.text = string.Empty;
				else
					syncDataText.text = drawElementData.RuntimeString;
			}
			hider.gameObject.SetActive(drawElementData.IsShowing == false);
		}
	}
}