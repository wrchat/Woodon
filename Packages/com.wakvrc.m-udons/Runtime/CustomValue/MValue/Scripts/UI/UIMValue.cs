using TMPro;
using UdonSharp;
using UnityEngine;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMValue : MValueFollower
	{
		[Header("_" + nameof(UIMValue))]
		[SerializeField] private TextMeshProUGUI[] valueTexts;
		[SerializeField] private string format = "{0}";
		[SerializeField] private GameObject[] hardButtons;
		[SerializeField] private float printMultiply = 1;
		[SerializeField] private int printPlus = 0;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (mValue == null)
				return;

			mValue.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			if (mValue == null)
				return;

			MDebugLog($"{nameof(UpdateUI)} : {mValue.Value}");

			int value = mValue.Value;

			string valueString = ((int)(value * printMultiply) + printPlus).ToString();
			string finalString = string.Format(format, valueString);
			finalString = finalString.Replace("MAX", mValue.MaxValue.ToString());

			foreach (TextMeshProUGUI valueText in valueTexts)
				valueText.text = finalString;

			for (int i = 0; i < hardButtons.Length; i++)
				hardButtons[i].SetActive(mValue.MinValue <= i && i <= mValue.MaxValue);
		}

		public override void SetMValue(MValue mValue)
		{
			MDebugLog($"{nameof(SetMValue)} : {mValue}");

			if (this.mValue != null)
				this.mValue.RemoveListener(this, nameof(UpdateUI));

			this.mValue = mValue;
			Init();
		}

		#region HorribleEvents
		public void IncreaseValue() => mValue.IncreaseValue();
		public void AddValue10() => mValue.AddValue(mValue.IncreaseAmount * 10);
		public void DecreaseValue() => mValue.DecreaseValue();
		public void SubValue10() => mValue.SubValue(mValue.DecreaseAmount * 10);
		public void ResetValue() => mValue.ResetValue();

		public void SetValue0() => mValue.SetValue(0);
		public void SetValue1() => mValue.SetValue(1);
		public void SetValue2() => mValue.SetValue(2);
		public void SetValue3() => mValue.SetValue(3);
		public void SetValue4() => mValue.SetValue(4);
		public void SetValue5() => mValue.SetValue(5);
		public void SetValue6() => mValue.SetValue(6);
		public void SetValue7() => mValue.SetValue(7);
		public void SetValue8() => mValue.SetValue(8);
		public void SetValue9() => mValue.SetValue(9);
		#endregion
	}
}