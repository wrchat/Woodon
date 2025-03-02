using UnityEngine;
using UnityEngine.UI;
using UdonSharp;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class SpriteList : WBase
	{
		[Header("_" + nameof(SpriteList))]
		[SerializeField] private Sprite[] sprites;

		[Header("_" + nameof(SpriteList) + " - Targets")]
		[SerializeField] private Image[] images;
		[SerializeField] private SpriteRenderer[] spriteRenderers;

		[Header("_" + nameof(SpriteList) + " - Options")]
		[SerializeField] private SpriteListInitType initType = SpriteListInitType.FirstSprite;
		[SerializeField] private WInt spriteIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (spriteIndex != null)
			{
				spriteIndex.SetMinMaxValue(0, sprites.Length - 1);
				spriteIndex.RegisterListener(this, nameof(SetAllByWInt));
				SetAllByWInt();
			}
			else
			{
				switch (initType)
				{
					case SpriteListInitType.FirstSprite:
						SetAll(0);
						break;
					case SpriteListInitType.EachIndex:
						InitSprites();
						break;
				}
			}
		}

		public void InitSprites()
		{
			WDebugLog(nameof(InitSprites));

			for (int i = 0; i < images.Length; i++)
				images[i].sprite = sprites[i];

			for (int i = 0; i < spriteRenderers.Length; i++)
				spriteRenderers[i].sprite = sprites[i];
		}

		[ContextMenu(nameof(SetAllByWInt))]
		public void SetAllByWInt()
		{
			WDebugLog(nameof(SetAllByWInt));
			if (spriteIndex)
				SetAll(spriteIndex.Value);
		}

		[ContextMenu(nameof(SetAll))]
		public void SetAll()
		{
			SetAll(0);
		}

		public void SetAll(int spriteIndex)
		{
			WDebugLog(nameof(SetAll));

			if (spriteIndex < 0 || spriteIndex >= sprites.Length)
			{
				WDebugLog($"{nameof(SetAll)}, Index out of range: {spriteIndex}", LogType.Error);
				return;
			}

			foreach (Image image in images)
				image.sprite = sprites[spriteIndex];

			foreach (SpriteRenderer spriteRenderer in spriteRenderers)
				spriteRenderer.sprite = sprites[spriteIndex];
		}
	}
}