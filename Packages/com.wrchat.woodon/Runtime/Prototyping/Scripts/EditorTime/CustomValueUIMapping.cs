using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace WRC
{
#if UNITY_EDITOR
	public class CustomValueUIMapping : MonoBehaviour
	{
		[SerializeField] private Transform wBoolsParent;
		[SerializeField] private Transform uiWBoolsParent;

		[ContextMenu(nameof(MappingWBool2UI))]
		public void MappingWBool2UI()
		{
			WBool[] wBools = wBoolsParent.GetComponentsInChildren<WBool>(true);
			UIWBool[] uiWBools = uiWBoolsParent.GetComponentsInChildren<UIWBool>(true);

			for (int i = 0; i < wBools.Length; i++)
			{
				WBool wBool = wBools[i];
				UIWBool uiWBool = uiWBools[i];

				uiWBool.SetWBool(wBool);
				EditorUtility.SetDirty(uiWBool);
			}

			AssetDatabase.SaveAssets();
		}

		[SerializeField] private Transform wIntsParent;
		[SerializeField] private Transform uiWIntsParent;

		[ContextMenu(nameof(MappingWInt2UI))]
		public void MappingWInt2UI()
		{
			WInt[] wInts = wIntsParent.GetComponentsInChildren<WInt>(true);
			UIWInt[] uiWInts = uiWIntsParent.GetComponentsInChildren<UIWInt>(true);

			for (int i = 0; i < wInts.Length; i++)
			{
				WInt wInt = wInts[i];
				UIWInt uiWInt = uiWInts[i];

				uiWInt.SetWInt(wInt);
				EditorUtility.SetDirty(uiWInt);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}