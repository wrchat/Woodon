using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MusicData : MBase
    {
        public VRCUrl url;
        public Sprite albumCover;
        public string artist;
        public string originalArtist = "";
        [SerializeField] private TextAsset lyricsTextFile;
        [HideInInspector] public string[] lyrics;
        [HideInInspector] public int[] lyricsTime;

        public void Init()
        {
            lyrics = null;
            lyricsTime = null;
            if (lyricsTextFile == null)
                return;
            var lyricsString = lyricsTextFile.ToString();
            var lyricsArr = lyricsString.Split('\n');

            lyrics = new string[lyricsArr.Length];
            lyricsTime = new int[lyricsArr.Length];

            for (var i = 0; i < lyricsArr.Length; i++)
            {
                var lyricsDatas = lyricsArr[i].Split('\t');
                lyricsTime[i] = int.Parse(lyricsDatas[0]);
                lyrics[i] = lyricsDatas.Length > 1 ? lyricsDatas[1] : "";
            }
        }
    }
}