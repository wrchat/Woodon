using UnityEngine;

namespace WRC.Woodon
{
	public abstract class WIntFollower : WBase
	{
		[Header("_" + nameof(WPlayerFollower))]
		[SerializeField] protected WInt wInt;

		public virtual void SetWInt(WInt wInt)
		{
			WDebugLog($"{nameof(SetWInt)} - {wInt}");
			this.wInt = wInt;
		}
	}
}