using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public enum StringComparisonOperatorType
	{
		Equal,
		NotEqual,
		SameLength,
		NotSameLength,
		ShorterThan,
		LongerThan,
		Contains,
		NotContains,
		StartsWith,
		NotStartsWith,
		EndsWith,
		NotEndsWith,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WStringComparisonOperator : WBase
	{
		[Header("_" + nameof(WStringComparisonOperator))]
		[SerializeField] private WString wString1;
		[SerializeField] private WString wString2;
		[SerializeField] private StringComparisonOperatorType stringComparisonOperatorType;
		[SerializeField] private WBool resultWBool;

		[SerializeField] private bool trimStrings = true;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			wString1.RegisterListener(this, nameof(UpdateValue));
			wString2.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			bool result = false;

			string string1Value = wString1.Value;
			string string2Value = wString2.Value;

			if (trimStrings)
			{
				string1Value = string1Value.Trim();
				string2Value = string2Value.Trim();
			}

			switch (stringComparisonOperatorType)
			{
				case StringComparisonOperatorType.Equal:
					result = string1Value == string2Value;
					break;
				case StringComparisonOperatorType.NotEqual:
					result = string1Value != string2Value;
					break;
				case StringComparisonOperatorType.SameLength:
					result = string1Value.Length == string2Value.Length;
					break;
				case StringComparisonOperatorType.NotSameLength:
					result = string1Value.Length != string2Value.Length;
					break;
				case StringComparisonOperatorType.ShorterThan:
					result = string1Value.Length < string2Value.Length;
					break;
				case StringComparisonOperatorType.LongerThan:
					result = string1Value.Length > string2Value.Length;
					break;
				case StringComparisonOperatorType.Contains:
					result = string1Value.Contains(string2Value);
					break;
				case StringComparisonOperatorType.NotContains:
					result = !string1Value.Contains(string2Value);
					break;
				case StringComparisonOperatorType.StartsWith:
					result = string1Value.StartsWith(string2Value);
					break;
				case StringComparisonOperatorType.NotStartsWith:
					result = !string1Value.StartsWith(string2Value);
					break;
				case StringComparisonOperatorType.EndsWith:
					result = string1Value.EndsWith(string2Value);
					break;
				case StringComparisonOperatorType.NotEndsWith:
					result = !string1Value.EndsWith(string2Value);
					break;
			}
			resultWBool.SetValue(result);
		}
	}
}