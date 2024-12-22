using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_MTarget : VoiceSetter
	{
		[field: Header("_" + nameof(VoiceSetter_MTarget))]
		[field: SerializeField] public MTarget[] TargetPlayers { get; private set; }

		protected override bool IsCondition(VRCPlayerApi playerAPI)
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