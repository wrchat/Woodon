using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMTargetSetter : MBase
	{
		[Header("_" + nameof(UIMTargetSetter))]
		[SerializeField] private UIMTarget[] mTargetUIs;
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

			foreach (UIMTarget mTargetUI in mTargetUIs)
				mTargetUI.SetMTarget(mTarget);
		}
	}
}