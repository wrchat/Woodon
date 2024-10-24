using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SpriteList : MBase
	{
		[Header("_" + nameof(SpriteList))]
		[SerializeField] private Sprite[] sprites;

		[Header("_" + nameof(SpriteList) + " - Targets")]
		[SerializeField] private Image[] images;
		[SerializeField] private SpriteRenderer[] spriteRenderers;

		[Header("_" + nameof(SpriteList) + " - Options")]
		[SerializeField] private MValue mValue_SpriteIndex;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mValue_SpriteIndex)
			{
				mValue_SpriteIndex.RegisterListener(this, nameof(SetAllByMValue));
				SetAllByMValue();
			}
			else
			{
				InitSprites();
			}
		}

		public void InitSprites()
		{
			MDebugLog(nameof(InitSprites));

			for (int i = 0; i < images.Length; i++)
				images[i].sprite = sprites[i];
			
			for (int i = 0; i < spriteRenderers.Length; i++)
				spriteRenderers[i].sprite = sprites[i];
		}

		[ContextMenu(nameof(SetAllByMValue))]
		public void SetAllByMValue()
		{
			MDebugLog(nameof(SetAllByMValue));
			if (mValue_SpriteIndex)
				SetAll(mValue_SpriteIndex.Value);
		}

		public void SetAll(int spriteIndex)
		{
			MDebugLog(nameof(SetAll));

			if (spriteIndex < 0 || spriteIndex >= sprites.Length)
			{
				MDebugLog($"Index out of range: {spriteIndex}");
				return;
			}

			foreach (Image targetImage in images)
				targetImage.sprite = sprites[spriteIndex];

			foreach (SpriteRenderer targetSpriteRenderer in spriteRenderers)
				targetSpriteRenderer.sprite = sprites[spriteIndex];
		}
	}
}