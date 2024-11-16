using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawTargetElement : MBase
	{
		[SerializeField] private Image elementImage;
		[SerializeField] private TextMeshProUGUI elementText;
		[SerializeField] private TextMeshProUGUI[] indexTexts;
		[SerializeField] private TextMeshProUGUI[] pointTexts;
		[SerializeField] private Image hasTeamImage;

		private AuctionDraw auctionDraw;
		private UIAuctionDrawTargetPanel panel;
		private DrawElementData drawElementData;

		public void Init(AuctionDraw auctionDraw, UIAuctionDrawTargetPanel panel)
		{
			this.auctionDraw = auctionDraw;
			this.panel = panel;
		}

		public void UpdateUI(DrawElementData drawElementData)
		{
			this.drawElementData = drawElementData;

			elementImage.sprite = drawElementData.Sprite;
			elementText.text = drawElementData.Name;
			foreach (TextMeshProUGUI indexText in indexTexts)
				indexText.text = drawElementData.Index.ToString();
			hasTeamImage.gameObject.SetActive(drawElementData.TeamType != TeamType.None);
			foreach (TextMeshProUGUI pointText in pointTexts)
			{
				if (drawElementData.RuntimeString == NONE_STRING)
					pointText.text = string.Empty;
				else
					pointText.text = drawElementData.RuntimeString;
			}
		}

		public void SelectTarget()
		{
			auctionDraw.SetTargetIndex(drawElementData.Index);
		}
	}
}