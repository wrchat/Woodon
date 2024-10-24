using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components.Video;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.RusukBar
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MusicManager : MBase
	{
		[Header("Sounds")]
		[SerializeField] private UIMusicPlayer[] uis;
		private bool isLoading;
		private bool isPause;
		private int loopType;
		private int lyricsIndex;
		public MusicData[] MusicDatas { get; private set; }
		public MusicPlaylist[] MusicPlaylists { get; private set; }

		private BaseVRCVideoPlayer videoPlayer;

		private int musicIndex = 0;
		private int musicPlaylistIndex = 0;
		private bool musicShuffle;

		[FieldChangeCallback(nameof(MusicURL))] private VRCUrl musicURL;
		public VRCUrl MusicURL
		{
			set
			{
				musicURL = value;
				if (videoPlayer != null)
					videoPlayer.PlayURL(musicURL);
			}
			get => musicURL;
		}

		[SerializeField] private float musicButtonCooltime = 5;
		public bool Cooling { get; private set; } = false;
		public bool IsPlaying => videoPlayer.IsPlaying;
		public void MusicCooling()
		{
			Cooling = false;
		}
		
		private void Start() => Init();

		private void Init()
		{
			MDebugLog(nameof(Init));

			videoPlayer = GetComponent<BaseVRCVideoPlayer>();

			MusicDatas = transform.GetComponentsInChildren<MusicData>();
			foreach (MusicData musicData in MusicDatas)
				musicData.Init();
				
			MusicPlaylists = transform.GetComponentsInChildren<MusicPlaylist>();

			foreach (UIMusicPlayer ui in uis)
			{
				ui.Init(this);
				ui.SetMusicPlaylist(MusicPlaylists[musicPlaylistIndex]);
			}

			int firstMusicIndex = 0;
			SetMusicAndPlay(firstMusicIndex);
		}

		private void Update()
		{
			if (MUtil.IsNotOnline())
				return;

			transform.SetPositionAndRotation(Networking.LocalPlayer.GetPosition(),
				Networking.LocalPlayer.GetRotation());

			foreach (UIMusicPlayer ui in uis)
				ui.UpdateUI();

			if (isLoading)
			{
			}
			else if (videoPlayer.IsPlaying)
			{
				foreach (UIMusicPlayer ui in uis)
					ui.SetMusicUI_UpdateTime(videoPlayer.GetDuration(), videoPlayer.GetTime(),
						videoPlayer.GetDuration());

				if (MusicPlaylists[musicPlaylistIndex].MusicDatas[musicIndex].Lyrics == null)
					return;

				if (lyricsIndex < MusicPlaylists[musicPlaylistIndex].MusicDatas[musicIndex].Lyrics.Length - 1)
					if (videoPlayer.GetTime() >= MusicPlaylists[musicPlaylistIndex].MusicDatas[musicIndex]
							.LyricsTime[lyricsIndex + 1])
					{
						lyricsIndex++;
						foreach (UIMusicPlayer ui in uis)
							ui.SetMusicUI_UpdateLyrics(lyricsIndex);
					}
			}
			else if (isPause == false)
			{
				switch (loopType)
				{
					case 0: // All
						if (musicShuffle)
							RandomMusic();
						else
							NextMusic();
						break;

					case 1: // One
							// PlayMusic();
						break;

					case 2: // None
						if (musicIndex == MusicPlaylists[musicPlaylistIndex].MusicDatas.Length - 1)
						{
						}
						else if (musicShuffle)
						{
							RandomMusic();
						}
						else
						{
							NextMusic();
						}

						break;
				}
			}
		}

		public void SelectPlayList(int index)
		{
			MDebugLog($"{nameof(SelectPlayList)} : {index}");

			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicPlaylist(MusicPlaylists[musicPlaylistIndex = index]);

			SetMusicAndPlay(0);
		}

		public void NextPlayList()
		{
			MDebugLog($"{nameof(NextPlayList)}");

			musicPlaylistIndex = (musicPlaylistIndex + 1) % MusicPlaylists.Length;
			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicPlaylist(MusicPlaylists[musicPlaylistIndex]);

			SetMusicAndPlay(0);
		}

		public void PauseMusic()
		{
			MDebugLog($"{nameof(PauseMusic)}");

			if (videoPlayer.IsPlaying && isPause == false)
			{
				videoPlayer.Pause();
				isPause = true;
			}
			else
			{
				videoPlayer.Play();
				isPause = false;
			}

			// foreach (CanvasManager canvas in canvasManagers)
			// 	canvas.UpdateMusicPlayIcon(!musicPlayer.IsPlaying);
		}

		public void NextMusic()
		{
			MDebugLog($"{nameof(NextMusic)}");

			if (isLoading)
				return;

			// do
			// {
			++musicIndex;
			if (musicIndex == MusicPlaylists[musicPlaylistIndex].MusicDatas.Length) musicIndex = 0;
			// } while (musicToggleState[musicIndex] == false);
			// bgmAS.clip = bgmDatas[musicIndex % bgmDatas.Length].audioClip;
			PlayMusic();
		}

		public void PrevMusic()
		{
			MDebugLog($"{nameof(PrevMusic)}");

			if (isLoading)
				return;

			// do
			//{
			--musicIndex;
			if (musicIndex < 0) musicIndex = MusicPlaylists[musicPlaylistIndex].MusicDatas.Length - 1;
			// } while (musicToggleState[musicIndex] == false);
			// bgmAS.clip = bgmDatas[musicIndex % bgmDatas.Length].audioClip;
			PlayMusic();
		}

		private void RandomMusic()
		{
			MDebugLog($"{nameof(RandomMusic)}");

			int randomIndex;
			do
			{
				randomIndex = Random.Range(0, MusicPlaylists[musicPlaylistIndex].MusicDatas.Length);
			} while (randomIndex == musicIndex /*|| musicToggleState[randomIndex] == false*/);

			SetMusicAndPlay(randomIndex);
		}

		public void MusicShuffleToggle()
		{
			MDebugLog($"{nameof(MusicShuffleToggle)}");

			musicShuffle = !musicShuffle;

			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicShuffleUI(musicShuffle);
		}

		public void SetMusicLoop(int loopType)
		{
			MDebugLog($"{nameof(SetMusicLoop)} : {loopType}");

			this.loopType = loopType;

			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicLoopUI(this.loopType);
		}

		public void MusicLoopSwitch()
		{
			MDebugLog($"{nameof(MusicLoopSwitch)}");

			if (loopType == 2) // None
			{
				loopType = 0; // All
				videoPlayer.Loop = false;
			}
			else if (loopType == 0) // All
			{
				loopType = 1; // One
				videoPlayer.Loop = true;
			}
			else // One
			{
				loopType = 2; // None
				videoPlayer.Loop = false;
			}

			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicLoopUI(loopType);
		}

		private void PlayMusic()
		{
			MDebugLog($"{nameof(PlayMusic)}");

			if (isLoading || Cooling)
				return;

			// bgmAS.Play();
			videoPlayer.PlayURL(MusicPlaylists[musicPlaylistIndex].MusicDatas[musicIndex].Url);

			Cooling = true;
			SendCustomEventDelayedSeconds(nameof(MusicCooling), musicButtonCooltime);
			isLoading = true;
			isPause = false;
			lyricsIndex = 0;

			foreach (UIMusicPlayer ui in uis)
				ui.SetMusicUI_Init(
				MusicPlaylists[musicPlaylistIndex]
					.MusicDatas[musicIndex % MusicPlaylists[musicPlaylistIndex].MusicDatas.Length],
				videoPlayer.GetDuration());
			// canvas.SetMusicUI_Init(bgmDatas[musicIndex % bgmDatas.Length].audioClip.name, bgmAS.clip.length, bgmDatas[musicIndex].albumCover);
		}

		public void SetMusicAndPlay(int index)
		{
			MDebugLog($"{nameof(SetMusicAndPlay)} : {index}");

			if (isLoading)
				return;

			// bgmAS.clip = bgmDatas[musicIndex = index].audioClip;
			musicIndex = index;
			PlayMusic();
		}

		public void ToggleMusic(int index)
		{
			MDebugLog($"{nameof(ToggleMusic)} : {index}");

			/*
            int curOnMusicCount = 0;

            foreach (var musicToggle in musicToggleState)
                if (musicToggle == true)
                    curOnMusicCount++;

            if (curOnMusicCount == 1 && musicToggleState[index])
                return;

            musicToggleState[index] = !musicToggleState[index];

            foreach (var canvas in gameManager.canvasManagers)
                canvas.SetMusicButton(index, musicToggleState[index]);
                */
		}

		public override void OnVideoReady()
		{
			MDebugLog($"{nameof(OnVideoReady)}");
			isLoading = false;
		}

		public override void OnVideoPlay()
		{
			MDebugLog($"{nameof(OnVideoPlay)}");
		}

		public override void OnVideoStart()
		{
			MDebugLog($"{nameof(OnVideoStart)}");
		}

		public override void OnVideoPause()
		{
			MDebugLog($"{nameof(OnVideoPause)}");
		}

		public override void OnVideoEnd()
		{
			MDebugLog($"{nameof(OnVideoEnd)}");
			NextMusic();
		}

		public override void OnVideoLoop()
		{
			MDebugLog($"{nameof(OnVideoLoop)}");
		}

		public override void OnVideoError(VideoError videoError)
		{
			MDebugLog($"{nameof(OnVideoError)} : {videoError}");
		}
	}
}