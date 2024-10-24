using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
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