using UnityEngine;

namespace WRC
{
#if UNITY_EDITOR
	public class GameObjectNameChanger : MonoBehaviour
	{
		[SerializeField] private string newName = string.Empty;
		[SerializeField] private int startIndex = 0;

		[ContextMenu(nameof(ChangeChildsName))]
		public void ChangeChildsName()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				string originalName = transform.GetChild(i).name;
				string newName = $"{this.newName} [{startIndex + i}]";

				transform.GetChild(i).name = newName;
			}
		}
	}
#endif
}