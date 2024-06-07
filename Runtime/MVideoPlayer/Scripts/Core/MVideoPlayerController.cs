using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MVideoPlayerController : MBase
	{
		[SerializeField] private MVideoPlayer mVideoPlayer;
		[SerializeField] private Transform videoDatasParent;
		[SerializeField] private MVideoPlayerControllerUI[] UIs;

		public VideoData[] VideoDatas
		{
			get
			{
				if (_videoDatas == null)
					_videoDatas = videoDatasParent.GetComponentsInChildren<VideoData>();

				return _videoDatas;
			}
		}
		private VideoData[] _videoDatas;

		private void Start()
		{
			InitUI();
		}

		private void InitUI()
		{
			foreach (var ui in UIs)
				ui.Init(this);
		}

		public void PlayVideo(int index) => mVideoPlayer.PlayURL(VideoDatas[index].VRCUrl);

		[ContextMenu(nameof(PlayVideo0))]
		public void PlayVideo0() => PlayVideo(0);
		public void PlayVideo1() => PlayVideo(1);
		public void PlayVideo2() => PlayVideo(2);

		public void StopVideo()
		{
			MDebugLog(nameof(StopVideo));
			mVideoPlayer.Stop();
		}

		public void PauseResumeVideo()
		{
			MDebugLog(nameof(PauseResumeVideo));
			mVideoPlayer.PauseResume();
		}
	}
}