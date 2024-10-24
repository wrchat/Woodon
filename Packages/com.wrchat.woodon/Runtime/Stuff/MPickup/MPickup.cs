using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using static VRC.SDKBase.VRC_Pickup;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MPickup : MBase
	{
		[field: Header("_" + nameof(MPickup))]
		[field: SerializeField] public VRC_Pickup Pickup { get; private set; }
		[field: SerializeField] public VRCObjectSync ObjectSync { get; private set; }
		[field: SerializeField] public MeshRenderer[] MeshRenderers { get; private set; }
		[field: SerializeField] public GameObject[] Childs { get; private set; }
		[field: SerializeField] public Collider[] Colliders { get; private set; }
		[field: SerializeField] public Rigidbody Rigidbody { get; private set; }
		[SerializeField] protected bool useGravityWhenOncePickedUp = false;

		[SerializeField] protected MBool isHolding;
		[SerializeField] protected MBool isSomeoneHolding;
		// [field: SerializeField] public MBool MBool { get; private set; }

		private bool _enabled = true;
		public bool Enabled
		{
			get => _enabled;
			private set
			{
				_enabled = value;
				OnEnableChange();
			}
		}

		private void OnEnableChange()
		{
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

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		public bool IsHolding(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;

			return IsPlayerHolding(targetPlayer, Pickup);
		}

		public PickupHand GetLocalPlayerGripHand()
		{
			if (IsHolding() == false)
				return PickupHand.None;

			return Networking.LocalPlayer.GetPickupInHand(PickupHand.Right) == Pickup
				? PickupHand.Right
				: PickupHand.Left;
		}

		public void Drop()
		{
			if (IsHolding())
			{
				UseGravity(false);
				Pickup.Drop();
			}
		}

		public void Drop_G()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Drop));
		}

		public void Respawn()
		{
			UseGravity(false);

			Drop();

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

		public override void OnPickup()
		{
			// MDebugLog(nameof(OnPickup));

			if (useGravityWhenOncePickedUp)
				UseGravity(true);

			if (isHolding != null)
				isHolding.SetValue(true);
		}

		public override void OnDrop()
		{
			// MDebugLog(nameof(OnDrop));
		
			if (isHolding != null)
				isHolding.SetValue(false);
		}

		protected virtual void Update()
		{
			if (isSomeoneHolding == null)
				return;

			VRCPlayerApi[] players = GetPlayers();
			foreach (VRCPlayerApi player in players)
			{
				if (IsPlayerHolding(player, Pickup))
				{
					isSomeoneHolding.SetValue(true);
					return;
				}
			}

			isSomeoneHolding.SetValue(false);
		}

		public void UseGravity(bool useGravity)
		{
			if (Rigidbody != null)
			{
				Rigidbody.isKinematic = !useGravity;
				Rigidbody.useGravity = useGravity;
			}
		}
	}
}