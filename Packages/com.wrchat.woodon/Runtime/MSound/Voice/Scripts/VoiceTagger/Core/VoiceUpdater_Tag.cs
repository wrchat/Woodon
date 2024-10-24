using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceUpdater_Tag : VoiceUpdater
	{
		[SerializeField] private VoiceTagger[] voiceTaggers;

		public override void UpdateVoice()
		{
			if (IsNotOnline())
				return;

			if (voiceManager.PlayerApis == null)
				return;

			string localTags = string.Empty;

			foreach (VoiceTagger voiceTagger in voiceTaggers)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{voiceTagger.Tag}");

				if (tag == null)
					tag = FALSE_STRING;

				localTags += tag;
			}

			for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
			{
				VRCPlayerApi player = voiceManager.PlayerApis[i];

				if (player == Networking.LocalPlayer)
					continue;

				string targetTags = string.Empty;
				foreach (var areaTagger in voiceTaggers)
				{
					string tag = Networking.LocalPlayer.GetPlayerTag($"{player.playerId}{areaTagger.Tag}");

					if (tag == null)
						tag = FALSE_STRING;

					targetTags += tag;
				}

				bool equal = localTags == targetTags;

				// MDebugLog($"{Networking.LocalPlayer.playerId + localTags}, {player.playerId + targetTags}, == {equal}");
				voiceManager.VoiceStates[i] = ((voiceManager.VoiceStates[i] != VoiceState.Mute) && equal)
					? VoiceState.Default
					: VoiceState.Mute;
			}
		}
	}
}