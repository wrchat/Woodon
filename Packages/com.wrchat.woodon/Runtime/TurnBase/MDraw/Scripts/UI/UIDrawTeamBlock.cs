using UdonSharp;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamBlock : MBase
	{
		private TeamType teamType = TeamType.None;
		private UIDrawElementBlock[] elementBlocks;
		private LayoutGroup layoutGroup;

		public void Init(TeamType teamType)
		{
			this.teamType = teamType;
			elementBlocks = GetComponentsInChildren<UIDrawElementBlock>(true);
			
			layoutGroup = GetComponentInChildren<LayoutGroup>(true);
			layoutGroup.enabled = false;
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

				if (elementIndex >= elementBlocks.Length)
					break;
			}
		}
	}
}
