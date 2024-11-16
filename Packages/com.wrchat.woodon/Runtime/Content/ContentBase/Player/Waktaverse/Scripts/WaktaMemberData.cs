using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WaktaMemberData : MDataContainer
	{
		[field: Header("Member Data")]
		[field: SerializeField] public WaktaMember Member { get; private set; }
	}
}