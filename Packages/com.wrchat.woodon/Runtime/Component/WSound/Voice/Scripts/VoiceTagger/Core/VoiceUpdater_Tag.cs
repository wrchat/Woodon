using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceUpdater_Tag : VoiceUpdater
	{
		[Header("_" + nameof(VoiceUpdater_Tag))]
		[SerializeField] private VoiceTagger[] voiceTaggers;

		public override void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates)
		{
			if (IsNotOnline())
				return;

			if (playerApis == null)
				return;

			string localTags = string.Empty;
			foreach (VoiceTagger voiceTagger in voiceTaggers)
			{
				string tag = VoiceUtil.GetVoiceTag(Networking.LocalPlayer, voiceTagger.Tag);

				if (tag == null)
					tag = FALSE_STRING;

				localTags += tag;
			}

			for (int i = 0; i < playerApis.Length; i++)
			{
				VRCPlayerApi player = playerApis[i];

				if (player == Networking.LocalPlayer)
					continue;

				string targetTags = string.Empty;
				foreach (VoiceTagger voiceTagger in voiceTaggers)
				{
					string tag = VoiceUtil.GetVoiceTag(player, voiceTagger.Tag);

					if (tag == null)
						tag = FALSE_STRING;

					targetTags += tag;
				}

				bool equal = localTags == targetTags;

				// MDebugLog($"{Networking.LocalPlayer.playerId + localTags}, {player.playerId + targetTags}, == {equal}");
				voiceStates[i] = ((voiceStates[i] != VoiceState.Mute) && equal)
					? VoiceState.Default
					: VoiceState.Mute;
			}
		}
	}
}