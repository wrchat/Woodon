using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WVideoPlayerController : WBase
	{
		[SerializeField] private WVideoPlayer wVideoPlayer;
		[SerializeField] private Transform videoDatasParent;
		[SerializeField] private WVideoPlayerControllerUI[] UIs;

		public VideoData[] VideoDatas
		{
			get
			{
				if (_videoDatas == null || _videoDatas.Length == 0)
					_videoDatas = videoDatasParent.GetComponentsInChildren<VideoData>();

				return _videoDatas;
			}
		}
		private VideoData[] _videoDatas = null;

		private void Start()
		{
			InitUI();
		}

		private void InitUI()
		{
			foreach (WVideoPlayerControllerUI ui in UIs)
				ui.Init(this);
		}

		public virtual void PlayVideo(int index) => wVideoPlayer.PlayURL(VideoDatas[index].VRCUrl);

		#region HorribleEvents
		[ContextMenu(nameof(PlayVideo0))]
		public void PlayVideo0() => PlayVideo(0);
		public void PlayVideo1() => PlayVideo(1);
		public void PlayVideo2() => PlayVideo(2);

		[ContextMenu(nameof(StopVideo))]
		public virtual void StopVideo() => wVideoPlayer.Stop();

		[ContextMenu(nameof(PauseResumeVideo))]
		public virtual void PauseResumeVideo() => wVideoPlayer.PauseResume();
		#endregion
	}
}