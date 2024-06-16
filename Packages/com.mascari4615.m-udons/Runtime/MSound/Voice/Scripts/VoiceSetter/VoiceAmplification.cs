using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class VoiceAmplification : VoiceUpdater
	{
		[Header("_" + nameof(VoiceAmplification))]
		[SerializeField] protected VoiceManager voiceManager;

		public override void UpdateVoice()
		{
			if (voiceManager.PlayerApis == null)
				return;

			for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
			{
				bool isAmplification = IsAmplification(voiceManager.PlayerApis[i]);
				isAmplification = isAmplification && ((enable == null) || enable.Value);

				voiceManager.VoiceStates[i] = isAmplification ? VoiceState.Amplification :
					usePrevData ? voiceManager.VoiceStates[i] : VoiceState.Default;
			}
		}

		protected abstract bool IsAmplification(VRCPlayerApi playerApi);
	}
}