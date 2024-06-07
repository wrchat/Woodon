using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplificationTargetNear : VoiceAmplification
	{
		public MTarget MTarget => targetPlayer;
		[SerializeField] private MTarget targetPlayer;

		[SerializeField] private float amplificationDistance = 10;

		protected override bool IsAmplification(VRCPlayerApi playerApi)
		{
			VRCPlayerApi targetPlayerAPI = targetPlayer.GetTargetPlayerAPI();

			if (targetPlayerAPI == null)
				return false;

			Vector3 targetPos = targetPlayerAPI.GetPosition();

			Vector3 thisPos = VRCPlayerApi.GetPlayerById(playerApi.playerId).GetPosition();
			bool isTarget = Vector3.Distance(thisPos, targetPos) < amplificationDistance;
			return isTarget;
		}
	}
}