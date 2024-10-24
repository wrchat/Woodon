using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MBoolFollowerSetter : MBase
	{
		[Header("_" + nameof(MBoolFollowerSetter))]
		[SerializeField] private MBoolFollower[] mBoolFollowers;
		[SerializeField] private MBool[] mBools;
		[SerializeField] private MValue mBoolIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			mBoolIndex.SetMinMaxValue(0, mBools.Length - 1);
			mBoolIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} : {mBoolIndex.Value}");

			int index = mBoolIndex.Value;
			MBool mBool = mBools[index];

			foreach (MBoolFollower mBoolFollower in mBoolFollowers)
				mBoolFollower.SetMBool(mBool);
		}
	}
}