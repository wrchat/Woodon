using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TeleportTo : MBase
	{
		[Header("_" + nameof(TeleportTo))]
		[SerializeField] private Transform targetPos;
		[SerializeField] private bool onInteract = true;
		[SerializeField] private bool onCollision;

		private void Start()
		{
			this.DisableInteractive = !onInteract;
		}

		public override void Interact()
		{
			if (onInteract)
				Teleport();
		}

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			if (onCollision)
				if (Networking.LocalPlayer == player)
					Teleport();
		}

		public void Teleport()
		{
			MDebugLog(nameof(Teleport));
			Networking.LocalPlayer.TeleportTo(targetPos.position, targetPos.rotation);
		}
	}
}