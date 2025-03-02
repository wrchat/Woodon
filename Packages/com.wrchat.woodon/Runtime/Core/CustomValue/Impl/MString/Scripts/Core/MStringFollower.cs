using UnityEngine;

namespace WRC.Woodon
{
	public abstract class MStringFollower : WBase
	{
		[Header("_" + nameof(MStringFollower))]
		[SerializeField] protected MString mString;

		public virtual void SetMString(MString mString)
		{
			MDebugLog($"{nameof(SetMString)} - {mString}");
			this.mString = mString;
		}
	}
}