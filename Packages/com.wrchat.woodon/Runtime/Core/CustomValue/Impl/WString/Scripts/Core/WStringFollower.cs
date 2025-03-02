using UnityEngine;

namespace WRC.Woodon
{
	public abstract class WStringFollower : WBase
	{
		[Header("_" + nameof(WStringFollower))]
		[SerializeField] protected WString wString;

		public virtual void SetWString(WString wString)
		{
			WDebugLog($"{nameof(SetWString)} - {wString}");
			this.wString = wString;
		}
	}
}