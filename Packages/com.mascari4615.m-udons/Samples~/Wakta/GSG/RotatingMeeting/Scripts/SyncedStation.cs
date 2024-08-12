using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SyncedStation : MBase
	{
		[Header("_" + nameof(SyncedStation))]
		[SerializeField] private VRCStation targetStation;
		[field: UdonSynced] public int OwnerID { get; private set; } = NONE_INT;

		public void UseStation()
		{
			if (OwnerID != NONE_INT)
				return;

			SetOwner();
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();

			targetStation.UseStation(Networking.LocalPlayer);
		}

		public void ExitStation()
		{
			if (OwnerID == NONE_INT)
				return;

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();

			targetStation.ExitStation(Networking.LocalPlayer);
		}
	}
}