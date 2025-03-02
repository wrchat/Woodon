using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WVideoPlayerControllerUI : WBase
	{
		[SerializeField] private Transform videoButtonsParent;
		private WVideoPlayerController videoPlayerController;

		public void Init(WVideoPlayerController videoPlayerController)
		{
			this.videoPlayerController = videoPlayerController;

			WVideoPlayerControllerButton[] videoPlayerControllerButtons = videoButtonsParent.GetComponentsInChildren<WVideoPlayerControllerButton>();
			for (int i = 0; i < videoPlayerControllerButtons.Length; i++)
			{
				if (i < videoPlayerController.VideoDatas.Length)
					videoPlayerControllerButtons[i].Init(this, i, videoPlayerController.VideoDatas[i].Name);
				else
					videoPlayerControllerButtons[i].gameObject.SetActive(false);
			}
		}

		public void PlayVideo(int index) => videoPlayerController.PlayVideo(index);
		public void StopVideo()
		{
			WDebugLog(nameof(StopVideo));
			videoPlayerController.StopVideo();
		}
		public void PauseVideo()
		{
			WDebugLog(nameof(PauseVideo));
			videoPlayerController.PauseResumeVideo();
		}
	}
}