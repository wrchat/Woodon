using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TransformMirror : MBase
	{
		[SerializeField] private Transform[] transforms;
		[SerializeField] private Transform[] mirrors;

		private void Start()
		{
			SetMirror();
		}

		[ContextMenu(nameof(SetMirror))]
		public void SetMirror()
		{
			MDebugLog($"{nameof(SetMirror)}");

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