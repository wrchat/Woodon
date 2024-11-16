using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceManager : MBase
	{
		protected const int VOICE_QUIET_GAIN = 5;
		protected const int VOICE_QUIET_FAR = 10;

		protected const int VOICE_DEFAULT_GAIN = 15;
		protected const int VOICE_DEFAULT_FAR = 25;

		protected const int VOICE_AMPLIFICATION_GAIN = 10;
		protected const int VOICE_AMPLIFICATION_FAR = 300;

		[field: Header("_" + nameof(VoiceManager))]
		public VRCPlayerApi[] PlayerApis { get; private set; }
		public VoiceState[] VoiceStates { get; private set; }
		public float[] CurVoiceFar { get; private set; }
		public float[] CurVoiceGain { get; private set; }
		[field: SerializeField] public int VoiceDefaultFarBoost { get; private set; }
		[field: SerializeField] public int VoiceDefaultGainBoost { get; private set; }
		[field: SerializeField] public int VoiceAmplificationFarBoost { get; private set; }
		[field: SerializeField] public int VoiceAmplificationGainBoost { get; private set; }

		[SerializeField] private VoiceUpdater[] voiceUpdaters;
		[SerializeField] private float updateTerm = .1f;
		[SerializeField] private bool useLerp = false;
		[SerializeField] private float lerpSpeed = 2f;

		public bool CanUpdateNow => (PlayerApis != null) && (PlayerApis.Length == VRCPlayerApi.GetPlayerCount()) && (VoiceStates != null);

		private bool isInited = false;

		private void Start() => Init();

		private void Init()
		{
			if (isInited)
				return;
			isInited = true;

			foreach (VoiceUpdater voiceUpdater in voiceUpdaters)
				voiceUpdater.Init(this);

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
			for (int i = 0; i < PlayerApis.Length; i++)
				VoiceStates[i] = VoiceState.Default;

			// 플레이어 별 VoiceState 종합 계산
			foreach (VoiceUpdater voiceUpdater in voiceUpdaters)
				voiceUpdater.UpdateVoice();

			// 플레이어 별 VoiceState 적용
			if (useLerp)
			{
				for (int i = 0; i < PlayerApis.Length; i++)
					SetVoiceLerp(i);
			}
			else
			{
				for (int i = 0; i < PlayerApis.Length; i++)
					SetVoice(PlayerApis[i], VoiceStates[i]);
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player) => UpdatePlayerList();
		public override void OnPlayerLeft(VRCPlayerApi player) => UpdatePlayerList();

		private void UpdatePlayerList()
		{
			MDebugLog($"{nameof(UpdatePlayerList)}, PlayerCount = {VRCPlayerApi.GetPlayerCount()}");

			PlayerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VoiceStates = new VoiceState[PlayerApis.Length];
			CurVoiceFar = new float[PlayerApis.Length];
			CurVoiceGain = new float[PlayerApis.Length];
			VRCPlayerApi.GetPlayers(PlayerApis);
		}

		protected void SetVoice(VRCPlayerApi player, VoiceState voiceState)
		{
			// MDebugLog($"{nameof(SetVoice)} : {player.playerId}, {voiceState}");

			player.SetVoiceDistanceNear(0);
			switch (voiceState)
			{
				case VoiceState.Default:
					player.SetVoiceDistanceFar(VOICE_DEFAULT_FAR + VoiceDefaultFarBoost);
					player.SetVoiceGain(VOICE_DEFAULT_GAIN + VoiceDefaultGainBoost);
					break;
				case VoiceState.Quiet:
					player.SetVoiceDistanceFar(VOICE_QUIET_FAR);
					player.SetVoiceGain(VOICE_QUIET_GAIN);
					break;
				case VoiceState.Mute:
					player.SetVoiceDistanceFar(0);
					player.SetVoiceGain(0);
					break;
				case VoiceState.Amplification:
					player.SetVoiceDistanceFar(VOICE_AMPLIFICATION_FAR + VoiceAmplificationFarBoost);
					player.SetVoiceGain(VOICE_AMPLIFICATION_GAIN + VoiceAmplificationGainBoost);
					break;
			}
		}

		public void SetVoiceLerp(int index)
		{
			MDebugLog(nameof(SetVoiceLerp));

			VRCPlayerApi player = PlayerApis[index];
			VoiceState voiceState = VoiceStates[index];

			player.SetVoiceDistanceNear(0);

			float targetFar = VOICE_DEFAULT_FAR;
			float targetGain = VOICE_DEFAULT_GAIN;

			switch (voiceState)
			{
				case VoiceState.Default:
					targetFar = VOICE_DEFAULT_FAR + VoiceDefaultFarBoost;
					targetGain = VOICE_DEFAULT_GAIN + VoiceDefaultGainBoost;
					break;
				case VoiceState.Quiet:
					targetFar = VOICE_QUIET_FAR;
					targetGain = VOICE_QUIET_GAIN;
					break;
				case VoiceState.Mute:
					targetFar = 0;
					targetGain = 0;
					break;
				case VoiceState.Amplification:
					targetFar = VOICE_AMPLIFICATION_FAR + VoiceAmplificationFarBoost;
					targetGain = VOICE_AMPLIFICATION_GAIN + VoiceAmplificationGainBoost;
					break;
			}

			player.SetVoiceDistanceFar(CurVoiceFar[index] = Mathf.Lerp(CurVoiceFar[index], targetFar, Time.deltaTime * lerpSpeed));
			player.SetVoiceGain(CurVoiceGain[index] = Mathf.Lerp(CurVoiceGain[index], targetGain, Time.deltaTime * lerpSpeed));
		}
	}
}