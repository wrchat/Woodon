using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TransformMirror : MBase
	{
		[SerializeField] private Transform[] transforms;
		[SerializeField] private Transform[] mirrors;

		private void Start()
		{
			if (transforms.Length != mirrors.Length)
			{
				Debug.LogError("Transforms and Mirrors length must be equal.");
				return;
			}

			for (int i = 0; i < transforms.Length; i++)
				mirrors[i].position = new Vector3(transforms[i].position.x, transforms[i].position.y, transforms[i].position.z);
		}
	}
}