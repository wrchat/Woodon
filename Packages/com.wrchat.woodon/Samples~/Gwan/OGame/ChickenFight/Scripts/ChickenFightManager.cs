using UdonSharp;
using UnityEngine;
using WakVRC;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ChickenFightManager : OGameManagerBase
    {
        public Transform respawnPos;
    }
}