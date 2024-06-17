using UnityEngine;

namespace Mascari4615
{
	public class MData : MBase
	{
		[field: SerializeField] public string[] Strings { get; private set; }
		[field: SerializeField] public Sprite[] Sprites { get; private set; }
	}
}