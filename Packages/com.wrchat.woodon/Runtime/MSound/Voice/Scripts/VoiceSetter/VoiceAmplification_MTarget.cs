using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplification_MTarget : VoiceAmplification
	{
		[field: Header("_" + nameof(VoiceAmplification_MTarget))]
		[field: SerializeField] public MTarget[] TargetPlayers { get; private set; }

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			foreach (MTarget targetPlayer in TargetPlayers)
			{
				if (playerAPI.playerId == targetPlayer.TargetPlayerID)
					return true;
			}

			return false;
		}

		public void SetPlayer(int id, int index = 0)
		{
			TargetPlayers[index].SetTarget(id);
		}
	}
}