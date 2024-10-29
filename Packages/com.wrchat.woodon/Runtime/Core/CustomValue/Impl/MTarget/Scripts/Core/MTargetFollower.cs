using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MTargetFollower : MBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MTarget mTarget;

		public virtual void SetMTarget(MTarget mTarget)
		{
			MDebugLog($"{nameof(SetMTarget)} - {mTarget}");
			this.mTarget = mTarget;
		}
	}
}