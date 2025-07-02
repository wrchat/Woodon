using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectAlign : WBase
	{
		[Header("_" + nameof(ObjectAlign))]
		[SerializeField] private Transform parent;
		[SerializeField] private float spacing = .1f;
		[SerializeField] private bool alignOnStart = true;
		[SerializeField] private bool alignCenter = true;

		private void Start()
		{
			if (alignOnStart == true)
				AlignObjects();
		}

		[ContextMenu(nameof(AlignObjects))]
		public void AlignObjects()
		{
			if (alignCenter == true)
			{
				float totalWidth = (parent.childCount - 1) * spacing;
				float startX = -totalWidth / 2f;

				for (int i = 0; i < parent.childCount; i++)
				{
					Transform obj = parent.GetChild(i);
					obj.localPosition = new Vector3(startX + spacing * i, 0, 0);
				}
			}
			else
			{
				for (int i = 0; i < parent.childCount; i++)
				{
					Transform obj = parent.GetChild(i);
					obj.localPosition = new Vector3(spacing * i, 0, 0);
				}
			}
		}
	}
}