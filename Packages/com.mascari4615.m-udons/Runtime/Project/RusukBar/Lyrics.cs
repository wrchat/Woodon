using System;
using UdonSharp;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [Serializable]
    public class Lyrics : MBase
    {
        public int minute;
        public int seconds;
        public string text;
    }
}