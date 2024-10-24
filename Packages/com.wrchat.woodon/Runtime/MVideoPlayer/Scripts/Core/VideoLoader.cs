using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using VRC.SDK3.Video.Components;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class VideoLoader : MBase
	{
		[SerializeField] private VRCUnityVideoPlayer unityVideoPlayer;
		[SerializeField] private VRCUrl videoURL;

		public void LoadVideo_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LoadVideo));

		public void LoadVideo()
		{
			MDebugLog(nameof(LoadVideo));
			unityVideoPlayer.LoadURL(videoURL);
		}
	}
}