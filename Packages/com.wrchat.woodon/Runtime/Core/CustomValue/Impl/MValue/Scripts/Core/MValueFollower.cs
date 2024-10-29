using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MValueFollower : MBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MValue mValue;

		public virtual void SetMValue(MValue mValue)
		{
			MDebugLog($"{nameof(SetMValue)} - {mValue}");
			this.mValue = mValue;
		}
	}
}