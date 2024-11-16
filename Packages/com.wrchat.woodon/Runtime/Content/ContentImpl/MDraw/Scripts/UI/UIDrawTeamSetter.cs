using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIDrawTeamSetter : MBase
	{
		[SerializeField] private DrawManager drawManager;
		[SerializeField] private MValue mValue;
		[SerializeField] private TMP_Dropdown dropdown;

		public void SetPlayerTeamByDropdown()
		{
			if ((drawManager.DrawElementDatas.Length <= mValue.Value) || (0 > mValue.Value))
				return;

			drawManager.SetElementData(mValue.Value, (TeamType)(dropdown.value - 1), DrawRole.Normal, true);
		}
	}
}