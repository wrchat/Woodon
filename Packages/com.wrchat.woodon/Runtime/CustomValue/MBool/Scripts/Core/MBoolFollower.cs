using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MBoolFollower : MBase
	{
		[Header("_" + nameof(MBoolFollower))]
		[SerializeField] protected MBool mBool;

		public abstract void SetMBool(MBool mBool);
	}
}