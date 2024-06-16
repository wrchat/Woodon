using UnityEngine;
using VRC.SDKBase;

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