using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class JoinBell : MBase
	{
		[Header("_" + nameof(JoinBell))]
		[SerializeField] private MSFXManager mSFXManager;

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			MDebugLog(nameof(OnPlayerJoined));

			if (player != null && player != Networking.LocalPlayer)
				mSFXManager.PlaySFX_L(0);
		}
	}
}