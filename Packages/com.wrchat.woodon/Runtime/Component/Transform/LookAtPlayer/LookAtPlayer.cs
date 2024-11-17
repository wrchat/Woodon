using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class LookAtPlayer : MBase
	{
		[Header("_" + nameof(LookAtPlayer))]
		[SerializeField] private HumanBodyBones targetBone = HumanBodyBones.Head;
		[SerializeField] private MTarget mTarget;
		[SerializeField] private bool lerp;
		[SerializeField] private float lerpValue = 5;
		[SerializeField] private bool isForUI = false;

		private void Update()
		{
			if (IsNotOnline())
				return;

			VRCPlayerApi targetPlayer = Networking.LocalPlayer;

			if (mTarget != null)
			{
				targetPlayer = VRCPlayerApi.GetPlayerById(mTarget.TargetPlayerID);
				if (targetPlayer == null)
					targetPlayer = Networking.LocalPlayer;
			}

			Vector3 headPos = targetPlayer.GetBonePosition(targetBone);

			if (lerp)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation,
					Quaternion.LookRotation(headPos - transform.position), Time.deltaTime * lerpValue);
			}
			else
			{
				transform.LookAt(headPos);
			}

			if (isForUI)
				transform.Rotate(0, 180, 0);
		}
	}
}