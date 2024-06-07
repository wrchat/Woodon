using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DropPickup : MBase
	{
		[SerializeField] private MPlayerSync mPlayerSync;
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

			if (mPlayerSync.PlayerID != Networking.LocalPlayer.playerId)
				return;

			foreach (var pickup in pickups)
				if (IsOwner(pickup.gameObject))
					pickup.Drop();
		}
	}
}