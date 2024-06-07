using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MMikeManager : MBase
	{
		// [SerializeField] private MMike[] mMikes;
		// [SerializeField] private int gain = 10;
		// [SerializeField] private int far = 300;
		// private VRCPlayerApi[] playerApis;
		//
		// private void Start()
		// {
		//     UpdateAmplificationLoop();
		// }
		//
		// public void UpdateAmplificationLoop()
		// {
		//     SendCustomEventDelayedSeconds(nameof(UpdateAmplificationLoop), .5f);
		//     UpdateAmplification();
		// }
		//
		// public void UpdateAmplification()
		// {
		//     MDebugLog(nameof(UpdateAmplification));
		//     if (playerApis == null)
		//     {
		//         playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
		//         VRCPlayerApi.GetPlayers(playerApis);
		//     }
		//
		//     foreach (var playerApi in playerApis)
		//         SetVoice(playerApi, false);
		//
		//     foreach (var mMike in mMikes)
		//     {
		//         var mikeOwner = Networking.GetOwner(mMike.Pickup.gameObject);
		//         if (mikeOwner.playerId == mMike.OwnerId)
		//             SetVoice(mikeOwner, true);
		//     }
		// }
		//
		// public void SetMikeOwner(GameObject mike)
		// {
		//     MDebugLog(nameof(SetMikeOwner));
		//     foreach (var mMike in mMikes)
		//         if (mMike.gameObject == mike)
		//         {
		//             MDebugLog("A");
		//             mMike.SetOwner();
		//         }
		//         else if (mMike.OwnerId == Networking.LocalPlayer.playerId)
		//         {
		//             MDebugLog("B");
		//             mMike.ResetOwner();
		//         }
		// }
		//
		// public override void OnPlayerJoined(VRCPlayerApi player)
		// {
		//     playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
		//     VRCPlayerApi.GetPlayers(playerApis);
		// }
		//
		// public override void OnPlayerLeft(VRCPlayerApi player)
		// {
		//     playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
		//     VRCPlayerApi.GetPlayers(playerApis);
		// }
	}
}