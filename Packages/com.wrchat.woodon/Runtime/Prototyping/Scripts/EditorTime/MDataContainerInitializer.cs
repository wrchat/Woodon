using UnityEditor;
using UnityEngine;
using WRC.Woodon;

namespace WRC
{
#if UNITY_EDITOR
	public class MDataContainerInitializer : MonoBehaviour
	{
		[SerializeField] private string prefix;
		[SerializeField] private string[] someStrings;

		[ContextMenu(nameof(InitName))]
		public void InitName()
		{
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Name = $"{prefix}{someStrings[0]}{i}";

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}

		[ContextMenu(nameof(InitValue))]
		public void InitValue()
		{
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Value = $"{prefix}{someStrings[0]}{i}";

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}

		[ContextMenu(nameof(InitStringData))]
		public void InitStringData()
		{
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.StringData = new string[] { $"{prefix}{someStrings[0]}{i}" };

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}

		[SerializeField] private Sprite[] sprite;
		[ContextMenu(nameof(InitSprites))]
		public void InitSprites()
		{
			MDataContainer[] mDatas = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < transform.childCount; i++)
			{
				MDataContainer mData = mDatas[i];
				mData.Sprite = sprite[i];

				EditorUtility.SetDirty(mData);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}