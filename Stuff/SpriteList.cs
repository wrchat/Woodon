using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SpriteList : MBase
	{
		[Header("_" + nameof(SpriteList))]
		[SerializeField] private MScore mScore;
		[SerializeField] private Sprite[] spriteList;
		[SerializeField] private Image[] targetImages;
		[SerializeField] private SpriteRenderer[] targetSpriteRenderers;

		[ContextMenu(nameof(UpdateSpriteByScore))]
		public void UpdateSpriteByScore()
		{
			MDebugLog(nameof(UpdateSpriteByScore));
			if (mScore)
				UpdateSprite(mScore.Score);
		}

		public void UpdateSprite(int index)
		{
			MDebugLog(nameof(UpdateSprite));

			if (index < 0 || index >= spriteList.Length)
			{
				MDebugLog($"Index out of range: {index}");
				return;
			}

			if (targetImages != null)
				foreach (Image targetImage in targetImages)
					targetImage.sprite = spriteList[index];

			if (targetSpriteRenderers != null)
				foreach (SpriteRenderer targetSpriteRenderer in targetSpriteRenderers)
					targetSpriteRenderer.sprite = spriteList[index];
		}
	}
}