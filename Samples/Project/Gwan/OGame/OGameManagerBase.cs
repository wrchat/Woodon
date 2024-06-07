using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
    public class OGameManagerBase : MBase
    { 
        [SerializeField] private MScore curGame;
        [SerializeField] private int gameIndex;
        
        public bool IsCurGame => curGame.SyncedScore == gameIndex;
    }
}