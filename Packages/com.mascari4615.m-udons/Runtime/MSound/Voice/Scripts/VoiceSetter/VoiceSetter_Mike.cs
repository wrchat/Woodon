using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_Mike : VoiceAmplification
	{
		[Header("_" + nameof(VoiceSetter_Mike))]
		[SerializeField] private MMike[] mikes;

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			bool isTarget = false;

			foreach (MMike mike in mikes)
			{
				if (mike.IsPlayerHoldingAndEnabled(playerAPI))
				{
					isTarget = true;
					break;
				}
			}
			
			MDebugLog(nameof(IsAmplification) + isTarget);
			return isTarget;
		}
	}
}