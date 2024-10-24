using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MTargetFollower : MBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MTarget mTarget;

		public abstract void SetMTarget(MTarget mTarget);
	}
}