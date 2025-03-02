using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WBoolFollowerSetter : WBase
	{
		[Header("_" + nameof(WBoolFollowerSetter))]
		[SerializeField] private WBoolFollower[] wBoolFollowers;
		[SerializeField] private WBool[] wBools;
		[SerializeField] private MValue wBoolIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			wBoolIndex.SetMinMaxValue(0, wBools.Length - 1);
			wBoolIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} : {wBoolIndex.Value}");

			int index = wBoolIndex.Value;
			WBool wBool = wBools[index];

			foreach (WBoolFollower wBoolFollower in wBoolFollowers)
				wBoolFollower.SetWBool(wBool);
		}
	}
}