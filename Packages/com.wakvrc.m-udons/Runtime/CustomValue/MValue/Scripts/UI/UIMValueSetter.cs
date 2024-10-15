using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMValueSetter : MBase
	{
		[Header("_" + nameof(UIMValueSetter))]
		[SerializeField] private UIMValue[] mValueUIs;
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

			foreach (UIMValue mValueUI in mValueUIs)
				mValueUI.SetMValue(mValue);
		}
	}
}