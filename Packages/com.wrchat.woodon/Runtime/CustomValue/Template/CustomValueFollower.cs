using UnityEngine;

namespace WRC.Woodon
{
	public abstract class CustomValueFollower<T> : MBase
	{
		[Header("_" + nameof(CustomValueFollower<T>))]
		[SerializeField] protected T customValue;

		public virtual void SetCustomValue(T customValue)
		{
			// MDebugLog($"{nameof(SetCustomValue)} - {customValue}");
			this.customValue = customValue;
		}
	}
}