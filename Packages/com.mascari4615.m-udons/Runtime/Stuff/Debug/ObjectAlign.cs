using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectAlign : MBase
	{
		[SerializeField] private Transform parent;
		[SerializeField] private float spacing = .1f;

		private void Start()
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform obj = parent.GetChild(i);
				obj.localPosition = new Vector3(i + spacing * i, 0, 0);
			}
		}
	}
}