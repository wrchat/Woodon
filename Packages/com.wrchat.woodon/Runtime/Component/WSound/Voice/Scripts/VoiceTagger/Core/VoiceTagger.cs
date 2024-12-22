using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	public class VoiceTagger : MBase
	{
		[field: Header("_" + nameof(VoiceTagger))]
		[field: SerializeField] public VoiceAreaTag Tag { get; private set; }
		[SerializeField] private float updateTerm = .5f;

		[SerializeField] private MBool localPlayerIn;
		private bool isLocalPlayerIn;
		[SerializeField] private MBool someoneIn;
		private bool isSomeoneIn;

		protected virtual void Start() => UpdateVoiceLoop();
		public void UpdateVoiceLoop()
		{
			SendCustomEventDelayedSeconds(nameof(UpdateVoiceLoop), updateTerm);
			UpdateAllTag();
		}

		protected virtual void UpdateAllTag()
		{
			if (IsNotOnline())
				return;

			isLocalPlayerIn = false;
			isSomeoneIn = false;

			VRCPlayerApi[] playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VRCPlayerApi.GetPlayers(playerApis);

			if (playerApis != null &&
				playerApis.Length == VRCPlayerApi.GetPlayerCount())
			{
				for (int i = 0; i < playerApis.Length; i++)
				{
					bool isIn = IsPlayerIn(playerApis[i]);

					UpdatePlayerTag(playerApis[i], isIn);

					isSomeoneIn = isSomeoneIn || isIn;
					if (playerApis[i].isLocal)
						isLocalPlayerIn = isIn;
				}
			}

			if (localPlayerIn)
				localPlayerIn.SetValue(isLocalPlayerIn);
			if (someoneIn)
				someoneIn.SetValue(isSomeoneIn);
		}

		public virtual bool IsPlayerIn(VRCPlayerApi player) { return true; }

		private bool UpdatePlayerTag(VRCPlayerApi player, bool isIn)
		{
			// MDebugLog($"{playerID}{Tag}" + (isin ? TRUE_STRING : FALSE_STRING));
			Networking.LocalPlayer.SetPlayerTag($"{player.playerId}{Tag}", isIn ? TRUE_STRING : FALSE_STRING);
			return isIn;
		}
	}
}