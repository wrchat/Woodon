using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public static class VoiceUtil
	{
		public const string TRUE_STRING = "TRUE";
		public const string FALSE_STRING = "FALSE";

		public static void SetVoiceTag(VRCPlayerApi player, VoiceTag voiceTag, bool value)
		{
			player.SetPlayerTag($"{player.playerId}{voiceTag}", value ? TRUE_STRING : FALSE_STRING);
		}
		
		public static bool HasVoiceTag(VRCPlayerApi player, VoiceTag voiceTag)
		{
			string tag = player.GetPlayerTag($"{player.playerId}{voiceTag}");
			return tag == TRUE_STRING;
		}
	}
}