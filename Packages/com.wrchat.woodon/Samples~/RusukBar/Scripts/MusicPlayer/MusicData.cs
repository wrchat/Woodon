using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MusicData : MBase
	{
		[field: SerializeField] public VRCUrl Url { get; private set; }
		[field: SerializeField] public Sprite AlbumCover { get; private set; }
		[field: SerializeField] public string Artist { get; private set; }
		[field: SerializeField] public string OriginalArtist { get; private set; }
		[field: SerializeField] public string[] Lyrics { get; private set; }
		[field: SerializeField] public int[] LyricsTime { get; private set; }

		[SerializeField] private TextAsset lyricsTextFile;

		public void Init()
		{
			Lyrics = null;
			LyricsTime = null;
			if (lyricsTextFile == null)
				return;
			string lyricsString = lyricsTextFile.ToString();
			string[] lyricsArr = lyricsString.Split('\n');

			Lyrics = new string[lyricsArr.Length];
			LyricsTime = new int[lyricsArr.Length];

			for (int i = 0; i < lyricsArr.Length; i++)
			{
				string[] lyricsDatas = lyricsArr[i].Split('\t');
				LyricsTime[i] = int.Parse(lyricsDatas[0]);
				Lyrics[i] = lyricsDatas.Length > 1 ? lyricsDatas[1] : "";
			}
		}
	}
}