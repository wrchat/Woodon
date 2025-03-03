using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WStringFollowerSetter : WBase
	{
		[Header("_" + nameof(WStringFollowerSetter))]
		[SerializeField] private WStringFollower[] wStringFollowers;
		[SerializeField] private WString[] wStrings;
		[SerializeField] private WInt wStringIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			wStringIndex.SetMinMaxValue(0, wStrings.Length - 1);
			wStringIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)} - {wStringIndex.Value}");

			int index = wStringIndex.Value;
			WString wString = wStrings[index];

			foreach (WStringFollower wStringFollower in wStringFollowers)
				wStringFollower.SetWString(wString);
		}
	}
}