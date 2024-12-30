using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	[DefaultExecutionOrder(0)]
	public class SendEventOnStationEvent : MBase
	{
		[Header("_" + nameof(SendEventOnStationEvent))]
		[SerializeField] private UdonSharpBehaviour[] enterListeners;
		[SerializeField] private UdonSharpBehaviour[] exitListeners;

		[SerializeField] private bool[] sendEnterCallbackGlobal;
		[SerializeField] private bool[] sendExitCallbackGlobal;

		[SerializeField] private string[] enterCallbacks;
		[SerializeField] private string[] exitCallbacks;

		[SerializeField] private bool onlyIfLocalPlayer = true;

		public override void OnStationEntered(VRCPlayerApi player)
		{
			MDebugLog($"{nameof(OnStationEntered)}");

			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			for (int i = 0; i < enterListeners.Length; i++)
			{
				if (sendEnterCallbackGlobal[i])
					enterListeners[i].SendCustomNetworkEvent(NetworkEventTarget.All, enterCallbacks[i]);
				else
					enterListeners[i].SendCustomEvent(enterCallbacks[i]);
			}
		}

		public override void OnStationExited(VRCPlayerApi player)
		{
			MDebugLog($"{nameof(OnStationExited)}");

			if (onlyIfLocalPlayer && (player != Networking.LocalPlayer))
				return;

			for (int i = 0; i < exitListeners.Length; i++)
			{
				if (sendExitCallbackGlobal[i])
					exitListeners[i].SendCustomNetworkEvent(NetworkEventTarget.All, exitCallbacks[i]);
				else
					exitListeners[i].SendCustomEvent(exitCallbacks[i]);
			}
		}
	}
}