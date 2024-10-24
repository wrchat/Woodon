using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MStation : MBase
	{
		[Header("_" + nameof(MStation))]
		[SerializeField] private VRCStation station;
		[field: UdonSynced] public int OwnerID { get; private set; } = NONE_INT;
		
		public bool IsLocalPlayerOwner => OwnerID == Networking.LocalPlayer.playerId;

		public void UseStation()
		{
			if (OwnerID != NONE_INT)
				return;

			if (IsLocalPlayerOwner == true)
				return;

			SetOwner();
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();

			station.UseStation(Networking.LocalPlayer);
		}

		public void ExitStation()
		{
			if (OwnerID == NONE_INT)
				return;

			if (IsLocalPlayerOwner == false)
				return;

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();

			station.ExitStation(Networking.LocalPlayer);
		}
	}
}