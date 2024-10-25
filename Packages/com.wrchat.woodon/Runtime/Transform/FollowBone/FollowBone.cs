using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class FollowBone : MBase
	{
		[Header("_" + nameof(FollowBone))]
		[SerializeField] private HumanBodyBones targetBone = HumanBodyBones.Head;
		[SerializeField] private GameObject[] targetObjects;
		[SerializeField] private Transform targetTrans;
		[SerializeField] private float fowardPower = 1;
		[SerializeField] private bool followRotation = true;
		[SerializeField] private bool followRoot;
		[SerializeField] private bool lerp;
		[SerializeField] private float lerpValue = 5f;

		private Vector3[] originPoss;
		private Quaternion[] originRots;

		[Header("_TargetPlayer")]
		[SerializeField] private MTarget mTarget;

		private void Start()
		{
			originPoss = new Vector3[targetObjects.Length];
			originRots = new Quaternion[targetObjects.Length];

			for (var i = 0; i < targetObjects.Length; i++)
			{
				originPoss[i] = targetObjects[i].transform.position;
				originRots[i] = targetObjects[i].transform.rotation;
			}
		}

		private void LateUpdate()
		{
			VRCPlayerApi targetPlayer;

			if (mTarget)
			{
				targetPlayer = (mTarget != null) && (mTarget.TargetPlayerID != NONE_INT)
					? VRCPlayerApi.GetPlayerById(mTarget.TargetPlayerID)
					: null;
			}
			else
			{
				targetPlayer = Networking.LocalPlayer;
			}

			if (targetPlayer == null)
			{
				for (var i = 0; i < targetObjects.Length; i++)
				{
					targetObjects[i].transform.position = originPoss[i];
					targetObjects[i].transform.rotation = originRots[i];
				}

				return;
			}

			if (followRoot)
			{
				targetTrans.position = targetPlayer.GetPosition();
				targetTrans.rotation = targetPlayer.GetRotation();
			}
			else
			{
				targetTrans.position = targetPlayer.GetBonePosition(targetBone);
				targetTrans.rotation = targetPlayer.GetBoneRotation(targetBone);
			}

			targetTrans.position += targetTrans.transform.forward * fowardPower;

			foreach (var targetObject in targetObjects)
				if (lerp)
				{
					targetObject.transform.position = Vector3.Lerp(targetObject.transform.position,
						targetTrans.position, lerpValue * Time.deltaTime);
					if (followRotation)
						targetObject.transform.rotation = Quaternion.Lerp(targetObject.transform.rotation,
							targetTrans.rotation, lerpValue * Time.deltaTime);
				}
				else
				{
					targetObject.transform.position = targetTrans.position;
					if (followRotation)
						targetObject.transform.rotation = targetTrans.rotation;
				}
		}
	}
}