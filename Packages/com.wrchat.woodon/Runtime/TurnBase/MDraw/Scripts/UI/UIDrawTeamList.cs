using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamList : MBase
	{
		[SerializeField] private UIDrawTeamBlock[] teamBlocks;

		public void Init()
		{
			if (teamBlocks == null || teamBlocks.Length == 0)
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