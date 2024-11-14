using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VideoData : MDataContainer
	{
		[field: Header("_" + nameof(VideoData))]
		[field: SerializeField] public VRCUrl VRCUrl { get; private set; }
	}
}