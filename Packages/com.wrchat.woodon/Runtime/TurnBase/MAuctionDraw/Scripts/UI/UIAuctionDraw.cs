using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDraw : MBase
	{
		[SerializeField] private Image[] teamLeaderImages;
		[SerializeField] private TextMeshProUGUI[] teamLeaderNameTexts;

		[SerializeField] private Image[] targetImages;
		[SerializeField] private TextMeshProUGUI[] targetNameTexts;

		[SerializeField] private TextMeshProUGUI[] stringDataTexts;
		[SerializeField] private TextMeshProUGUI[] maxPointTexts;
		[SerializeField] private TextMeshProUGUI[] resultTexts;

		[SerializeField] private Image[] spinImages;

		[SerializeField] private UIAuctionDrawTargetPanel[] targetPanels;

		[SerializeField] private MAnimator[] mAnimators;

		private AuctionDraw auctionDraw;

		public void Init(AuctionDraw auctionDraw)
		{
			this.auctionDraw = auctionDraw;

			foreach (UIAuctionDrawTargetPanel targetPanel in targetPanels)
				targetPanel.Init(auctionDraw);
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)}");

			foreach (UIAuctionDrawTargetPanel targetPanel in targetPanels)
				targetPanel.UpdateUI();

			foreach (MAnimator mAnimator in mAnimators)
				mAnimator.SetTrigger(auctionDraw.AuctionManager.CurGameState.ToString());

			if (auctionDraw.TargetIndex == NONE_INT)
			{
				MDebugLog($"{nameof(UpdateUI)}, TargetIndex is NONE_INT");
				return;
			}

			// Spin
			UpdateUI_Spin();
			
			// CurTarget
			UpdateUI_CurTarget();

			// Result
			UpdateUI_Result();
		}

		public void UpdateUI_Spin()
		{
			DrawElementData[] drawElementDatas = auctionDraw.DrawManager.DrawElementDatas;
			DrawElementData[] noTeamDatas = new DrawElementData[drawElementDatas.Length];

			int noTeamDataCount = 0;
			for (int i = 0; i < drawElementDatas.Length; i++)
			{
				DrawElementData drawElementData = drawElementDatas[i];
				if (drawElementData.InitTeamType == TeamType.None)
				{
					noTeamDatas[noTeamDataCount] = drawElementData;
					noTeamDataCount++;
				}
			}

			MDataUtil.ResizeArr(ref noTeamDatas, noTeamDataCount);

			for (int i = 0; i < spinImages.Length - 1; i++)
				spinImages[i].sprite = noTeamDatas[i % noTeamDatas.Length].Sprite;

			AuctionSeat maxTryPointSeat = auctionDraw.AuctionManager.MaxTryPointSeat;

			if (maxTryPointSeat != null)
				spinImages[spinImages.Length - 2].sprite = auctionDraw.DrawManager.DrawElementDatas[maxTryPointSeat.Index].Sprite;
		}

		public void UpdateUI_CurTarget()
		{
			MDebugLog(nameof(UpdateUI_CurTarget));

			DrawElementData targetData = auctionDraw.DrawManager.DrawElementDatas[auctionDraw.TargetIndex];

			foreach (Image targetImage in targetImages)
				targetImage.sprite = targetData.Sprite;
			foreach (TextMeshProUGUI targetNameText in targetNameTexts)
				targetNameText.text = targetData.Name;
			for (int i = 0; i < stringDataTexts.Length; i++)
				stringDataTexts[i].text = targetData.StringData[i];
		}

		public void UpdateUI_Result()
		{
			MDebugLog(nameof(UpdateUI_Result));

			AuctionSeat winner = auctionDraw.AuctionManager.MaxTryPointSeat;

			if (winner != null)
			{
				DrawElementData winnerData = auctionDraw.DrawManager.DrawElementDatas[winner.Index];

				foreach (Image image in teamLeaderImages)
				{
					image.sprite = winnerData.Sprite;
					image.color = Color.white;
				}

				foreach (TextMeshProUGUI text in teamLeaderNameTexts)
					text.text = winnerData.Name;

				foreach (TextMeshProUGUI resultText in resultTexts)
					resultText.text = $"<color=#FFC049>{winner.TryPoint}</color> 포인트로 '{winnerData.Name}' 팀에 낙찰";
			}
			else
			{
				foreach (Image image in teamLeaderImages)
					image.color = Color.clear;

				foreach (TextMeshProUGUI teamLeaderNameText in teamLeaderNameTexts)
					teamLeaderNameText.text = "-";

				foreach (TextMeshProUGUI resultText in resultTexts)
					resultText.text = $"유찰";
			}
		}
	}
}