using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MValueFollower : MBase
	{
		[Header("_" + nameof(MTargetFollower))]
		[SerializeField] protected MValue mValue;

		public abstract void SetMValue(MValue mValue);
	}
}