using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VideoData : MBase
	{
		[field: SerializeField] public VRCUrl VRCUrl { get; private set; }
		[field: SerializeField] public string VideoName { get; private set; }
	}
}