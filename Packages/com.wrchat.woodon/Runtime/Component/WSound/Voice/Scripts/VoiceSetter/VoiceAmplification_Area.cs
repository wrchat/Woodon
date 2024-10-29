using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceAmplification_Area : VoiceAmplification
	{
		[Header("_" + nameof(VoiceAmplification_Area))]
		[SerializeField] private VoiceArea voiceArea;

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			bool isTarget = voiceArea.IsPlayerIn(playerAPI);
			MDebugLog(nameof(IsAmplification) + isTarget);
			return isTarget;
		}
	}
}