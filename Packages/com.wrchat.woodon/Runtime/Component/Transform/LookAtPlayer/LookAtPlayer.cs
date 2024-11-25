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
		[SerializeField] private float lerpSpeed = 5;
		[SerializeField] private bool isForUI = false;
		[SerializeField] private bool onlyRotateYAxis = false;

		private void Update()
		{
			// 타겟 플레이어 설정
			VRCPlayerApi targetPlayer;
			{
				if (IsNotOnline())
					return;

				targetPlayer = Networking.LocalPlayer;

				if (mTarget != null)
				{
					targetPlayer = VRCPlayerApi.GetPlayerById(mTarget.TargetPlayerID);
					if (targetPlayer == null)
						targetPlayer = Networking.LocalPlayer;
				}
			}

			// 타겟 플레이어의 머리 위치
			Vector3 headPos;
			{
				headPos = targetPlayer.GetBonePosition(targetBone);
			}

			// 계산
			{
				if (onlyRotateYAxis)
				{
					if (lerp)
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(headPos.x, transform.position.y, headPos.z) - transform.position), lerpSpeed * Time.deltaTime);
					else
						transform.LookAt(new Vector3(headPos.x, transform.position.y, headPos.z));
				}
				else
				{
					if (lerp)
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(headPos - transform.position), lerpSpeed * Time.deltaTime);
					else
						transform.LookAt(headPos);
				}

				if (isForUI)
					transform.Rotate(0, 180, 0);
			}
		}
	}
}