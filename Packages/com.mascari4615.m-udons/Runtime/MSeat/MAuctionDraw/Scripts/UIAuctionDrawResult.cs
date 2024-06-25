using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawResult : MUI
	{
		[SerializeField] private Image[] teamLeaderImages;
		[SerializeField] private TextMeshProUGUI[] teamLeaderTexts;

		[SerializeField] private Image[] targetImages;
		[SerializeField] private TextMeshProUGUI[] targetTexts;

		[SerializeField] private TextMeshProUGUI[] pointTexts;

		private AuctionDraw auctionDraw;
		private CanvasGroup canvasGroup;

		public override void Init(MBase mBase = null)
		{
			auctionDraw = (AuctionDraw)mBase;

			canvasGroup = GetComponent<CanvasGroup>();
		}

		public override void UpdateUI(MBase mBase = null)
		{
			auctionDraw = (AuctionDraw)mBase;

			canvasGroup.alpha = auctionDraw.AuctionManager.CurGameState == (int)AuctionState.CheckResult ? 1 : 0;

			if (auctionDraw.AuctionManager.CurGameState != (int)AuctionState.CheckResult)
				return;

			if (auctionDraw.TargetIndex == NONE_INT)
			{
				MDebugLog($"{nameof(UpdateUI)}, TargetIndex is NONE_INT");
				return;
			}

			DrawElementData targetData = auctionDraw.DrawManager.DrawElementDatas[auctionDraw.TargetIndex];
		
			foreach (Image image in targetImages)
				image.sprite = targetData.Sprite;

			foreach (TextMeshProUGUI text in targetTexts)
				text.text = targetData.Name;

			int maxTryPoint = auctionDraw.AuctionManager.GetMaxTurnData();
			if (maxTryPoint == 0)
			{
				foreach (Image image in teamLeaderImages)
				{
					image.sprite = null;
					image.color = Color.clear;
				}

				foreach (TextMeshProUGUI text in teamLeaderTexts)
					text.text = "-";

				foreach (Image image in targetImages)
					image.sprite = targetData.Sprite;

				foreach (TextMeshProUGUI text in targetTexts)
					text.text = targetData.Name;

				foreach (TextMeshProUGUI text in pointTexts)
					text.text = $"유찰";
			}
			else
			{
				AuctionSeat winner = null;
				foreach (AuctionSeat auctionSeat in auctionDraw.AuctionManager.TurnSeats)
				{
					if (auctionSeat.TryPoint == maxTryPoint)
					{
						winner = auctionSeat;
						break;
					}
				}

				DrawElementData winnerData = auctionDraw.DrawManager.DrawElementDatas[winner.Index];

				foreach (Image image in teamLeaderImages)
				{
					image.sprite = winnerData.Sprite;
					image.color = Color.white;
				}

				foreach (TextMeshProUGUI text in teamLeaderTexts)
					text.text = winnerData.Name;

				foreach (TextMeshProUGUI text in pointTexts)
					text.text = $"<color=#FFC049>{maxTryPoint}</color> 포인트로 '{winnerData.Name}' 팀에 낙찰";
			}
		}
	}
}