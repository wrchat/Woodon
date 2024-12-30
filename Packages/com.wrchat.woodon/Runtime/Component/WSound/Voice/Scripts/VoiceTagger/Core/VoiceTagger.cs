using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	public abstract class VoiceTagger : MBase
	{
		[field: Header("_" + nameof(VoiceTagger))]
		[field: SerializeField] public VoiceTag Tag { get; private set; }
		[SerializeField] private float updateTerm = .5f;

		[SerializeField] private MBool localPlayerIn;
		[SerializeField] private MBool someoneIn;

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

			bool isLocalPlayerIn = false;
			bool isSomeoneIn = false;

			VRCPlayerApi[] playerApis = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
			VRCPlayerApi.GetPlayers(playerApis);

			for (int i = 0; i < playerApis.Length; i++)
			{
				bool isCondition = IsCondition(playerApis[i]);
				VoiceUtil.SetVoiceTag(playerApis[i], Tag, isCondition);

				isSomeoneIn = isSomeoneIn || isCondition;
				if (playerApis[i].isLocal)
					isLocalPlayerIn = isCondition;
			}

			if (localPlayerIn)
				localPlayerIn.SetValue(isLocalPlayerIn);
			if (someoneIn)
				someoneIn.SetValue(isSomeoneIn);
		}

		public abstract bool IsCondition(VRCPlayerApi player);
	}
}