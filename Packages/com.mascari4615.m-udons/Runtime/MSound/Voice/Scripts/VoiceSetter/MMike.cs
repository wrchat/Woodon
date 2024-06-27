﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[RequireComponent(typeof(VRC_Pickup))]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MMike : MBase
	{
		[SerializeField] private VRC_Pickup pickup;
		[SerializeField] private SyncedBool enable;

		public bool IsPlayerHolding(VRCPlayerApi targetPlayer)
		{
			return IsPlayerHolding(targetPlayer, pickup);
		}
		
		public bool IsEnabled()
		{
			if (enable != null)
				return enable.Value;
			else
				return true;
		}

		public bool IsPlayerHoldingAndEnabled(VRCPlayerApi targetPlayer)
		{
			return IsPlayerHolding(targetPlayer) && IsEnabled();
		}

		public override void OnPickupUseDown()
		{
			ToggleEnable();
		}

		public void ToggleEnable()
		{
			if (enable != null)
				enable.ToggleValue();
		}
	}
}