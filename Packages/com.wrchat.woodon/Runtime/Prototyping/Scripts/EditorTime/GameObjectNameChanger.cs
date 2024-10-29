using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
#if UNITY_EDITOR
	public class GameObjectNameChanger : MonoBehaviour
	{
		[SerializeField] private string[] someStrings;

		[ContextMenu(nameof(ChangeChildsName))]
		public void ChangeChildsName()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				string originalName = transform.GetChild(i).name;
				string newName = $"{someStrings[0]} [{i}]";

				transform.GetChild(i).name = newName;
			}
		}
	}
#endif
}