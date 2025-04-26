using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WStringList : WBase
	{
		[Header("_" + nameof(WStringList))]
		[SerializeField] private string[] strings;
		[SerializeField] private WString[] targetStrings;
		[SerializeField] private WInt index;

		private void Start()
		{
			index.SetMinMaxValue(0, strings.Length - 1, recalcValue: false);
			index.RegisterListener(this, nameof(OnIndexChanged));

			OnIndexChanged();
		}

		// strings[index.Value] 값으로 targetStrings의 값을 설정합니다. - KarmoDDrine 250427
		public void OnIndexChanged()
		{
			foreach (WString targetString in targetStrings)
			{
				targetString.SetValue(strings[index.Value]);
			}
		}
	}
}