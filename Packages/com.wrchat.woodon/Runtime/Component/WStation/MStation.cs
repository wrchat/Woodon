using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MStation : MTarget
	{
		[Header("_" + nameof(MStation))]
		[SerializeField] private VRCStation station;

		protected override void OnTargetChange(DataChangeState changeState)
		{
			base.OnTargetChange(changeState);

			if (IsTargetPlayer())
				UseStation();
			else if (IsTargetPlayer() == false)
				ExitStation();
		}

		[ContextMenu(nameof(UseStation))]
		public void UseStation()
		{
			station.UseStation(Networking.LocalPlayer);
		}

		[ContextMenu(nameof(ExitStation))]
		public void ExitStation()
		{
			station.ExitStation(Networking.LocalPlayer);
		}

		[ContextMenu(nameof(ToggleStation))]
		public void ToggleStation()
		{
			if (TargetPlayerID == NONE_INT)
				UseStation();
			else if (IsTargetPlayer())
				ExitStation();
		}

		public override void OnPlayerRespawn(VRCPlayerApi player)
		{
			if (IsTargetPlayer(player))
				ResetPlayer();
		}
	}
}