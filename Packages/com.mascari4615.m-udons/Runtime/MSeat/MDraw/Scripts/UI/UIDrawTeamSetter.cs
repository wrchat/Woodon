using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamSetter : MBase
	{
		[SerializeField] private DrawManager drawManager;
		[SerializeField] private MValue mScore;
		[SerializeField] private TMP_Dropdown dropdown;

		public void SetPlayerTeamByDropdown()
		{
			if ((drawManager.DrawElementDatas.Length <= mScore.Value) || (0 > mScore.Value))
				return;

			drawManager.SetElementData(mScore.Value, (TeamType)(dropdown.value - 1), DrawRole.Normal, true);
			drawManager.SyncData();
		}
	}
}