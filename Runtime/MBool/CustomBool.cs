using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class CustomBool : MEventSender
	{
		[Header("_" + nameof(CustomBool))]
		[SerializeField] protected bool defaultValue;
		[SerializeField] private Image[] buttonUIImages;
		[SerializeField] private Sprite[] buttonUISprites;

		[FieldChangeCallback(nameof(Value))]
		protected bool _value;
		public bool Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}

		protected virtual void Start()
		{
			SetValue(defaultValue);
			OnValueChange();
		}

		protected virtual void OnValueChange()
		{
			MDebugLog($"{nameof(OnValueChange)}");

			UpdateUI();
			SendEvents();
		}

		protected void UpdateUI()
		{
			foreach (var i in buttonUIImages)
			{
				if (buttonUISprites != null && buttonUISprites.Length > 0)
					i.sprite = buttonUISprites[Value ? 0 : 1];
				else
					i.color = MColorUtil.GetGreenOrRed(Value);
			}
		}

		public virtual void SetValue(bool newValue)
		{
			MDebugLog($"{nameof(SetValue)}({newValue})");

			if (Value != newValue)
				Value = newValue;
		}

		// Called By Other Udons
		public virtual void ToggleValue() => SetValue(!Value);
		public void SetValueTrue() => SetValue(true);
		public void SetValueFalse() => SetValue(false);
	}
}
