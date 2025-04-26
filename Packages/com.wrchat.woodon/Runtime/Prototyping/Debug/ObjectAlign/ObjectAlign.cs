using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectAlign : WBase
	{
		[SerializeField] private Transform parent;
		[SerializeField] private float spacing = .1f;
		[SerializeField] private bool alignOnStart = true;

		private void Start()
		{
			if (alignOnStart)
				AlignObjects();
		}

		[ContextMenu(nameof(AlignObjects))]
		public void AlignObjects()
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform obj = parent.GetChild(i);
				obj.localPosition = new Vector3(i + spacing * i, 0, 0);
			}
		}

	}
}