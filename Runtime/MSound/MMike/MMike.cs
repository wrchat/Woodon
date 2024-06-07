using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MMike : MBase
	{
		// [SerializeField] private MMikeManager _mMikeManager;
		// [SerializeField] private VRC_Pickup _pickup;
		// public VRC_Pickup Pickup => _pickup;
		// [field: UdonSynced()] public int OwnerId { get; private set; } = NONE_INT;
		//
		// private void Update()
		// {
		//     if (Networking.IsOwner(_pickup.gameObject))
		//     {
		//         if (LocalPlayerHolding(_pickup))
		//         {
		//             if (OwnerId != Networking.LocalPlayer.playerId) _mMikeManager.SetMikeOwner(gameObject);
		//         }
		//         else
		//         {
		//             if (OwnerId == Networking.LocalPlayer.playerId) ResetOwner();
		//         }
		//     }
		// }
		//
		// public void SetOwner()
		// {
		//     if (!Networking.IsOwner(gameObject))
		//         Networking.SetOwner(Networking.LocalPlayer, gameObject);
		//     OwnerId = Networking.LocalPlayer.playerId;
		//     RequestSerialization();
		// }
		//
		// public void ResetOwner()
		// {
		//     if (!Networking.IsOwner(gameObject))
		//         Networking.SetOwner(Networking.LocalPlayer, gameObject);
		//     OwnerId = NONE_INT;
		//     RequestSerialization();
		// }
	}
}