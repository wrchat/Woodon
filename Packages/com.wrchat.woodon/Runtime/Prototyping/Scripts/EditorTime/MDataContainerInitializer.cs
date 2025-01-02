using System;
using UnityEditor;
using UnityEngine;
using WRC.Woodon;

namespace WRC
{
#if UNITY_EDITOR
	public class MDataContainerInitializer : MonoBehaviour
	{
		[SerializeField] private string stringPrefix;
		[SerializeField] private string[] strings;
		[SerializeField] private Sprite[] sprites;

		[ContextMenu(nameof(InitName))]
		public void InitName()
		{
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.Name = $"{stringPrefix}{strings[0]}{index}";
			});
		}

		[ContextMenu(nameof(InitValue))]
		public void InitValue()
		{
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.Value = $"{stringPrefix}{strings[0]}{index}";
			});
		}

		[ContextMenu(nameof(InitStringData))]
		public void InitStringData()
		{
			ForEachMDataContainer((mDataContainer, index) =>
			{
				mDataContainer.StringData = new string[] { $"{stringPrefix}{strings[0]}{index}" };
			});
		}

		[ContextMenu(nameof(InitMainSprite))]
		public void InitMainSprite()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprite = sprites[index];
			});
		}

		[ContextMenu(nameof(InitSprites_A))]
		public void InitSprites_A()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprites = sprites;
			});
		}

		[ContextMenu(nameof(InitSprites_B))]
		public void InitSprites_B()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachMDataContainer((mDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				mDataContainer.Sprites = new Sprite[] { sprites[index] };
			});
		}

		private void ForEachMDataContainer(Action<MDataContainer, int> action)
		{
			MDataContainer[] mDataContainer = GetComponentsInChildren<MDataContainer>(true);

			for (int i = 0; i < mDataContainer.Length; i++)
			{
				action(mDataContainer[i], i);
				EditorUtility.SetDirty(mDataContainer[i]);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}