﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceReseter : VoiceUpdater
	{
		[Header("_" + nameof(VoiceReseter))]
		[SerializeField] private WPlayer[] ignoreTargets;

		[SerializeField] private bool useIngnoreTargetTag;
		[SerializeField] private VoiceTag IgnoreTargetTag;

		[SerializeField] private bool useTargetTag;
		[SerializeField] private VoiceTag targetTag;

		public override void UpdateVoice(VRCPlayerApi[] playerApis, VoiceState[] voiceStates)
		{
			if (IsNotOnline())
				return;

			if (playerApis == null)
				return;

			if (Enable == false)
				return;

			// 무시 WPlayer 대상이라면 return
			foreach (WPlayer ignoreTarget in ignoreTargets)
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
			for (int i = 0; i < playerApis.Length; i++)
				voiceStates[i] = VoiceState.Default;
		}
	}
}