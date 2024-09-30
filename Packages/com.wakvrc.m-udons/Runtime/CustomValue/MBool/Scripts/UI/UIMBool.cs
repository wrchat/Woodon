using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMBool : MBase
	{
		[Header("_" + nameof(UIMBool))]
		[SerializeField] private MBool mBool;
		[SerializeField] private Image[] images;

		[SerializeField] private Sprite trueSprite;
		[SerializeField] private Sprite falseSprite;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mBool.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			// Update Sprite
			if (trueSprite != null && falseSprite != null)
			{
				Sprite sprite = mBool.Value ? trueSprite : falseSprite;
				foreach (Image image in images)
					image.sprite = sprite;
			}

			// Update Color
			else
			{
				Color color = MColorUtil.GetGreenOrRed(mBool.Value);
				foreach (Image image in images)
					image.color = color;
			}
		}

		#region HorribleEvents
		public void ToggleValue() => mBool.SetValue(!mBool.Value);
		public void SetValueTrue() => mBool.SetValue(true);
		public void SetValueFalse() => mBool.SetValue(false);
		#endregion

		#region EditorTime
		public void SetMBool(MBool mBool)
		{
			this.mBool = mBool;
		}
		#endregion
	}
}