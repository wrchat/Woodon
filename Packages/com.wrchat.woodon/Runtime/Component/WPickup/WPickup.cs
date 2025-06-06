﻿using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using static VRC.SDKBase.VRC_Pickup;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class WPickup : WBase
	{
		[field: Header("_" + nameof(WPickup))]
		[field: SerializeField] public VRC_Pickup Pickup { get; private set; }
		[field: SerializeField] public VRCObjectSync ObjectSync { get; private set; }
		[field: SerializeField] public MeshRenderer[] MeshRenderers { get; private set; }
		[field: SerializeField] public GameObject[] Childs { get; private set; }
		[field: SerializeField] public Collider[] Colliders { get; private set; }
		[field: SerializeField] public Rigidbody Rigidbody { get; private set; }
		[SerializeField] protected bool useGravityWhenOncePickedUp = false;

		[SerializeField] protected WBool isHolding;
		[SerializeField] protected WBool isSomeoneHolding;
		[SerializeField] protected WBool isEnabledWBool;
		// [field: SerializeField] public WBool WBool { get; private set; }

		private bool isEnabled = true;
		public bool IsEnabled
		{
			get => isEnabled;
			private set
			{
				isEnabled = value;
				OnEnableChange();
			}
		}

		private void OnEnableChange()
		{
			foreach (MeshRenderer meshRenderer in MeshRenderers)
				meshRenderer.enabled = isEnabled;

			foreach (Collider collider in Colliders)
				collider.enabled = isEnabled;

			foreach (GameObject child in Childs)
				child.SetActive(isEnabled);

			Pickup.pickupable = isEnabled;

			if (isEnabled == false)
				Drop();
		}

		public void SetEnabled(bool enabled)
		{
			if (isEnabledWBool != null)
			{
				isEnabledWBool.SetValue(enabled);
			}
			else
			{
				IsEnabled = enabled;
			}
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
			// WDebugLog(nameof(OnPickup));

			if (useGravityWhenOncePickedUp)
				UseGravity(true);

			if (isHolding != null)
				isHolding.SetValue(true);
		}

		public override void OnDrop()
		{
			// WDebugLog(nameof(OnDrop));

			if (isHolding != null)
				isHolding.SetValue(false);
		}

		private void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (isEnabledWBool != null)
			{
				isEnabledWBool.RegisterListener(this, nameof(SetEnabledTrue), WBoolEvent.OnTrue);
				isEnabledWBool.RegisterListener(this, nameof(SetEnabledFalse), WBoolEvent.OnFalse);
				IsEnabled = isEnabledWBool.Value;
			}
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

		#region HorribleEvents
		// 아래는 로컬로 동작합니다. (isEnabled 값이 바뀌지 않습니다.)
		public void SetEnabledTrue() => IsEnabled = true;
		public void SetEnabledFalse() => IsEnabled = false;
		#endregion
	}
}