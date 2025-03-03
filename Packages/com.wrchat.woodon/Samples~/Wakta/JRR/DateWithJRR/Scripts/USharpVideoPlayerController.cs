using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;
using UdonSharp.Video;

public class USharpVideoPlayerController : WVideoPlayerController
{
	[SerializeField] private USharpVideoPlayer videoPlayer;

	public override void PlayVideo(int index)
	{
		videoPlayer.PlayVideo(VideoDatas[index].VRCUrl);
	}

	public override void StopVideo()
	{
		videoPlayer.StopVideo();
	}

	public override void PauseResumeVideo()
	{
		videoPlayer.SetPaused(!videoPlayer.IsPaused());
	}
}
