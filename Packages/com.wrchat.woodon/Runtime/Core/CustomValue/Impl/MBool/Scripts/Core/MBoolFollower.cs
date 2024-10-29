using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MBoolFollower : MBase
	{
		[Header("_" + nameof(MBoolFollower))]
		[SerializeField] protected MBool mBool;

		public virtual void SetMBool(MBool mBool)
		{
			MDebugLog($"{nameof(SetMBool)} - {mBool}");
			this.mBool = mBool;
		}
	}
}