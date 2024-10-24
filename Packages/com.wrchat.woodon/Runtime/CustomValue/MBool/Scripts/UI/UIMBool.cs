using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMBool : MBoolFollower
	{
		[Header("_" + nameof(UIMBool))]
		[SerializeField] private Image[] images;

		[SerializeField] private Sprite trueSprite;
		[SerializeField] private Sprite falseSprite;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (mBool == null)
				return;

			mBool.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			MDebugLog($"{nameof(UpdateUI)} : {mBool.Value}");

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

		public override void SetMBool(MBool mBool)
		{
			MDebugLog($"{nameof(SetMBool)} : {mBool}");

			if (this.mBool != null)
				this.mBool.RemoveListener(this, nameof(UpdateUI));

			this.mBool = mBool;
			Init();
		}

		#region HorribleEvents
		public void ToggleValue() => mBool.SetValue(!mBool.Value);
		public void SetValueTrue() => mBool.SetValue(true);
		public void SetValueFalse() => mBool.SetValue(false);
		#endregion
	}
}