using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMusicPlayer : MBase
	{
		[Header("_" + nameof(UIMusicPlayer))]
		private MusicButton[] musicButtons;
		private UISelectPlayListButton[] playListButtons;
		[SerializeField] private Animator alarmAnimator;
		[SerializeField] private Sprite[] cocktails;
		[SerializeField] private Image cocktail;
		[SerializeField] private TextMeshProUGUI curMusicTitleText;
		[SerializeField] private TextMeshProUGUI[] curMusicLyricsTexts;
		[SerializeField] private TextMeshProUGUI curMusicDurationText;
		[SerializeField] private TextMeshProUGUI curMusicTimeText;
		[SerializeField] private TextMeshProUGUI curMusicArtistText;
		[SerializeField] private Image musicLoopSwitchButtonImage;
		[SerializeField] private TextMeshProUGUI curMusicLoopTypeText;
		[SerializeField] private Image musicShuffleToggleButtonImage;
		[SerializeField] private Image curMusicAlbumCover;
		[SerializeField] private Image curMusicTimeBar;
		[SerializeField] private Animator curMusicLyricsAnimator;
		[SerializeField] private Image[] cooltimeUIs;

		[SerializeField] private Image musicPlayButtonImage;
		[SerializeField] private Sprite[] musicPlayButtonSprites;
		private string[] curMusicLyrics;
		private MusicManager musicManager;

		public void Init(MusicManager musicManager)
		{
			this.musicManager = musicManager;

			musicButtons = GetComponentsInChildren<MusicButton>(true);
			playListButtons = GetComponentsInChildren<UISelectPlayListButton>(true);

			for (int i = 0; i < musicButtons.Length; i++)
				musicButtons[i].Init(this, i);
			for (int i = 0; i < playListButtons.Length; i++)
				playListButtons[i].Init(this, i);
		}

		public void SetMusicUI_Init(MusicData musicData, float musicDuration)
		{
			curMusicArtistText.text = musicData.Artist;
			curMusicTitleText.text = musicData.gameObject.name;
			curMusicTimeText.text = "-:-";
			curMusicDurationText.text = "-:-";
			curMusicAlbumCover.sprite = musicData.AlbumCover;
			curMusicLyrics = musicData.Lyrics;
			curMusicLyricsTexts[0].text = musicData.Artist;
			curMusicLyricsTexts[1].text = musicData.gameObject.name;
			curMusicLyricsTexts[2].text = musicData.OriginalArtist;
			curMusicLyricsTexts[3].text = "";

			ChangeCocktail();
			alarmAnimator.SetBool("ISSHAKING", true);
		}

		public void ChangeCocktail()
		{
			cocktail.sprite = cocktails[Random.Range(0, cocktails.Length)];
		}

		public void SetMusicPlaylist(MusicPlaylist musicPlaylist)
		{
			/*if (musicButtonUINavigations == null)
			{
				musicButtonUINavigations = new UINavigation[musicButtons.Length];
				for (var i = 0; i < musicButtons.Length; i++)
				{
					musicButtonUINavigations[i] = musicButtons[i].GetComponent<UINavigation>();
					musicButtonUINavigations[i].Init(i);
				}
			}*/

			int maxItemListIndex = musicPlaylist.MusicDatas.Length;
			for (int i = 0; i < musicButtons.Length; i++)
				if (i < maxItemListIndex)
				{
					musicButtons[i].gameObject.SetActive(true);
					musicButtons[i].SetMusicData(musicPlaylist.MusicDatas[i]);
					/*musicButtonUINavigations[i].upper =
						musicButtonUINavigations[(i + maxItemListIndex - 1) % maxItemListIndex];
					musicButtonUINavigations[i].lower = musicButtonUINavigations[(i + 1) % maxItemListIndex];

					if (maxItemListIndex >= 20)
					{
						musicButtonUINavigations[i].left =
							musicButtonUINavigations[(i + maxItemListIndex - 20) % maxItemListIndex];
						musicButtonUINavigations[i].right = musicButtonUINavigations[(i + 20) % maxItemListIndex];
					}

					if (i < 20) musicButtonUINavigations[i].left = uiNavi_SelectMusicCancel;*/
				}
				else
				{
					musicButtons[i].gameObject.SetActive(false);
				}
		}

		private void SetMusicLoopTypeImage(TextMeshProUGUI text, bool isAll, bool isOn)
		{
			string s = isAll ? "ALL" : "1";
			text.text = s;
			text.gameObject.SetActive(isOn);
		}

		public void PauseMusic() => musicManager.PauseMusic();
		public void NextMusic() => musicManager.NextMusic();
		public void PrevMusic() => musicManager.PrevMusic();
		public void MusicShuffleToggle() => musicManager.MusicShuffleToggle();

		public void SetMusicShuffleUI(bool musicShuffle)
		{
			musicShuffleToggleButtonImage.color = MColorUtil.GetWhiteOrGray(musicShuffle);
		}

		public void MusicLoopSwitch() => musicManager.MusicLoopSwitch();

		public void SetMusicLoopUI(int loopType)
		{
			if (loopType == 0) // None
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, true, true);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColorPreset.White);
			}
			else if (loopType == 1) // All
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, false, true);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColorPreset.White);
			}
			else // One
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, false, false);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColorPreset.Gray);
			}
		}

		public void SetMusicUI_UpdateTime(float clipLength, float curPlayTime, float musicDuration)
		{
			curMusicTimeBar.fillAmount = curPlayTime / clipLength;
			curMusicTimeText.text = $"{(int)curPlayTime / 60}:{(int)curPlayTime % 60}";
			curMusicDurationText.text = $"{(int)musicDuration / 60}:{(int)musicDuration % 60}";

			alarmAnimator.SetBool("ISSHAKING", false);
		}

		public void SetMusicUI_UpdateLyrics(int lyricsIndex)
		{
			curMusicLyricsTexts[0].text = lyricsIndex > 1 ? curMusicLyrics[lyricsIndex - 2] : "";
			curMusicLyricsTexts[1].text = lyricsIndex > 0 ? curMusicLyrics[lyricsIndex - 1] : "";
			curMusicLyricsTexts[2].text = curMusicLyrics[lyricsIndex];
			curMusicLyricsTexts[3].text =
				lyricsIndex < curMusicLyrics.Length - 1 ? curMusicLyrics[lyricsIndex + 1] : "";

			curMusicLyricsAnimator.SetTrigger("NEXT");
		}
		
		public void PlayMusic(int index)
		{
			if (musicManager.Cooling)
				return;

			//for (var i = 0; i < playListCurs.Length; i++)
			//	playListCurs[i].SetActive(i == index);
			// CurUINavigation = uiNavi_SelectMusic;
			musicManager.SetMusicAndPlay(index);
		}

		public void SelectPlayList(int index)
		{
			if (musicManager.Cooling)
				return;

			for (int i = 0; i < playListButtons.Length; i++)
				playListButtons[i].UpdateUI(musicManager.MusicPlaylists[i], index);

			musicManager.SelectPlayList(index);
		}

		public void SwitchPlayList() => musicManager.NextPlayList();
		public void ToggleMusic(int index) => musicManager.ToggleMusic(index);

		public void UpdateUI()
		{
			foreach (Image cooltimeUI in cooltimeUIs)
			{
				// cooltimeUI.color = MColorUtil.GetGreenOrRed(musicManager.Cooling);
				cooltimeUI.gameObject.SetActive(musicManager.Cooling);
			}

			musicPlayButtonImage.sprite = musicPlayButtonSprites[musicManager.IsPlaying ? 1 : 0];
		}
	}
}