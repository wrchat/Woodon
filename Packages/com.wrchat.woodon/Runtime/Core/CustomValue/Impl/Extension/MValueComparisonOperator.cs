using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace WRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MValueComparisonOperator : MBase
	{
		[Header("_" + nameof(MValueComparisonOperator))]
		[SerializeField] private MValue mValue1;
		[SerializeField] private MValue mValue2;
		[SerializeField] private int value;
		[SerializeField] private ComparisonOperatorType comparisonOperatorType;

		[SerializeField] private MBool resultMBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			mValue1.RegisterListener(this, nameof(UpdateValue));

			if (mValue2 != null)
				mValue2.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			if (mValue2 == null)
			{
				bool result = Compare(mValue1.Value, value);
				resultMBool.SetValue(result);
			}
			else
			{
				bool result = Compare(mValue1.Value, mValue2.Value);
				resultMBool.SetValue(result);
			}
		}

		private bool Compare(int left, int right)
		{
			switch (comparisonOperatorType)
			{
				case ComparisonOperatorType.EQUAL:
					return left == right;
				case ComparisonOperatorType.NOT_EQUAL:
					return left != right;
				case ComparisonOperatorType.GREATER_THAN:
					return left > right;
				case ComparisonOperatorType.GREATER_THAN_OR_EQUAL:
					return left >= right;
				case ComparisonOperatorType.LESS_THAN:
					return left < right;
				case ComparisonOperatorType.LESS_THAN_OR_EQUAL:
					return left <= right;
				default:
					return false;
			}
		}
	}
}