using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_Area : VoiceSetter
	{
		[Header("_" + nameof(VoiceSetter_Area))]
		[SerializeField] private VoiceArea voiceArea;

		protected override bool IsCondition(VRCPlayerApi playerAPI)
		{
			bool isTarget = voiceArea.IsPlayerIn(playerAPI);
			MDebugLog(nameof(IsCondition) + isTarget);
			return isTarget;
		}
	}
}