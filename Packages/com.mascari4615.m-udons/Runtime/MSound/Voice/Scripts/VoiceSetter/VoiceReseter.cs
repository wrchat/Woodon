using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceReseter : VoiceUpdater
	{
		[SerializeField] private MTarget[] ignoreTargets;

		[SerializeField] private bool useIngnoreTargetTag;
		[SerializeField] private VoiceAreaTag IgnoreTargetTag;

		public override void UpdateVoice()
		{
			if (NotOnline)
				return;

			if (voiceManager.PlayerApis == null)
				return;

			// 타겟이라면 무시
			foreach (MTarget ignoreTarget in ignoreTargets)
			{
				if (ignoreTarget.IsTargetPlayer(Networking.LocalPlayer))
					return;
			}

			// 타겟 태그를 가지고 있으면 무시
			if (useIngnoreTargetTag)
			{
				string tag =
					Networking.LocalPlayer.GetPlayerTag($"{Networking.LocalPlayer.playerId}{IgnoreTargetTag}");

				if (tag == TRUE_STRING)
					return;
			}

			// 보이스 상태 초기화
			for (int i = 0; i < voiceManager.PlayerApis.Length; i++)
				voiceManager.VoiceStates[i] = VoiceState.Default;
		}
	}
}