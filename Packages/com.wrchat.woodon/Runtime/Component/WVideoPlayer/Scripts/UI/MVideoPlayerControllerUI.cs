using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MVideoPlayerControllerUI : WBase
	{
		[SerializeField] private Transform videoButtonsParent;
		private MVideoPlayerController videoPlayerController;

		public void Init(MVideoPlayerController videoPlayerController)
		{
			this.videoPlayerController = videoPlayerController;

			MVideoPlayerControllerButton[] videoPlayerControllerButtons = videoButtonsParent.GetComponentsInChildren<MVideoPlayerControllerButton>();
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