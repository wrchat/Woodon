using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnOwnershipTransferred : WEventPublisher
	{
		public override void OnOwnershipTransferred(VRCPlayerApi player)
		{
			SendEvents();
		}
	}
}