using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.LIL.LilpaTactical
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DropPickup : MBase
	{
		[SerializeField] private MTarget mTarget;
		[SerializeField] private VRC_Pickup[] pickups;

		public void Drop_Global()
		{
			if (DEBUG)
				Debug.Log(nameof(Drop_Global));

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Drop));
		}

		public void Drop()
		{
			if (DEBUG)
				Debug.Log(nameof(Drop));

			if (mTarget.IsTargetPlayer())
				return;

			foreach (VRC_Pickup pickup in pickups)
				if (IsOwner(pickup.gameObject))
					pickup.Drop();
		}
	}
}