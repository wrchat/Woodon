using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace WRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WIntComparisonOperator : WBase
	{
		[Header("_" + nameof(WIntComparisonOperator))]
		[SerializeField] private WInt mInt1;
		[SerializeField] private WInt mInt2;
		[SerializeField] private int value;
		[SerializeField] private ComparisonOperatorType comparisonOperatorType;

		[SerializeField] private WBool resultWBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			mInt1.RegisterListener(this, nameof(UpdateValue));

			if (mInt2 != null)
				mInt2.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			if (mInt2 == null)
			{
				bool result = Compare(mInt1.Value, value);
				resultWBool.SetValue(result);
			}
			else
			{
				bool result = Compare(mInt1.Value, mInt2.Value);
				resultWBool.SetValue(result);
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