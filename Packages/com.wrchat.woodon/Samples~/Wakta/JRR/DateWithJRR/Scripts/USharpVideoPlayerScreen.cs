using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Video.Components;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDKBase;
using VRC.Udon;

public class USharpVideoPlayerScreen : UdonSharpBehaviour
{
	[SerializeField] private VRCUnityVideoPlayer unityVideoPlayer;
	[SerializeField] private VRCAVProVideoPlayer avProPlayer;
	[SerializeField] private VideoPlayerManager videoPlayerManager;
	[SerializeField] private RawImage[] rawImages;
	[SerializeField] private bool hideVideoWhenIsNotPlaying = false;

	private void Update()
	{
		UpdateScreen();
	}

	public void UpdateScreen()
	{
		foreach (RawImage rawImage in rawImages)
		{
			bool isUnityVideoPlayerPlaying = unityVideoPlayer != null && unityVideoPlayer.IsPlaying;
			bool isAVProPlayerPlaying = avProPlayer != null && avProPlayer.IsPlaying;

			// init 전에 IsPlaying 호출하면 null exception 발생
			// if (videoPlayerManager.IsPlaying())
			if (isUnityVideoPlayerPlaying || isAVProPlayerPlaying)
			{
				rawImage.texture = videoPlayerManager.GetVideoTexture();
				rawImage.color = Color.white;
			}
			else if (hideVideoWhenIsNotPlaying)
			{
				rawImage.texture = null;
				rawImage.color = Color.clear;
			}
			else
			{
				rawImage.texture = null;
				rawImage.color = Color.black;
			}
		}
	}
}
