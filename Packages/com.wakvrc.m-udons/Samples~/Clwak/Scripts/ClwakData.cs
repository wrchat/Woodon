using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ClwakData : UdonSharpBehaviour
    {
        public int Hour;
        public int Minute;
        public string data;
        public Sprite[] sprite;
        public Color color = Color.white;
    }
}