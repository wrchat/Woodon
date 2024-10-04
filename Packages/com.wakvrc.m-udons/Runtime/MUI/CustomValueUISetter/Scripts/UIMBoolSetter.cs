using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMBoolSetter : MBase
	{
		[Header("_" + nameof(UIMBoolSetter))]
		[SerializeField] private UIMBool[] mBoolUIs;
		[SerializeField] private MBool[] mBools;
		[SerializeField] private MValue mBoolIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mBoolIndex.SetMinMaxValue(0, mBools.Length - 1);
			mBoolIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} - {mBoolIndex.Value}");

			int index = mBoolIndex.Value;
			MBool mBool = mBools[index];

			foreach (UIMBool mBoolUI in mBoolUIs)
				mBoolUI.SetMBool(mBool);
		}
	}
}