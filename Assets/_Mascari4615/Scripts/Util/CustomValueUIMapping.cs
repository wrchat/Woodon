using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WakVRC;

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

		[SerializeField] private Transform mValuesParent;
		[SerializeField] private Transform uimValuesParent;

		[ContextMenu(nameof(MappingMValue2UI))]
		public void MappingMValue2UI()
		{
			MValue[] mValues = mValuesParent.GetComponentsInChildren<MValue>(true);
			UIMValue[] uiMValues = uimValuesParent.GetComponentsInChildren<UIMValue>(true);

			for (int i = 0; i < mValues.Length; i++)
			{
				MValue mValue = mValues[i];
				UIMValue uiMValue = uiMValues[i];

				uiMValue.SetMValue(mValue);
				EditorUtility.SetDirty(uiMValue);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}