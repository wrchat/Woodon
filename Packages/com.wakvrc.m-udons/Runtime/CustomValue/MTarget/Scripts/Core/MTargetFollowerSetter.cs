using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MTargetFollowerSetter : MBase
	{
		[Header("_" + nameof(MTargetFollowerSetter))]
		[SerializeField] private MTargetFollower[] mTargetFollowers;
		[SerializeField] private MTarget[] mTargets;
		[SerializeField] private MValue mTargetIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mTargetIndex.SetMinMaxValue(0, mTargets.Length - 1);
			mTargetIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} - {mTargetIndex.Value}");

			int index = mTargetIndex.Value;
			MTarget mTarget = mTargets[index];

			foreach (MTargetFollower mTargetFollower in mTargetFollowers)
				mTargetFollower.SetMTarget(mTarget);
		}
	}
}