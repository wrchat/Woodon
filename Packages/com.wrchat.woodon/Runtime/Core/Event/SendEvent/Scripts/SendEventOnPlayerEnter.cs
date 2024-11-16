using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnPlayerEnter : WEventPublisher
	{
		[Header("_" + nameof(SendEventOnPlayerExit))]
		[SerializeField] private bool onlyIfLocalPlayer = true;

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			MDebugLog($"{nameof(OnPlayerTriggerEnter)} : {player.displayName} | {player.playerId}");

			bool isLocalPlayer = player == Networking.LocalPlayer;

			if (onlyIfLocalPlayer && (isLocalPlayer == false))
				return;

			SendEvents();
		}
	}
}