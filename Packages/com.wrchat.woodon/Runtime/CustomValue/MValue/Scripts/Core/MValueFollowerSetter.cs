using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MValueFollowerSetter : MBase
	{
		[Header("_" + nameof(MValueFollowerSetter))]
		[SerializeField] private MValueFollower[] mValueFollowers;
		[SerializeField] private MValue[] mValues;
		[SerializeField] private MValue mValueIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mValueIndex.SetMinMaxValue(0, mValues.Length - 1);
			mValueIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} - {mValueIndex.Value}");

			int index = mValueIndex.Value;
			MValue mValue = mValues[index];

			foreach (MValueFollower mValueFollower in mValueFollowers)
				mValueFollower.SetMValue(mValue);
		}
	}
}