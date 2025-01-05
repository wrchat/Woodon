using System;
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
			Networking.LocalPlayer.SetPlayerTag($"{player.playerId}{voiceTag}", value ? TRUE_STRING : FALSE_STRING);
		}

		public static string GetVoiceTag(VRCPlayerApi player, VoiceTag voiceTag)
		{
			string tag = Networking.LocalPlayer.GetPlayerTag($"{player.playerId}{voiceTag}");
			if (tag == null)
				return FALSE_STRING;
			return tag;
		}

		public static bool HasVoiceTag(VoiceTag voiceTag, VRCPlayerApi players)
		{
			string tag = GetVoiceTag(players, voiceTag);
			return tag == TRUE_STRING;
		}
		
		public static bool HasVoiceTag(VoiceTag voiceTag, params VRCPlayerApi[] players)
		{
			foreach (VRCPlayerApi player in players)
			{
				if (HasVoiceTag(voiceTag, player) == false)
					return false;
			}
			return true;
		}
	}
}