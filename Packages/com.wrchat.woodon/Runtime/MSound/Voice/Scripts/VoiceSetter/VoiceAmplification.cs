using UdonSharp;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class VoiceAmplification : VoiceUpdater
	{
		// [Header("_" + nameof(VoiceAmplification))]

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