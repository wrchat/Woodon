
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplification_Area : VoiceAmplification
	{
		[Header("_" + nameof(VoiceAmplification_Area))]
		[SerializeField] private VoiceArea voiceSeparator;

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			bool isTarget = voiceSeparator.IsPlayerIn(playerAPI);
			MDebugLog(nameof(IsAmplification) + isTarget);
			return isTarget;
		}
	}
}