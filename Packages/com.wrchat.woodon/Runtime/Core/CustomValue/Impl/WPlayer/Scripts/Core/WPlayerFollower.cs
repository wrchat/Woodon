using UnityEngine;

namespace WRC.Woodon
{
	public abstract class WPlayerFollower : WBase
	{
		[Header("_" + nameof(WPlayerFollower))]
		[SerializeField] protected WPlayer wPlayer;

		public virtual void SetWPlayer(WPlayer wPlayer)
		{
			WDebugLog($"{nameof(SetWPlayer)} - {wPlayer}");
			this.wPlayer = wPlayer;
		}
	}
}