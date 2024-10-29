using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MVideoPlayerControllerUI : MBase
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
					videoPlayerControllerButtons[i].Init(this, i, videoPlayerController.VideoDatas[i].VideoName);
				else
					videoPlayerControllerButtons[i].gameObject.SetActive(false);
			}
		}

		public void PlayVideo(int index) => videoPlayerController.PlayVideo(index);
		public void StopVideo()
		{
			MDebugLog(nameof(StopVideo));
			videoPlayerController.StopVideo();
		}
		public void PauseVideo()
		{
			MDebugLog(nameof(PauseVideo));
			videoPlayerController.PauseResumeVideo();
		}
	}
}