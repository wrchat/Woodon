using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWInt : WIntFollower
	{
		[Header("_" + nameof(UIWInt))]
		[SerializeField] private TextMeshProUGUI[] valueTexts;
		[SerializeField] private string format = "{0}";
		[SerializeField] private Image[] hardButtons;
		[SerializeField] private float printMultiply = 1;
		[SerializeField] private int printPlus = 0;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			WDebugLog($"{nameof(Init)}");

			if (wInt == null)
				return;

			wInt.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			if (wInt == null)
				return;

			WDebugLog($"{nameof(UpdateUI)} : {wInt.Value}");

			int value = wInt.Value;

			string valueString = ((int)(value * printMultiply) + printPlus).ToString();
			string finalString = string.Format(format, valueString);
			finalString = finalString.Replace("MAX", wInt.MaxValue.ToString());

			foreach (TextMeshProUGUI valueText in valueTexts)
				valueText.text = finalString;

			for (int i = 0; i < hardButtons.Length; i++)
			{
				hardButtons[i].color = WColorUtil.GetBlackOrGray(i != value);

				// AutosizeButton
				hardButtons[i].transform.parent.gameObject.SetActive(wInt.MinValue <= i && i <= wInt.MaxValue);
			}
		}

		public override void SetWInt(WInt wInt)
		{
			WDebugLog($"{nameof(SetWInt)} : {wInt}");

			if (this.wInt != null)
				this.wInt.UnregisterListener(this, nameof(UpdateUI));

			this.wInt = wInt;
			Init();
		}

		#region HorribleEvents
		public void IncreaseValue() => wInt.IncreaseValue();
		public void AddValue10() => wInt.AddValue(wInt.IncreaseAmount * 10);
		public void DecreaseValue() => wInt.DecreaseValue();
		public void SubValue10() => wInt.SubValue(wInt.DecreaseAmount * 10);
		public void ResetValue() => wInt.ResetValue();

		public void SetValue0() => wInt.SetValue(0);
		public void SetValue1() => wInt.SetValue(1);
		public void SetValue2() => wInt.SetValue(2);
		public void SetValue3() => wInt.SetValue(3);
		public void SetValue4() => wInt.SetValue(4);
		public void SetValue5() => wInt.SetValue(5);
		public void SetValue6() => wInt.SetValue(6);
		public void SetValue7() => wInt.SetValue(7);
		public void SetValue8() => wInt.SetValue(8);
		public void SetValue9() => wInt.SetValue(9);
		#endregion
	}
}