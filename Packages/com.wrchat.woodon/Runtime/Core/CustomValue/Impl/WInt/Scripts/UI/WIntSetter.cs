using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WIntSetter : WIntFollower
	{
		[Header("_" + nameof(WIntSetter))]
		[SerializeField] private int value;

		[ContextMenu(nameof(SetValue))]
		public void SetValue()
		{
			if (wInt == null)
				return;

			wInt.SetValue(value);
		}

		[ContextMenu(nameof(AddValue))]
		public void AddValue()
		{
			if (wInt == null)
				return;

			wInt.AddValue(value);
		}

		[ContextMenu(nameof(SubValue))]
		public void SubValue()
		{
			if (wInt == null)
				return;

			wInt.SubValue(value);
		}
	}
}