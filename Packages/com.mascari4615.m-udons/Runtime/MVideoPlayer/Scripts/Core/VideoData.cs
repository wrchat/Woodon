using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VideoData : MBase
	{
		[field: SerializeField] public VRCUrl VRCUrl { get; private set; }
		[field: SerializeField] public string VideoName { get; private set; }
	}
}