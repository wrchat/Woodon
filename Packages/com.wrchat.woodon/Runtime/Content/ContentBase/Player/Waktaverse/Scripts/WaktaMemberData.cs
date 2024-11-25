using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WaktaMemberData : MDataContainer
	{
		[field: Header("Member Data")]
		[field: SerializeField] public string Nickname { get; private set; }
		[field: SerializeField] public string DisplayName { get; private set; }
		[field: SerializeField] public WaktaMember Member { get; private set; }
	}
}