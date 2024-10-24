using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class JoinBell : MBase
	{
		[Header("_" + nameof(JoinBell))]
		[SerializeField] private MSFXManager mSFXManager;

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			MDebugLog(nameof(OnPlayerJoined));

			if (player == Networking.LocalPlayer)
				mSFXManager.PlaySFX_G(0);
		}
	}
}