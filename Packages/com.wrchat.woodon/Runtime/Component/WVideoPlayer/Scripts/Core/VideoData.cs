using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VideoData : WDataContainer
	{
		[field: Header("_" + nameof(VideoData))]
		[field: SerializeField] public VRCUrl VRCUrl { get; private set; }

		public void SetVRCUrl(VRCUrl url)
		{
			VRCUrl = url;
		}
	}
}