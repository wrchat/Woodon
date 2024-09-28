using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
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