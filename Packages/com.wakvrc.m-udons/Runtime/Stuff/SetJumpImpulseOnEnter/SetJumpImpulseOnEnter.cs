using UnityEngine;
using VRC.SDKBase;
using static WakVRC.MUtil;

namespace WakVRC
{
	public class SetJumpImpulseOnEnter : MBase
	{
		[SerializeField] private float targetImpulse;

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			if (IsLocalPlayer(player)) player.SetJumpImpulse(targetImpulse);
		}
	}
}