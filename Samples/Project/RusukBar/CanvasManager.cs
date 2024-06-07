using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasManager : MBase
	{
		[SerializeField] private MusicButton[] musicButtons;
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

		[SerializeField] private Animator alarmAnimator;

		[SerializeField] private Image musicPlayButtonImage;
		[SerializeField] private Sprite[] musicPlayButtonSprites;
		[SerializeField] private Image standMikeActiveToggleImage;
		[SerializeField] private Image pianoToggleImage;
		[SerializeField] private Image drumToggleImage;
		[SerializeField] private Image guitarToggleImage;
		[SerializeField] private Image colliderToggleImage;
		[SerializeField] private Image bellToggleImage;
		[SerializeField] private Image postProcessToggleImage;
		[SerializeField] private Image musicToggleImage;
		[SerializeField] private Sprite[] cocktails;
		[SerializeField] private TextMeshProUGUI curSelectedUIText;

		[SerializeField] private bool isOverlayCanvas;
		[SerializeField] private Image cocktail;
		[SerializeField] private GameObject[] playListCurs;

		[SerializeField] private GameObject playListPanel;

		private string[] curMusicLyrics;

		// private UINavigation curUINavigation;
		private GameManager gameManager;

		private MusicManager musicManager;

		/*public UINavigation CurUINavigation
		{
			get => curUINavigation;
			set
			{
				if (curUINavigation != null)
					curUINavigation.SetSelect(false);
				curUINavigation = value;
				curUINavigation.SetSelect(true);
				curSelectedUIText.text = CurUINavigation.gameObject.name;
			}
		}*/

		private void Start()
		{
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();

			// Init();
		}

		/*private void Update()
		{
			if (isOverlayCanvas)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					SelectNearUINavigation(0);
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					SelectNearUINavigation(1);
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					SelectNearUINavigation(2);
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					SelectNearUINavigation(3);
				}
				else if (Input.GetKeyDown(KeyCode.Return))
				{
					if (CurUINavigation.gameObject.name.Contains("MusicButton"))
						CurUINavigation.OnClick(true);
					else
						CurUINavigation.OnClick(false);
				}
			}
		}*/

		public void Init()
		{
			if (isOverlayCanvas)
			{
				// CurUINavigation = defaultUINavi;
				// CurUINavigation.OnClick(false);
			}
		}

		/*public void SelectNearUINavigation(int direction)
		{
			var nextUI =
				direction == 0 && CurUINavigation.upper != null ? CurUINavigation.upper :
				direction == 1 && CurUINavigation.lower != null ? CurUINavigation.lower :
				direction == 2 && CurUINavigation.left != null ? CurUINavigation.left :
				direction == 3 && CurUINavigation.right != null ? CurUINavigation.right : null;

			if (nextUI == null)
			{
				Debug.Log("Error : Invalid CurUINavigation Pointer Reference");
				return;
			}

			CurUINavigation = nextUI;

			if (CurUINavigation.gameObject.name.Contains("MENU"))
				CurUINavigation.OnClick(false);
		}*/

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

			var maxItemListIndex = musicPlaylist.musicDatas.Length;
			for (var i = 0; i < musicButtons.Length; i++)
				if (i < maxItemListIndex)
				{
					musicButtons[i].gameObject.SetActive(true);
					musicButtons[i].Init(musicPlaylist.musicDatas[i]);
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
			var s = isAll ? "ALL" : "1";
			text.text = s;
			text.gameObject.SetActive(isOn);
		}

		public void PauseMusic()
		{
			musicManager.PauseMusic();
			gameManager.ButtonSFX();
		}

		public void UpdateMusicPlayIcon(bool isPaused)
		{
			musicPlayButtonImage.sprite = isPaused ? musicPlayButtonSprites[1] : musicPlayButtonSprites[0];
			musicToggleImage.color = MColorUtil.GetGreenOrRed(!isPaused);
		}

		public void NextMusic()
		{
			musicManager.NextMusic();
			gameManager.ButtonSFX();
		}

		public void PrevMusic()
		{
			musicManager.PrevMusic();
			gameManager.ButtonSFX();
		}

		public void MusicShuffleToggle()
		{
			musicManager.MusicShuffleToggle();
			gameManager.ButtonSFX();
		}

		public void SetMusicShuffleUI(bool musicShuffle)
		{
			musicShuffleToggleButtonImage.color = MColorUtil.GetWhiteOrGray(musicShuffle);
		}

		public void MusicLoopSwitch()
		{
			musicManager.MusicLoopSwitch();
			gameManager.ButtonSFX();
		}

		public void SetMusicLoopUI(int loopType)
		{
			if (loopType == 0) // None
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, true, true);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColor.White);
			}
			else if (loopType == 1) // All
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, false, true);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColor.White);
			}
			else // One
			{
				SetMusicLoopTypeImage(curMusicLoopTypeText, false, false);
				musicLoopSwitchButtonImage.color = MColorUtil.GetColor(MColor.Gray);
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

		public void SetMusicUI_Init(MusicData musicData, float musicDuration)
		{
			curMusicArtistText.text = musicData.artist;
			curMusicTitleText.text = musicData.gameObject.name;
			curMusicTimeText.text = "-:-";
			curMusicDurationText.text = "-:-";
			curMusicAlbumCover.sprite = musicData.albumCover;
			curMusicLyrics = musicData.lyrics;
			curMusicLyricsTexts[0].text = musicData.artist;
			curMusicLyricsTexts[1].text = musicData.gameObject.name;
			curMusicLyricsTexts[2].text = musicData.originalArtist;
			curMusicLyricsTexts[3].text = "";

			ChangeCocktail();
			alarmAnimator.SetBool("ISSHAKING", true);
		}

		public void ChangeCocktail()
		{
			cocktail.sprite = cocktails[Random.Range(0, cocktails.Length)];
		}

		[SerializeField] private Image musicCooltimeImage;
		[SerializeField] private float musicButtonCooltime = 5;
		private bool canMusic = true;

		public void MusicCooling()
		{
			canMusic = true;
			musicCooltimeImage.color = MColorUtil.GetGreenOrRed(canMusic);
		}

		public void PlayMusic(int index)
		{
			if (!canMusic)
				return;

			canMusic = false;
			musicCooltimeImage.color = MColorUtil.GetGreenOrRed(canMusic);
			SendCustomEventDelayedSeconds(nameof(MusicCooling), musicButtonCooltime);

			//for (var i = 0; i < playListCurs.Length; i++)
			//	playListCurs[i].SetActive(i == index);
			// CurUINavigation = uiNavi_SelectMusic;
			musicManager.SetMusicAndPlay(index);
			gameManager.ButtonSFX();
		}

		public void PlayPlayList(int index)
		{
			if (!canMusic)
				return;

			canMusic = false;
			musicCooltimeImage.color = MColorUtil.GetGreenOrRed(canMusic);
			SendCustomEventDelayedSeconds(nameof(MusicCooling), musicButtonCooltime);

			for (var i = 0; i < playListCurs.Length; i++)
				playListCurs[i].SetActive(i == index);
			musicManager.SelectPlayList(index);
			gameManager.ButtonSFX();
		}

		public void PlayPlayList0()
		{
			PlayPlayList(0);
		}

		public void PlayPlayList1()
		{
			PlayPlayList(1);
		}

		public void PlayPlayList2()
		{
			PlayPlayList(2);
		}

		public void PlayPlayList3()
		{
			PlayPlayList(3);
		}


		public void SwitchPlayList()
		{
			musicManager.NextPlayList();
			gameManager.ButtonSFX();
		}

		public void ResetAllPos()
		{
			gameManager.ResetAllPos();
			gameManager.ButtonSFX();
		}

		public void ResetCocktailPos()
		{
			gameManager.ResetCocktailPos();
			gameManager.ButtonSFX();
		}

		public void ResetPizzaPos()
		{
			gameManager.ResetPizzaPos();
			gameManager.ButtonSFX();
		}

		public void ResetChessPos()
		{
			gameManager.ResetChessPos();
			gameManager.ButtonSFX();
		}

		public void ResetPens_Global()
		{
			gameManager.ResetPens_Global();
			gameManager.ButtonSFX();
		}

		public void ResetPens()
		{
			gameManager.ResetPens();
			gameManager.ButtonSFX();
		}

		public void ClearPens_Global()
		{
			gameManager.ClearPens_Global();
			gameManager.ButtonSFX();
		}

		public void ClearPens()
		{
			gameManager.ClearPens();
			gameManager.ButtonSFX();
		}

		#region

		public void Play0()
		{
			PlayMusic(0);
		}

		public void Play1()
		{
			PlayMusic(1);
		}

		public void Play2()
		{
			PlayMusic(2);
		}

		public void Play3()
		{
			PlayMusic(3);
		}

		public void Play4()
		{
			PlayMusic(4);
		}

		public void Play5()
		{
			PlayMusic(5);
		}

		public void Play6()
		{
			PlayMusic(6);
		}

		public void Play7()
		{
			PlayMusic(7);
		}

		public void Play8()
		{
			PlayMusic(8);
		}

		public void Play9()
		{
			PlayMusic(9);
		}

		public void Play10()
		{
			PlayMusic(10);
		}

		public void Play11()
		{
			PlayMusic(11);
		}

		public void Play12()
		{
			PlayMusic(12);
		}

		public void Play13()
		{
			PlayMusic(13);
		}

		public void Play14()
		{
			PlayMusic(14);
		}

		public void Play15()
		{
			PlayMusic(15);
		}

		public void Play16()
		{
			PlayMusic(16);
		}

		public void Play17()
		{
			PlayMusic(17);
		}

		public void Play18()
		{
			PlayMusic(18);
		}

		public void Play19()
		{
			PlayMusic(19);
		}

		public void Play20()
		{
			PlayMusic(20);
		}

		public void Play21()
		{
			PlayMusic(21);
		}

		public void Play22()
		{
			PlayMusic(22);
		}

		public void Play23()
		{
			PlayMusic(23);
		}

		public void Play24()
		{
			PlayMusic(24);
		}

		public void Play25()
		{
			PlayMusic(25);
		}

		public void Play26()
		{
			PlayMusic(26);
		}

		public void Play27()
		{
			PlayMusic(27);
		}

		public void Play28()
		{
			PlayMusic(28);
		}

		public void Play29()
		{
			PlayMusic(29);
		}

		public void Play30()
		{
			PlayMusic(30);
		}

		public void Play31()
		{
			PlayMusic(31);
		}

		public void Play32()
		{
			PlayMusic(32);
		}

		public void Play33()
		{
			PlayMusic(33);
		}

		public void Play34()
		{
			PlayMusic(34);
		}

		public void Play35()
		{
			PlayMusic(35);
		}

		public void Play36()
		{
			PlayMusic(36);
		}

		public void Play37()
		{
			PlayMusic(37);
		}

		public void Play38()
		{
			PlayMusic(38);
		}

		public void Play39()
		{
			PlayMusic(39);
		}

		public void Play40()
		{
			PlayMusic(40);
		}

		public void Play41()
		{
			PlayMusic(41);
		}

		public void Play42()
		{
			PlayMusic(42);
		}

		public void Play43()
		{
			PlayMusic(43);
		}

		public void Play44()
		{
			PlayMusic(44);
		}

		public void Play45()
		{
			PlayMusic(45);
		}

		public void Play46()
		{
			PlayMusic(46);
		}

		public void Play47()
		{
			PlayMusic(47);
		}

		public void Play48()
		{
			PlayMusic(48);
		}

		public void Play49()
		{
			PlayMusic(49);
		}

		public void Play50()
		{
			PlayMusic(50);
		}

		public void Play51()
		{
			PlayMusic(51);
		}

		public void Play52()
		{
			PlayMusic(52);
		}

		public void Play53()
		{
			PlayMusic(53);
		}

		public void Play54()
		{
			PlayMusic(54);
		}

		public void Play55()
		{
			PlayMusic(55);
		}

		public void Play56()
		{
			PlayMusic(56);
		}

		public void Play57()
		{
			PlayMusic(57);
		}

		public void Play58()
		{
			PlayMusic(58);
		}

		public void Play59()
		{
			PlayMusic(59);
		}

		public void Play60()
		{
			PlayMusic(60);
		}

		public void Play61()
		{
			PlayMusic(61);
		}

		public void Play62()
		{
			PlayMusic(62);
		}

		public void Play63()
		{
			PlayMusic(63);
		}

		public void Play64()
		{
			PlayMusic(64);
		}

		public void Play65()
		{
			PlayMusic(65);
		}

		public void Play66()
		{
			PlayMusic(66);
		}

		public void Play67()
		{
			PlayMusic(67);
		}

		public void Play68()
		{
			PlayMusic(68);
		}

		public void Play69()
		{
			PlayMusic(69);
		}

		public void Play70()
		{
			PlayMusic(70);
		}

		public void Play71()
		{
			PlayMusic(71);
		}

		public void Play72()
		{
			PlayMusic(72);
		}

		public void Play73()
		{
			PlayMusic(73);
		}

		public void Play74()
		{
			PlayMusic(74);
		}

		public void Play75()
		{
			PlayMusic(75);
		}

		public void Play76()
		{
			PlayMusic(76);
		}

		public void Play77()
		{
			PlayMusic(77);
		}

		public void Play78()
		{
			PlayMusic(78);
		}

		public void Play79()
		{
			PlayMusic(79);
		}

		public void Play80()
		{
			PlayMusic(80);
		}

		public void Play81()
		{
			PlayMusic(81);
		}

		public void Play82()
		{
			PlayMusic(82);
		}

		public void Play83()
		{
			PlayMusic(83);
		}

		public void Play84()
		{
			PlayMusic(84);
		}

		public void Play85()
		{
			PlayMusic(85);
		}

		public void Play86()
		{
			PlayMusic(86);
		}

		public void Play87()
		{
			PlayMusic(87);
		}

		public void Play88()
		{
			PlayMusic(88);
		}

		public void Play89()
		{
			PlayMusic(89);
		}

		public void Play90()
		{
			PlayMusic(90);
		}

		public void Play91()
		{
			PlayMusic(91);
		}

		public void Play92()
		{
			PlayMusic(92);
		}

		public void Play93()
		{
			PlayMusic(93);
		}

		public void Play94()
		{
			PlayMusic(94);
		}

		public void Play95()
		{
			PlayMusic(95);
		}

		public void Play96()
		{
			PlayMusic(96);
		}

		public void Play97()
		{
			PlayMusic(97);
		}

		public void Play98()
		{
			PlayMusic(98);
		}

		public void Play99()
		{
			PlayMusic(99);
		}


		public void ToggleMusic0()
		{
			musicManager.ToggleMusic(0);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic1()
		{
			musicManager.ToggleMusic(1);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic2()
		{
			musicManager.ToggleMusic(2);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic3()
		{
			musicManager.ToggleMusic(3);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic4()
		{
			musicManager.ToggleMusic(4);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic5()
		{
			musicManager.ToggleMusic(5);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic6()
		{
			musicManager.ToggleMusic(6);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic7()
		{
			musicManager.ToggleMusic(7);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic8()
		{
			musicManager.ToggleMusic(8);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic9()
		{
			musicManager.ToggleMusic(9);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic10()
		{
			musicManager.ToggleMusic(10);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic11()
		{
			musicManager.ToggleMusic(11);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic12()
		{
			musicManager.ToggleMusic(12);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic13()
		{
			musicManager.ToggleMusic(13);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic14()
		{
			musicManager.ToggleMusic(14);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic15()
		{
			musicManager.ToggleMusic(15);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic16()
		{
			musicManager.ToggleMusic(16);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic17()
		{
			musicManager.ToggleMusic(17);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic18()
		{
			musicManager.ToggleMusic(18);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic19()
		{
			musicManager.ToggleMusic(19);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic20()
		{
			musicManager.ToggleMusic(20);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic21()
		{
			musicManager.ToggleMusic(21);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic22()
		{
			musicManager.ToggleMusic(22);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic23()
		{
			musicManager.ToggleMusic(23);
			gameManager.ButtonSFX();
		}

		public void ToggleMusic24()
		{
			musicManager.ToggleMusic(24);
			gameManager.ButtonSFX();
		}

		#endregion

		#region Setting

		public void ToggleGuitar()
		{
			gameManager.ToggleGuitar();
			gameManager.ButtonSFX();
		}

		public void SetGuitarToggleImage(bool actice)
		{
			guitarToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void ToggleDrum()
		{
			gameManager.ToggleDrum();
			gameManager.ButtonSFX();
		}

		public void SetDrumToggleImage(bool actice)
		{
			drumToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void TogglePiano()
		{
			gameManager.TogglePiano();
			gameManager.ButtonSFX();
		}

		public void SetPianoToggleImage(bool actice)
		{
			pianoToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void ToggleMikeActive()
		{
			gameManager.ToggleMike();
			gameManager.ButtonSFX();
		}

		public void SetMikeActiveToggleImage(bool actice)
		{
			standMikeActiveToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void ToggleBell()
		{
			gameManager.ToggleBell();
			gameManager.ButtonSFX();
		}

		public void SetBellToggleImage(bool actice)
		{
			bellToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void TogglePostProcess()
		{
			gameManager.TogglePostProcess();
			gameManager.ButtonSFX();
		}

		public void SetPostProcessToggleImage(bool actice)
		{
			postProcessToggleImage.color = MColorUtil.GetGreenOrRed(actice);
		}

		public void ToggleColliders()
		{
			gameManager.ToggleColliders();
			gameManager.ButtonSFX();
		}

		public void SetColliderToggleImage(bool enabled)
		{
			colliderToggleImage.color = MColorUtil.GetGreenOrRed(enabled);
		}

		#endregion

		public void TogglePlayList()
		{
			// CurUINavigation = uiNavi_SelectMusicDefault;
			playListPanel.SetActive(!playListPanel.activeSelf);
		}
	}
}