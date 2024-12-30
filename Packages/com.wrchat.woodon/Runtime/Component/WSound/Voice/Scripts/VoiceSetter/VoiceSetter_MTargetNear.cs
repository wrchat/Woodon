using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_MTargetNear : VoiceSetter
	{
		[field: Header("_" + nameof(VoiceSetter_MTargetNear))]
		[field: SerializeField] public MTarget TargetPlayer { get; private set; }
		[SerializeField] private float amplificationDistance = 10;

		protected override bool IsCondition(VRCPlayerApi playerApi)
		{
			VRCPlayerApi targetPlayerAPI = TargetPlayer.GetTargetPlayerAPI();

			if (targetPlayerAPI == null)
				return false;

			Vector3 nearTargetPos = targetPlayerAPI.GetPosition();
			Vector3 playerPos = VRCPlayerApi.GetPlayerById(playerApi.playerId).GetPosition();

			bool isCondition = Vector3.Distance(playerPos, nearTargetPos) < amplificationDistance;
			return isCondition;
		}
	}
}