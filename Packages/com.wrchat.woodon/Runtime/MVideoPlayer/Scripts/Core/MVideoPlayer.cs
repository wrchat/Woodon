
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components.Video;
using VRC.SDK3.Video.Components;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	// https://github.com/MerlinVR/USharpVideo

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MVideoPlayer : MBase
	{
		private BaseVRCVideoPlayer _currentPlayer;

		[SerializeField] private VRCUnityVideoPlayer unityVideoPlayer;
		// public VRCAVProVideoPlayer avProPlayer;

		[SerializeField] private Renderer unityTextureRenderer;
		// public Renderer avProTextureRenderer;

		private MaterialPropertyBlock _fetchBlock;
		// private Material avproFetchMaterial;

		private void Start()
		{
			_currentPlayer = unityVideoPlayer;

			// Material m = unityTextureRenderer.material;
			// m = avProTextureRenderer.material;
			_fetchBlock = new MaterialPropertyBlock();
			// avproFetchMaterial = avProTextureRenderer.material;
		}

		public void Play() => _currentPlayer.Play();
		public void Pause() => _currentPlayer.Pause();
		public void PauseResume()
		{
			if (_currentPlayer.IsPlaying)
				_currentPlayer.Pause();
			else
				_currentPlayer.Play();
		}
		public void Stop() => _currentPlayer.Stop();
		public float GetTime() => _currentPlayer.GetTime();
		public float GetDuration() => _currentPlayer.GetDuration();
		public bool IsPlaying => _currentPlayer != null && _currentPlayer.IsPlaying;
		public void LoadURL(VRCUrl url) => _currentPlayer.LoadURL(url);
		public void PlayURL(VRCUrl url) => _currentPlayer.PlayURL(url);
		public void SetTime(float time) => _currentPlayer.SetTime(time);

		public Texture GetVideoTexture()
		{
			// if (_currentPlayer == unityVideoPlayer)
			{
				unityTextureRenderer.GetPropertyBlock(_fetchBlock);
				return _fetchBlock.GetTexture("_MainTex");
			}
			// else
			// {
			// 	return avproFetchMaterial.GetTexture("_MainTex");
			// }
		}

		public override void OnVideoEnd()
		{
		}

		public override void OnVideoError(VideoError videoError)
		{
		}

		public override void OnVideoLoop()
		{
		}

		public override void OnVideoPause()
		{
		}

		public override void OnVideoPlay()
		{
		}

		public override void OnVideoReady()
		{
		}

		public override void OnVideoStart()
		{
		}
	}
}