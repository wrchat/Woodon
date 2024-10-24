using TMPro;
using UdonSharp;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MusicButton : MBase
	{
		private UIMusicPlayer musicPlayerUI;
		private int index;

		private Image albumCover;
		private TextMeshProUGUI artist;
		private TextMeshProUGUI title;

		public void Init(UIMusicPlayer musicPlayerUI, int index)
		{
			this.musicPlayerUI = musicPlayerUI;
			this.index = index;

			title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			artist = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
			albumCover = transform.GetChild(2).GetChild(0).GetComponent<Image>();
		}

		public void SetMusicData(MusicData musicData)
		{
			title.text = musicData.gameObject.name;
			artist.text = musicData.Artist;
			albumCover.sprite = musicData.AlbumCover;
		}

		public void Click()
		{
			musicPlayerUI.PlayMusic(index);
		}
	}
}