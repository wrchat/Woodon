using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
#if UNITY_EDITOR
	public class MDataInitializer : MonoBehaviour
	{
		[SerializeField] private string prefix;
		[SerializeField] private string[] someStrings;

		[ContextMenu(nameof(Init))]
		public void Init()
		{
			MData[] mDatas = GetComponentsInChildren<MData>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MData mData = mDatas[i];
				// mData.Value = $"{prefix} {someStrings[0]}{i}";
				mData.StringData = new string[] { $"{prefix} {i}" };

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}