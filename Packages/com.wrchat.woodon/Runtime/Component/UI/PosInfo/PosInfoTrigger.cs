using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// PosInfo 트리거 (영역에 들어가면 PosInfo에 위치 이름을 보내는 역할)
	// KarmoDDrine - 2025-09-30. 23:25
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class PosInfoTrigger : WBase
	{
		[Header("_" + nameof(PosInfoTrigger))]
		[SerializeField] private PosInfo posInfo;
		[SerializeField] private string posName;

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			WDebugLog($"{nameof(OnPlayerTriggerEnter)} : {player.displayName} | {player.playerId}");

			bool isLocalPlayer = player == Networking.LocalPlayer;

			if (isLocalPlayer == false)
				return;

			posInfo.ShowPosInfo(posName);
		}
	}
}