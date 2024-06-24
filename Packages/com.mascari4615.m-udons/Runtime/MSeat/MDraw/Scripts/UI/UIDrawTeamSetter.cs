using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamSetter : MBase
	{
		[SerializeField] private DrawManager drawManager;
		[SerializeField] private MScore mScore;
		[SerializeField] private TMP_Dropdown dropdown;

		public void SetPlayerTeamByDropdown()
		{
			if ((drawManager.DrawElementDatas.Length <= mScore.Score) || (0 > mScore.Score))
				return;

			drawManager.SetElementData(mScore.Score, (TeamType)(dropdown.value - 1), DrawRole.Normal, true);
			drawManager.SyncData();
		}
	}
}