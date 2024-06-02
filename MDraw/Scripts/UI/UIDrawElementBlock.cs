
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawElementBlock : MBase
	{
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Image spriteImage;

		[SerializeField] private Image hider;

		public void UpdateUI(DrawElementData drawElementData)
		{
			nameText.text = drawElementData.Name;
			spriteImage.sprite = drawElementData.Sprite;

			hider.gameObject.SetActive(drawElementData.IsShowing == false);
		}
	}
}