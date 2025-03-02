using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WIntFollowerSetter : WBase
	{
		[Header("_" + nameof(WIntFollowerSetter))]
		[SerializeField] private WIntFollower[] wIntFollowers;
		[SerializeField] private WInt[] wInts;
		[SerializeField] private WInt wIntIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			wIntIndex.SetMinMaxValue(0, wInts.Length - 1);
			wIntIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)} - {wIntIndex.Value}");

			int index = wIntIndex.Value;
			WInt wInt = wInts[index];

			foreach (WIntFollower wIntFollower in wIntFollowers)
				wIntFollower.SetWInt(wInt);
		}
	}
}