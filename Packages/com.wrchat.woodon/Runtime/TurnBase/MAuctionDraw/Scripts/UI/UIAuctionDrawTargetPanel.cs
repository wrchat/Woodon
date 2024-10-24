using UdonSharp;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawTargetPanel : MBase
	{
		private AuctionDraw auctionDraw;
		private UIAuctionDrawTargetElement[] elements;

		public void Init(AuctionDraw auctionDraw)
		{
			MDebugLog($"{nameof(Init)}({auctionDraw})");
			
			this.auctionDraw = auctionDraw;

			elements = GetComponentsInChildren<UIAuctionDrawTargetElement>(true);

			foreach (UIAuctionDrawTargetElement element in elements)
				element.gameObject.SetActive(false);

			int uiIndex = 0;
			for (int i = 0; i < this.auctionDraw.DrawManager.DrawElementDatas.Length; i++)
			{
				DrawElementData targetData = this.auctionDraw.DrawManager.DrawElementDatas[i];

				if (i < elements.Length && targetData.InitTeamType == TeamType.None)
				{
					elements[uiIndex].gameObject.SetActive(true);
					uiIndex++;
				}
			}

			foreach (UIAuctionDrawTargetElement element in elements)
				element.Init(this.auctionDraw, this);

			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog(nameof(UpdateUI));
		
			int uiIndex = 0;
			for (int i = 0; i < auctionDraw.DrawManager.DrawElementDatas.Length; i++)
			{
				DrawElementData targetData = auctionDraw.DrawManager.DrawElementDatas[i];

				if (i < elements.Length && targetData.InitTeamType == TeamType.None)
				{
					elements[uiIndex].UpdateUI(auctionDraw.DrawManager.DrawElementDatas[i]);
					uiIndex++;
				}
			}
		}
	}
}