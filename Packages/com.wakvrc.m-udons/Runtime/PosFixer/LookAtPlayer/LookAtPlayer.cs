using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	public class LookAtPlayer : MBase
	{
		[SerializeField] private HumanBodyBones targetBone = HumanBodyBones.Head;
		[SerializeField] private MTarget mTarget;
		[SerializeField] private bool lerp;
		[SerializeField] private float lerpValue = 5;

		private void Update()
		{
			if (IsNotOnline())
				return;

			VRCPlayerApi targetPlayer = Networking.LocalPlayer;

			if (mTarget != null)
			{
				targetPlayer = VRCPlayerApi.GetPlayerById(mTarget.CurTargetPlayerID);
				if (targetPlayer == null)
					targetPlayer = Networking.LocalPlayer;
			}

			Vector3 headPos = targetPlayer.GetBonePosition(targetBone);

			if (lerp)
				transform.rotation = Quaternion.Lerp(transform.rotation,
					Quaternion.LookRotation(headPos - transform.position), Time.deltaTime * lerpValue);
			else
				transform.LookAt(headPos);
		}
	}
}