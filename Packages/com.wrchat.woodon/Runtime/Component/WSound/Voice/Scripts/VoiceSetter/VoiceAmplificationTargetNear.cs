using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplificationTargetNear : VoiceAmplification
	{
		[field: Header("_" + nameof(VoiceAmplificationTargetNear))]
		[field: SerializeField] public MTarget TargetPlayer { get; private set; }
		[SerializeField] private float amplificationDistance = 10;

		protected override bool IsAmplification(VRCPlayerApi playerApi)
		{
			VRCPlayerApi targetPlayerAPI = TargetPlayer.GetTargetPlayerAPI();

			if (targetPlayerAPI == null)
				return false;

			Vector3 targetPos = targetPlayerAPI.GetPosition();

			Vector3 thisPos = VRCPlayerApi.GetPlayerById(playerApi.playerId).GetPosition();
			bool isTarget = Vector3.Distance(thisPos, targetPos) < amplificationDistance;
			return isTarget;
		}
	}
}