using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_Tag : VoiceSetter
	{
		[Header("_" + nameof(VoiceSetter_Tag))]
		[SerializeField] private VoiceTag voiceTag;

		protected override bool IsCondition(VRCPlayerApi playerAPI)
		{
			bool isCondition = VoiceUtil.HasVoiceTag(voiceTag, playerAPI);
			MDebugLog($"{nameof(IsCondition)} : {isCondition}");
			return isCondition;
		}
	}
}