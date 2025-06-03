using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public enum BooleanOperatorType
	{
		AND,
		OR,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WBooleanOperator : WBase
	{
		[Header("_" + nameof(WBooleanOperator))]
		[SerializeField] private WBool[] wBools;
		[SerializeField] private BooleanOperatorType booleanOperatorType;
		[SerializeField] private WBool resultWBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			foreach (WBool wBool in wBools)
				wBool.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			bool result = wBools[0].Value;
			for (int i = 1; i < wBools.Length; i++)
			{
				switch (booleanOperatorType)
				{
					case BooleanOperatorType.AND:
						result &= wBools[i].Value;
						break;
					case BooleanOperatorType.OR:
						result |= wBools[i].Value;
						break;
				}
			}
			resultWBool.SetValue(result);
		}
	}
}