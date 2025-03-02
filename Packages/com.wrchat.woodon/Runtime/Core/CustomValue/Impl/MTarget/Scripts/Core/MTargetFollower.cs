using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MTargetFollower : WBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MTarget mTarget;

		public virtual void SetMTarget(MTarget mTarget)
		{
			WDebugLog($"{nameof(SetMTarget)} - {mTarget}");
			this.mTarget = mTarget;
		}
	}
}