using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIAuctionDrawTargetPanel : MUI
	{
		private AuctionDraw auctionDraw;
		private UIAuctionDrawTargetElement[] elements;

		public override void Init(MBase auctionDraw = null)
		{
			MDebugLog($"{nameof(Init)}({auctionDraw})");
			
			this.auctionDraw = (AuctionDraw)auctionDraw;

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
				element.Init(this);

			UpdateUI(this);
		}

		public override void UpdateUI(MBase _ = null)
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