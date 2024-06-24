using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawCurTarget : MUI
	{
		[SerializeField] private Image elementImage;
		[SerializeField] private TextMeshProUGUI elementText;
		[SerializeField] private TextMeshProUGUI[] stringDataTexts;

		private AuctionDraw auctionDraw;
		private CanvasGroup canvasGroup;

		public override void Init(MBase auctionDraw)
		{
			MDebugLog(nameof(Init));

			this.auctionDraw = (AuctionDraw)auctionDraw;
			canvasGroup = GetComponent<CanvasGroup>();

			elementImage.sprite = null;
			elementText.text = string.Empty;

			canvasGroup.alpha = 0;
		}

		public override void UpdateUI(MBase _)
		{
			MDebugLog(nameof(UpdateUI));

			if ((AuctionState)auctionDraw.AuctionManager.CurGameState == AuctionState.Wait ||
				(AuctionState)auctionDraw.AuctionManager.CurGameState == AuctionState.ApplyResult)
			{
				canvasGroup.alpha = 0;
				return;
			}

			if (auctionDraw.TargetIndex == NONE_INT)
			{
				MDebugLog($"{nameof(UpdateUI)}, TargetIndex is NONE_INT");
				canvasGroup.alpha = 0;
				return;
			}

			foreach (DrawElementData drawElementData in auctionDraw.DrawManager.DrawElementDatas)
			{
				if (drawElementData.Index == auctionDraw.TargetIndex)
				{
					elementImage.sprite = drawElementData.Sprite;
					elementText.text = drawElementData.Name;

					for (int i = 0; i < stringDataTexts.Length; i++)
						stringDataTexts[i].text = drawElementData.StringData[i];

					canvasGroup.alpha = 1;
					break;
				}
			}
		}
	}
}