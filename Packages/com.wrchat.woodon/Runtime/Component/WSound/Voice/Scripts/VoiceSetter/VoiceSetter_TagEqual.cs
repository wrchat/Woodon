using UdonSharp;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_TagEqual : VoiceSetter
	{
		protected override bool IsCondition(VRCPlayerApi playerAPI)
		{
			if (playerAPI.isLocal)
				return false; // 로컬 플레이어는 항상 false 반환

			VoiceTag[] allVoiceTags = new VoiceTag[]
			{
				VoiceTag.AREA_1,
				VoiceTag.AREA_2,
				VoiceTag.AREA_3,
				VoiceTag.AREA_4,
				VoiceTag.AREA_A,
				VoiceTag.AREA_B,
				VoiceTag.AREA_C,
				VoiceTag.AREA_D,
				VoiceTag.ROOM_1,
				VoiceTag.ROOM_2,
				VoiceTag.ROOM_3,
				VoiceTag.ROOM_4,
				VoiceTag.ROOM_A,
				VoiceTag.ROOM_B,
				VoiceTag.ROOM_C,
				VoiceTag.ROOM_D,
				VoiceTag.ELSE_1,
				VoiceTag.ELSE_2,
				VoiceTag.ELSE_3,
				VoiceTag.ELSE_4,
			};
			
			foreach (VoiceTag voiceTag in allVoiceTags)
			{
				bool hasTagLocalPlayer = VoiceUtil.HasVoiceTag(voiceTag, Networking.LocalPlayer);
				bool hasTag = VoiceUtil.HasVoiceTag(voiceTag, playerAPI);
				WDebugLog($"{nameof(IsCondition)} : {voiceTag} - LocalPlayer: {hasTagLocalPlayer}, Player({playerAPI.displayName}): {hasTag}");
				if (hasTagLocalPlayer != hasTag)
				{
					WDebugLog($"{nameof(IsCondition)} : false");
					return false; // 하나라도 다르면 false 반환
				}
			}

			return true; // 모두 같으면 true 반환
		}
	}
}