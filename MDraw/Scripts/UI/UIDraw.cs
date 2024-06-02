
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDraw : MBase
	{
		private UIDrawTeamBlock[] teamBlocks;

		public void Init()
		{
			teamBlocks = GetComponentsInChildren<UIDrawTeamBlock>(true);

			for (int i = 0; i < teamBlocks.Length; i++)
				teamBlocks[i].Init((TeamType)i);
		}

		public void UpdateUI(DrawElementData[] drawElementDatas)
		{
			foreach (UIDrawTeamBlock teamBlock in teamBlocks)
				teamBlock.UpdateUI(drawElementDatas);
		}
	}
}