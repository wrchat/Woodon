using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MPickup : MBase
	{
		private bool _enabled = true;
		public bool Enabled
		{
			get => _enabled;
			private set
			{
				_enabled = value;

				foreach (MeshRenderer meshRenderer in MeshRenderers)
					meshRenderer.enabled = _enabled;

				foreach (Collider collider in Colliders)
					collider.enabled = _enabled;

				foreach (GameObject child in Childs)
					child.SetActive(_enabled);

				Pickup.pickupable = _enabled;

				if (_enabled == false)
					Drop();
			}
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		[field: SerializeField] public VRC_Pickup Pickup { get; private set; }
		[field: SerializeField] public VRCObjectSync ObjectSync { get; private set; }
		[field: SerializeField] public MeshRenderer[] MeshRenderers { get; private set; }
		[field: SerializeField] public GameObject[] Childs { get; private set; }
		[field: SerializeField] public Collider[] Colliders { get; private set; }
		// [field: SerializeField] public CustomBool CustomBool { get; private set; }

		public bool IsHolding(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			return IsPlayerHolding(targetPlayer, Pickup);
		}

		public void Drop()
		{
			if (IsHolding())
				Pickup.Drop();
		}

		public void Drop_G()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Drop));
		}

		public void Respawn()
		{
			if (ObjectSync)
				ObjectSync.Respawn();
		}

		public void Respawn_G()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Respawn));
		}

		public void DropAndRespawn()
		{
			Drop();

			if (IsOwner())
				Respawn();
		}

		public void DropAndRespawn_G()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(DropAndRespawn));
		}
	}
}