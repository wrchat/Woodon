using System;
using UnityEngine;
using UdonSharp;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class VoiceSetter : VoiceUpdater
	{
		[Header("_" + nameof(VoiceSetter))]
		[SerializeField] private VoiceState voiceState = VoiceState.Amplification;

		public override void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates)
		{
			for (int i = 0; i < playerApis.Length; i++)
			{
				bool isCondition = IsCondition(playerApis[i]) && Enable;

				if (isCondition)
				{
					voiceStates[i] = voiceState;
				}
				else
				{
					if (usePrevData)
					{
						voiceStates[i] = voiceStates[i];
					}
					else
					{
						voiceStates[i] = VoiceState.Default;
					}
				}
			}
		}

		protected abstract bool IsCondition(VRCPlayerApi playerApi);
	}
}