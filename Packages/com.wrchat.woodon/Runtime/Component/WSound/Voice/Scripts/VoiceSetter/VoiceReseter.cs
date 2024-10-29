using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceReseter : VoiceUpdater
	{
		[Header("_" + nameof(VoiceReseter))]
		[SerializeField] private MTarget[] ignoreTargets;

		[SerializeField] private bool useIngnoreTargetTag;
		[SerializeField] private VoiceAreaTag IgnoreTargetTag;

		[SerializeField] private bool useTargetTag;
		[SerializeField] private VoiceAreaTag targetTag;

		public override void UpdateVoice()
		{
			if (IsNotOnline())
				return;

			if (voiceManager.PlayerApis == null)
				return;

			if (enable != null && (enable.Value == false))
				return;

			// 무시 MTarget 대상이라면 return
			foreach (MTarget ignoreTarget in ignoreTargets)
			{
				if (ignoreTarget.IsTargetPlayer(Networking.LocalPlayer))
					return;
			}

			// 무시 태그를 가지고 있으면 return
			if (useIngnoreTargetTag)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{IgnoreTargetTag}");

				if (tag == TRUE_STRING)
					return;
			}

			// 타겟 태그를 가지고 있지 않으면 return
			if (useTargetTag)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{targetTag}");

				if (tag != TRUE_STRING)
					return;
			}

			// 보이스 상태 초기화
			for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
				voiceManager.VoiceStates[i] = VoiceState.Default;
		}
	}
}