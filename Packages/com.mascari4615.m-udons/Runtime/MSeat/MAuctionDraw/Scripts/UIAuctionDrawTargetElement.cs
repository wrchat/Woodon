using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawTargetElement : MUI
	{
		[SerializeField] private Image elementImage;
		[SerializeField] private TextMeshProUGUI elementText;
		[SerializeField] private Image hasTeamImage;

		private UIAuctionDrawTargetPanel panel;

		public override void Init(MBase panel = null)
		{
			this.panel = (UIAuctionDrawTargetPanel)panel;

		}
		public override void UpdateUI(MBase data = null)
		{
			DrawElementData drawElementData = (DrawElementData)data;

			elementImage.sprite = drawElementData.Sprite;
			elementText.text = drawElementData.Name;
			hasTeamImage.gameObject.SetActive(drawElementData.TeamType != TeamType.None);
		}
	}
}