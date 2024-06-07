
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplification_MTarget : VoiceAmplification
	{
		[Header("_" + nameof(VoiceAmplification_MTarget))]
		[SerializeField] private MTarget[] targetPlayers;
		public MTarget[] MTargets => targetPlayers;

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			foreach (var targetPlayer in targetPlayers)
			{
				if (playerAPI.playerId == targetPlayer.CurTargetPlayerID)
					return true;
			}

			return false;
		}

		public void SetPlayer(int id, int index = 0)
		{
			targetPlayers[index].SetPlayer(id);
		}
	}
}