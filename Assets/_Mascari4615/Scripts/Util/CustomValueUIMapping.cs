using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
#if UNITY_EDITOR
	public class CustomValueUIMapping : MonoBehaviour
	{
		[SerializeField] private Transform mBoolsParent;
		[SerializeField] private Transform uimBoolsParent;

		[ContextMenu(nameof(MappingMBool2UI))]
		public void MappingMBool2UI()
		{
			MBool[] mBools = mBoolsParent.GetComponentsInChildren<MBool>(true);
			UIMBool[] uimBools = uimBoolsParent.GetComponentsInChildren<UIMBool>(true);

			for (int i = 0; i < mBools.Length; i++)
			{
				MBool mBool = mBools[i];
				UIMBool uimBool = uimBools[i];

				uimBool.SetMBool(mBool);
				EditorUtility.SetDirty(uimBool);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}