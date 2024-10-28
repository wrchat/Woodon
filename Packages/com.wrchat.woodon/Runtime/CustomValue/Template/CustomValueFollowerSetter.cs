using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CustomValueFollowerSetter<T> : MBase
	{
		[Header("_" + nameof(CustomValueFollowerSetter<T>))]
		[SerializeField] private CustomValueFollower<T>[] followers;
		[SerializeField] private T[] values;
		[SerializeField] private MValue valueIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			valueIndex.SetMinMaxValue(0, values.Length - 1);
			valueIndex.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} - {valueIndex.Value}");

			int index = valueIndex.Value;
			T targetValue = values[index];

			foreach (CustomValueFollower<T> follower in followers)
				follower.SetCustomValue(targetValue);
		}
	}
}