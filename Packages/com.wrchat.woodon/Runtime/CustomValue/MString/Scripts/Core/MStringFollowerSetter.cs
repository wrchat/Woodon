using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MStringFollowerSetter : MBase
	{
		[Header("_" + nameof(MStringFollowerSetter))]
		[SerializeField] private MStringFollower[] mStringFollowers;
		[SerializeField] private MString[] mStrings;
		[SerializeField] private MValue mStringIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mStringIndex.SetMinMaxValue(0, mStrings.Length - 1);
			mStringIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} - {mStringIndex.Value}");

			int index = mStringIndex.Value;
			MString mString = mStrings[index];

			foreach (MStringFollower mStringFollower in mStringFollowers)
				mStringFollower.SetMString(mString);
		}
	}
}