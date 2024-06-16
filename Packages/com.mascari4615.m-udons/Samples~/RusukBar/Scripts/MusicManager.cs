using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components.Video;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class MusicManager : MBase
    {
        [Header("Sounds")] private AudioSource bgmAS;

        private CanvasManager[] canvasManagers;
        [SerializeField] private GameManager gameManager;
        private bool isLoading;
        private bool isPause;
        private int loopType;
        private int lyricsIndex;
        private MusicData[] musicDatas;
        private int musicIndex;

        private BaseVRCVideoPlayer musicPlayer;

        [FieldChangeCallback(nameof(VideoUrl))]
        private VRCUrl musicPlayerURL;

        private int musicPlaylistIndex;
        private MusicPlaylist[] musicPlaylists;
        private bool musicShuffle;

        public VRCUrl VideoUrl
        {
            set
            {
                musicPlayerURL = value;
                if (musicPlayer != null)
                    musicPlayer.PlayURL(musicPlayerURL);
            }
            get => musicPlayerURL;
        }

        private void Start()
        {
            canvasManagers = gameManager.canvasManagers;

            musicPlayer = GetComponent<BaseVRCVideoPlayer>();
            musicDatas = transform.Find("MusicDatas").GetComponentsInChildren<MusicData>();
            musicPlaylists = transform.Find("MusicPlaylists").GetComponentsInChildren<MusicPlaylist>();
            bgmAS = (AudioSource)transform.Find("AudioSource").GetComponent(typeof(AudioSource));

            foreach (var bgmData in musicDatas)
                bgmData.Init();

            foreach (var canvasManager in canvasManagers)
                canvasManager.SetMusicPlaylist(musicPlaylists[musicPlaylistIndex]);

            var firstMusicIndex = 0;
            SetMusicAndPlay(firstMusicIndex);
        }

        private void Update()
        {
            transform.SetPositionAndRotation(Networking.LocalPlayer.GetPosition(),
                Networking.LocalPlayer.GetRotation());

            if (isLoading)
            {
            }
            else if (musicPlayer.IsPlaying)
            {
                foreach (var canvas in gameManager.canvasManagers)
                    canvas.SetMusicUI_UpdateTime(musicPlayer.GetDuration(), musicPlayer.GetTime(),
                        musicPlayer.GetDuration());

                if (musicPlaylists[musicPlaylistIndex].musicDatas[musicIndex].lyrics == null)
                    return;

                if (lyricsIndex < musicPlaylists[musicPlaylistIndex].musicDatas[musicIndex].lyrics.Length - 1)
                    if (musicPlayer.GetTime() >= musicPlaylists[musicPlaylistIndex].musicDatas[musicIndex]
                            .lyricsTime[lyricsIndex + 1])
                    {
                        lyricsIndex++;
                        foreach (var canvas in gameManager.canvasManagers)
                            canvas.SetMusicUI_UpdateLyrics(lyricsIndex);
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
                        if (musicIndex == musicPlaylists[musicPlaylistIndex].musicDatas.Length - 1)
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
            foreach (var canvasManager in canvasManagers)
                canvasManager.SetMusicPlaylist(musicPlaylists[musicPlaylistIndex = index]);

            SetMusicAndPlay(0);
        }

        public void NextPlayList()
        {
            musicPlaylistIndex = (musicPlaylistIndex + 1) % musicPlaylists.Length;
            foreach (var canvasManager in canvasManagers)
                canvasManager.SetMusicPlaylist(musicPlaylists[musicPlaylistIndex]);

            SetMusicAndPlay(0);
        }

        public void PauseMusic()
        {
            if (musicPlayer.IsPlaying && isPause == false)
            {
                musicPlayer.Pause();
                isPause = true;
            }
            else
            {
                musicPlayer.Play();
                isPause = false;
            }

            gameManager.MusicPause(!musicPlayer.IsPlaying);
        }

        public void NextMusic()
        {
            if (isLoading)
                return;

            // do
            // {
            ++musicIndex;
            if (musicIndex == musicPlaylists[musicPlaylistIndex].musicDatas.Length) musicIndex = 0;
            // } while (musicToggleState[musicIndex] == false);
            // bgmAS.clip = bgmDatas[musicIndex % bgmDatas.Length].audioClip;
            PlayMusic();
        }

        public void PrevMusic()
        {
            if (isLoading)
                return;

            // do
            //{
            --musicIndex;
            if (musicIndex < 0) musicIndex = musicPlaylists[musicPlaylistIndex].musicDatas.Length - 1;
            // } while (musicToggleState[musicIndex] == false);
            // bgmAS.clip = bgmDatas[musicIndex % bgmDatas.Length].audioClip;
            PlayMusic();
        }

        private void RandomMusic()
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, musicPlaylists[musicPlaylistIndex].musicDatas.Length);
            } while (randomIndex == musicIndex /*|| musicToggleState[randomIndex] == false*/);

            SetMusicAndPlay(randomIndex);
        }

        public void MusicShuffleToggle()
        {
            musicShuffle = !musicShuffle;

            foreach (var canvas in gameManager.canvasManagers)
                canvas.SetMusicShuffleUI(musicShuffle);
        }

        public void SetMusicLoop(int loopType)
        {
            this.loopType = loopType;

            foreach (var canvas in gameManager.canvasManagers)
                canvas.SetMusicLoopUI(this.loopType);
        }

        public void MusicLoopSwitch()
        {
            if (loopType == 2) // None
            {
                loopType = 0; // All
                musicPlayer.Loop = false;
            }
            else if (loopType == 0) // All
            {
                loopType = 1; // One
                musicPlayer.Loop = true;
            }
            else // One
            {
                loopType = 2; // None
                musicPlayer.Loop = false;
            }

            foreach (var canvas in gameManager.canvasManagers)
                canvas.SetMusicLoopUI(loopType);
        }

        private void PlayMusic()
        {
            if (isLoading)
                return;

            // bgmAS.Play();
            musicPlayer.PlayURL(musicPlaylists[musicPlaylistIndex].musicDatas[musicIndex].url);

            isLoading = true;
            isPause = false;
            lyricsIndex = 0;

            foreach (var canvas in gameManager.canvasManagers)
                canvas.SetMusicUI_Init(
                    musicPlaylists[musicPlaylistIndex]
                        .musicDatas[musicIndex % musicPlaylists[musicPlaylistIndex].musicDatas.Length],
                    musicPlayer.GetDuration());
            // canvas.SetMusicUI_Init(bgmDatas[musicIndex % bgmDatas.Length].audioClip.name, bgmAS.clip.length, bgmDatas[musicIndex].albumCover);
        }

        public void SetMusicAndPlay(int index)
        {
            if (isLoading)
                return;

            // bgmAS.clip = bgmDatas[musicIndex = index].audioClip;
            musicIndex = index;
            PlayMusic();
        }

        public void ToggleMusic(int index)
        {
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
            Debug.Log("Kar - OnVideoReady");
            isLoading = false;
        }

        public override void OnVideoPlay()
        {
            Debug.Log("Kar -OnVideoPlay");
        }

        public override void OnVideoStart()
        {
            Debug.Log("Kar - OnVideoStart");
        }

        public override void OnVideoPause()
        {
            Debug.Log("Kar - OnVideoPause");
        }

        public override void OnVideoEnd()
        {
            Debug.Log("Kar - OnVideoEnd");
            NextMusic();
        }

        public override void OnVideoLoop()
        {
            Debug.Log("Kar - OnVideoLoop");
        }

        public override void OnVideoError(VideoError videoError)
        {
            Debug.Log($"Kar - OnVideoError : {videoError}");
        }
    }
}