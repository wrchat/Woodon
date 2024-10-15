using UnityEngine;

namespace WakVRC
{
	public abstract class MTargetFollower : MBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MTarget mTarget;

		public abstract void SetMTarget(MTarget mTarget);
	}
}