using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamSetter : WBase
	{
		[SerializeField] private DrawManager drawManager;
		[SerializeField] private WInt wInt;
		[SerializeField] private TMP_Dropdown dropdown;

		public void SetPlayerTeamByDropdown()
		{
			if ((drawManager.DrawElementDatas.Length <= wInt.Value) || (0 > wInt.Value))
				return;

			drawManager.SetElementData(wInt.Value, (TeamType)(dropdown.value - 1), DrawRole.Normal, true);
		}
	}
}