using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnPlayerExit : WEventPublisher
	{
		[Header("_" + nameof(SendEventOnPlayerExit))]
		[SerializeField] private bool onlyIfLocalPlayer = true;

		public override void OnPlayerTriggerExit(VRCPlayerApi player)
		{
			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			SendEvents();
		}
	}
}