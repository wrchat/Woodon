using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MValueSetter : MValueFollower
	{
		[Header("_" + nameof(MValueSetter))]
		[SerializeField] private int value;

		[ContextMenu(nameof(SetValue))]
		public void SetValue()
		{
			if (mValue == null)
				return;

			mValue.SetValue(value);
		}

		[ContextMenu(nameof(AddValue))]
		public void AddValue()
		{
			if (mValue == null)
				return;

			mValue.AddValue(value);
		}

		[ContextMenu(nameof(SubValue))]
		public void SubValue()
		{
			if (mValue == null)
				return;

			mValue.SubValue(value);
		}
	}
}