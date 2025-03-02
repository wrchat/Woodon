using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MValueFollower : WBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MValue mValue;

		public virtual void SetMValue(MValue mValue)
		{
			WDebugLog($"{nameof(SetMValue)} - {mValue}");
			this.mValue = mValue;
		}
	}
}