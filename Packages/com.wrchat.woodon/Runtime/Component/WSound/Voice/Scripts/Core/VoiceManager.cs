using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceManager : WBase
	{
		[field: Header("_" + nameof(VoiceManager))]
		[field: SerializeField] public int VoiceDefaultFarBoost { get; private set; }
		[field: SerializeField] public int VoiceDefaultGainBoost { get; private set; }
		[field: SerializeField] public int VoiceAmplificationFarBoost { get; private set; }
		[field: SerializeField] public int VoiceAmplificationGainBoost { get; private set; }
		[field: SerializeField] public float VoiceFarBoostDistance { get; private set; } = 100f;

		[SerializeField] private VoiceState initialVoiceState = VoiceState.Default;
		[SerializeField] private VoiceUpdater[] voiceUpdaters;
		[SerializeField] private float updateTerm = .1f;
		[SerializeField] private bool useLerp = false;
		[SerializeField] private float lerpSpeed = 2f;

		private VRCPlayerApi[] playerApis;
		private VoiceState[] voiceStates;
		private float[] curVoiceFar;
		private float[] curVoiceGain;
		private bool isInited = false;

		public bool CanUpdateNow => (playerApis != null) && (playerApis.Length == VRCPlayerApi.GetPlayerCount()) && (voiceStates != null);

		private void Start() => Init();

		private void Init()
		{
			if (isInited)
				return;
			isInited = true;

			UpdateVoiceLoop();
		}

		public void UpdateVoiceLoop()
		{
			if (useLerp || updateTerm <= 0)
				return;

			SendCustomEventDelayedSeconds(nameof(UpdateVoiceLoop), updateTerm);
			UpdateVoice();
		}

		private void Update()
		{
			if (useLerp || updateTerm <= 0)
				UpdateVoice();
		}

		public void UpdateVoice()
		{
			if (isInited == false)
				Init();

			// 플레이어 리스트 유효성 확인
			if (CanUpdateNow == false)
			{
				UpdatePlayerList();
				return;
			}

			// Default로 초기화
			for (int i = 0; i < playerApis.Length; i++)
				voiceStates[i] = initialVoiceState;

			// 플레이어 별 VoiceState 종합 계산
			foreach (VoiceUpdater voiceUpdater in voiceUpdaters)
				voiceUpdater.UpdateVoice(playerApis, voiceStates);

			// 플레이어 별 VoiceState 적용
			for (int i = 0; i < playerApis.Length; i++)
				SetVoice(i, playerApis[i], voiceStates[i]);
		}

		public override void OnPlayerJoined(VRCPlayerApi player) => UpdatePlayerList();
		public override void OnPlayerLeft(VRCPlayerApi player) => UpdatePlayerList();

		private void UpdatePlayerList()
		{
			WDebugLog($"{nameof(UpdatePlayerList)}, PlayerCount = {VRCPlayerApi.GetPlayerCount()}");

			playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			voiceStates = new VoiceState[playerApis.Length];
			curVoiceFar = new float[playerApis.Length];
			curVoiceGain = new float[playerApis.Length];
			VRCPlayerApi.GetPlayers(playerApis);
		}

		protected void SetVoice(int index, VRCPlayerApi player, VoiceState voiceState)
		{
			// WDebugLog($"{nameof(SetVoice)} : {player.playerId}, {voiceState}");

			const int VOICE_QUIET_GAIN = 5;
			const int VOICE_QUIET_FAR = 10;

			const int VOICE_DEFAULT_GAIN = 15;
			const int VOICE_DEFAULT_FAR = 25;

			const int VOICE_AMPLIFICATION_GAIN = 10;
			const int VOICE_AMPLIFICATION_FAR = 300;

			float distanceFar = VOICE_DEFAULT_FAR;
			float gain = VOICE_DEFAULT_GAIN;

			switch (voiceState)
			{
				case VoiceState.Default:
					distanceFar = VOICE_DEFAULT_FAR + VoiceDefaultFarBoost;
					gain = VOICE_DEFAULT_GAIN + VoiceDefaultGainBoost;
					break;
				case VoiceState.Quiet:
					distanceFar = VOICE_QUIET_FAR;
					gain = VOICE_QUIET_GAIN;
					break;
				case VoiceState.Mute:
					distanceFar = 0;
					gain = 0;
					break;
				case VoiceState.Amplification:
					distanceFar = VOICE_AMPLIFICATION_FAR + VoiceAmplificationFarBoost;

					// 로컬 플레이어와 해당 플레이어와의 거리가 가까울 때는 증폭 키면 조금 귀 아픔. 거리 기준으로 Lerp 적용. [고선파] - KarmoDDrine 2025.11.02, 2025 11.16
					float distance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), player.GetPosition());
					bool isFarEnough = distance >= VoiceFarBoostDistance;

					float normalGain = VOICE_DEFAULT_GAIN + VoiceDefaultGainBoost;
					float amplifiedGain = VOICE_AMPLIFICATION_GAIN + VoiceAmplificationGainBoost;

					if (isFarEnough)
					{
						gain = VOICE_AMPLIFICATION_GAIN + VoiceAmplificationGainBoost;
					}
					else
					{
						float t = distance / VoiceFarBoostDistance;
						gain = Mathf.Lerp(normalGain, amplifiedGain, t);
					}

					player.SetVoiceGain(gain);
					break;
			}

			if (useLerp)
			{
				curVoiceFar[index] = Mathf.Lerp(curVoiceFar[index], distanceFar, Time.deltaTime * lerpSpeed);
				curVoiceGain[index] = Mathf.Lerp(curVoiceGain[index], gain, Time.deltaTime * lerpSpeed);

				distanceFar = curVoiceFar[index];
				gain = curVoiceGain[index];
			}

			player.SetVoiceDistanceNear(0);
			player.SetVoiceDistanceFar(distanceFar);
			player.SetVoiceGain(gain);
		}
	}
}