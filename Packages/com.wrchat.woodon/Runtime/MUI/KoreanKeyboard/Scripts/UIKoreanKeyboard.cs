using UdonSharp;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIKoreanKeyboard : MBase
	{
		[Header("_" + nameof(UIKoreanKeyboard))]
		[SerializeField] private KoreanKeyboard koreanKeyboard;
		[SerializeField] private TextMeshProUGUI[] curStringTMPs;
		[SerializeField] private TextMeshProUGUI shiftButtonText;
		[SerializeField] private Image shiftButtonImage;
		
		private UIKoreanKey[] _koreanKeys;

		private void Start()
		{
			Init();
		}
		
		private void Init()
		{
			_koreanKeys = GetComponentsInChildren<UIKoreanKey>();
			foreach (UIKoreanKey koreanKeyUI in _koreanKeys)
				koreanKeyUI.Init(this);

			koreanKeyboard.RegisterUI(this);
		}
		
		public void UpdateText(string text)
		{
			foreach (TextMeshProUGUI curStringTMP in curStringTMPs)
				curStringTMP.text = koreanKeyboard.CurString;
		}

		public void UpdateShift(bool shifting)
		{
			shiftButtonImage.color = MColorUtil.GetWhiteOrBlack(shifting == true);
			shiftButtonText.color = MColorUtil.GetWhiteOrBlack(shifting == false);

			foreach (UIKoreanKey koreanKey in _koreanKeys)
				koreanKey.UpdateShift(shifting);
		}

		public void Input(char c)
		{
			koreanKeyboard.Input(c);
		}

		public void InputBackspace()
		{
			koreanKeyboard.InputBackspace();
		}

		public void InputSpace()
		{
			koreanKeyboard.InputSpace();
		}

		public void ToggleShift()
		{
			koreanKeyboard.ToggleShift();
		}

		public void Clear()
		{
			koreanKeyboard.Clear();
		}
	}
}