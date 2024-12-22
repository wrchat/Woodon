using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[RequireComponent(typeof(VRC_Pickup))]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class WMike : MPickup
	{
		[Header("_" + nameof(WMike))]
		[SerializeField] private MBool mikeEnable;
		
		public bool IsMikeEnabled()
		{
			if (mikeEnable != null)
				return mikeEnable.Value;
			else
				return true;
		}

		public bool IsPlayerHoldingAndEnabled(VRCPlayerApi targetPlayer)
		{
			return IsHolding(targetPlayer) && IsMikeEnabled();
		}

		public override void OnPickupUseDown()
		{
			ToggleMikeEnable();
		}

		public void ToggleMikeEnable()
		{
			if (mikeEnable != null)
				mikeEnable.ToggleValue();
		}
	}
}