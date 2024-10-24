using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615
{
    public class OGameManagerBase : MBase
    { 
        [SerializeField] private MValue curGame;
        [SerializeField] private int gameIndex;
        
        public bool IsCurGame => curGame.SyncedValue == gameIndex;
    }
}