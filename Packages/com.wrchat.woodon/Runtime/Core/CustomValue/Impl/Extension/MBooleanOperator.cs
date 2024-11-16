using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace WRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MBooleanOperator : MBase
	{
		[Header("_" + nameof(MBooleanOperator))]
		[SerializeField] private MBool[] mBools;
		[SerializeField] private BooleanOperatorType booleanOperatorType;
		[SerializeField] private MBool resultMBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			foreach (MBool mBool in mBools)
				mBool.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			bool result = mBools[0].Value;
			for (int i = 1; i < mBools.Length; i++)
			{
				switch (booleanOperatorType)
				{
					case BooleanOperatorType.AND:
						result &= mBools[i].Value;
						break;
					case BooleanOperatorType.OR:
						result |= mBools[i].Value;
						break;
				}
			}
			resultMBool.SetValue(result);
		}
	}
}