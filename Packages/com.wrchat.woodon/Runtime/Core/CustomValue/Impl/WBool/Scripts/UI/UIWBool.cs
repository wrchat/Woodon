using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWBool : WBoolFollower
	{
		[Header("_" + nameof(UIWBool))]
		[SerializeField] private Image[] images;

		[SerializeField] private Sprite trueSprite;
		[SerializeField] private Sprite falseSprite;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			WDebugLog($"{nameof(Init)}");

			if (wBool == null)
				return;

			wBool.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)} : {wBool.Value}");

			// Update Sprite
			if (trueSprite != null && falseSprite != null)
			{
				Sprite sprite = wBool.Value ? trueSprite : falseSprite;
				foreach (Image image in images)
					image.sprite = sprite;
			}

			// Update Color
			else
			{
				Color color = WColorUtil.GetGreenOrRed(wBool.Value);
				foreach (Image image in images)
					image.color = color;
			}
		}

		public override void SetWBool(WBool wBool)
		{
			WDebugLog($"{nameof(SetWBool)} : {wBool}");

			if (this.wBool != null)
				this.wBool.UnregisterListener(this, nameof(UpdateUI));

			this.wBool = wBool;
			Init();
		}

		#region HorribleEvents
		public void ToggleValue() => wBool.SetValue(!wBool.Value);
		public void SetValueTrue() => wBool.SetValue(true);
		public void SetValueFalse() => wBool.SetValue(false);
		#endregion
	}
}