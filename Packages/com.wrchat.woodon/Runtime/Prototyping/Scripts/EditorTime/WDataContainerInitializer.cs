using System;
using UnityEditor;
using UnityEngine;
using WRC.Woodon;

namespace WRC
{
#if UNITY_EDITOR
	public class WDataContainerInitializer : MonoBehaviour
	{
		[SerializeField] private string stringPrefix;
		[Multiline]
		[SerializeField] private string someString;
		[SerializeField] private string[] strings;
		[SerializeField] private Sprite[] sprites;

		[ContextMenu(nameof(InitName))]
		public void InitName()
		{
			ForEachWDataContainer((wDataContainer, index) =>
			{
				wDataContainer.Name = $"{stringPrefix}{strings[0]}{index}";
			});
		}

		[ContextMenu(nameof(InitValue))]
		public void InitValue()
		{
			ForEachWDataContainer((wDataContainer, index) =>
			{
				wDataContainer.Value = $"{stringPrefix}{strings[0]}{index}";
			});
		}

		[ContextMenu(nameof(InitStringData))]
		public void InitStringData()
		{
			ForEachWDataContainer((wDataContainer, index) =>
			{
				wDataContainer.StringData = new string[] { $"{stringPrefix}{strings[0]}{index}" };
			});
		}

		[ContextMenu(nameof(TheSuspect_ItemText))]
		public void TheSuspect_ItemText()
		{
			string[] lines = someString.Split("\n");
			Debug.Log($"Lines Length: {lines.Length}");
			Debug.Log($"Lines: {string.Join("|", lines)}");

			ForEachWDataContainer((wDataContainer, index) =>
			{
				string line = lines.Length > index ? lines[index] : "";
				string[] element = line.Split('\t');
				string number = element.Length > 0 ? element[0] : "";
				string place = element.Length > 1 ? element[1] : "";
				string name = element.Length > 2 ? element[2] : "";
				string _ = element.Length > 3 ? element[3] : "";
				string __ = element.Length > 4 ? element[4] : "";
				string description = element.Length > 5 ? element[5].Trim() : "";
				Debug.Log($"Processing index: {index}");
				Debug.Log($"Number: {number}, Place: {place}, Name: {name}, Description: {description}");

				wDataContainer.Name = name;
				wDataContainer.StringData = new string[] { description, place };
			});
		}

		[ContextMenu(nameof(InitMainSprite))]
		public void InitMainSprite()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachWDataContainer((wDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				wDataContainer.Sprite = sprites[index];
			});
		}

		[ContextMenu(nameof(InitSprites_A))]
		public void InitSprites_A()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachWDataContainer((wDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				wDataContainer.Sprites = sprites;
			});
		}

		[ContextMenu(nameof(InitSprites_B))]
		public void InitSprites_B()
		{
			if (sprites == null || sprites.Length == 0)
				return;

			ForEachWDataContainer((wDataContainer, index) =>
			{
				if (sprites.Length <= index)
					return;

				wDataContainer.Sprites = new Sprite[] { sprites[index] };
			});
		}

		private void ForEachWDataContainer(Action<WDataContainer, int> action)
		{
			WDataContainer[] wDataContainer = GetComponentsInChildren<WDataContainer>(true);

			for (int i = 0; i < wDataContainer.Length; i++)
			{
				action(wDataContainer[i], i);
				EditorUtility.SetDirty(wDataContainer[i]);
			}

			AssetDatabase.SaveAssets();
		}
	}
#endif
}