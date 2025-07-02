using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public enum StringOperatorType
	{
		Add,
		Remove,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WStringOperator : WBase
	{
		[Header("_" + nameof(WStringOperator))]
		[SerializeField] private WString wString1;
		[SerializeField] private WString wString2;
		[SerializeField] private StringOperatorType stringOperatorType;
		[SerializeField] private WString resultWString;
		[SerializeField] private bool autoUpdate = false;

		private void Start()
		{
			Init();
			if (autoUpdate == true)
				UpdateValue();
		}

		private void Init()
		{
			// wString1.RegisterListener(this, nameof(UpdateValue));
			// wString2.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			string result = string.Empty;

			string string1Value = wString1.Value;
			string string2Value = wString2.Value;
			switch (stringOperatorType)
			{
				case StringOperatorType.Add:
					result = string1Value + string2Value;
					break;
				case StringOperatorType.Remove:
					result = string1Value.Replace(string2Value, string.Empty);
					break;
			}

			resultWString.SetValue(result);
		}
	}
}