using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnStationEnter : MBase
	{
		[Header("_" + nameof(SendEventOnStationEnter))]
		[SerializeField] private UdonSharpBehaviour[] enterUdons;
		[SerializeField] private UdonSharpBehaviour[] exitUdons;

		[SerializeField] private bool[] isEnterEventGlobal;
		[SerializeField] private bool[] isExitEventGlobal;

		[SerializeField] private string[] enterEvents;
		[SerializeField] private string[] exitEvents;

		[SerializeField] private bool onlyIfLocalPlayer = true;

		public override void OnStationEntered(VRCPlayerApi player)
		{
			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			for (int i = 0; i < enterUdons.Length; i++)
			{
				if (isEnterEventGlobal[i])
					enterUdons[i].SendCustomNetworkEvent(NetworkEventTarget.All, enterEvents[i]);
				else
					enterUdons[i].SendCustomEvent(enterEvents[i]);
			}
		}

		public override void OnStationExited(VRCPlayerApi player)
		{
			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			for (int i = 0; i < exitUdons.Length; i++)
			{
				if (isExitEventGlobal[i])
					exitUdons[i].SendCustomNetworkEvent(NetworkEventTarget.All, exitEvents[i]);
				else
					exitUdons[i].SendCustomEvent(exitEvents[i]);
			}
		}
	}
}