
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamBlock : MBase
	{
		private TeamType teamType = TeamType.None;
		private UIDrawElementBlock[] elementBlocks;

		public void Init(TeamType teamType)
		{
			this.teamType = teamType;
			elementBlocks = GetComponentsInChildren<UIDrawElementBlock>(true);
		}

		public void UpdateUI(DrawElementData[] drawElementDatas)
		{
			foreach (UIDrawElementBlock elementBlock in elementBlocks)
				elementBlock.gameObject.SetActive(false);

			int elementIndex = 0;
			foreach (DrawElementData drawElementData in drawElementDatas)
			{
				if (drawElementData == null)
					continue;

				if (drawElementData.TeamType != teamType)
					continue;

				UIDrawElementBlock elementBlock = elementBlocks[elementIndex];
				elementBlock.UpdateUI(drawElementData);
				elementBlock.gameObject.SetActive(true);

				elementIndex++;
			}
		}
	}
}
