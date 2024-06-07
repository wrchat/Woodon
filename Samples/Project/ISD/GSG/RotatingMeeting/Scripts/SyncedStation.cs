using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedStation : MBase
    {
        public int OwnerID => ownerID;
        [UdonSynced] private int ownerID = NONE_INT;
        [SerializeField] private VRCStation station;

        public void UseStation()
        {
            if (ownerID != NONE_INT)
                return;

            SetOwner();
            ownerID = Networking.LocalPlayer.playerId;
            RequestSerialization();
            
            station.UseStation(Networking.LocalPlayer);
        }

        public void ExitStation()
        {
            if (ownerID == NONE_INT)
                return;

            SetOwner();
            ownerID = NONE_INT;
            RequestSerialization();
            
            station.ExitStation(Networking.LocalPlayer);
        }
    }
}