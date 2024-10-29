using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIKoreanKey : UdonSharpBehaviour
	{
		[Header("_" + nameof(UIKoreanKey))]
		[SerializeField] private char targetChar;
		[SerializeField] private string shiftChar;

		private UIKoreanKeyboard _koreanKeyboardUI;
		private TextMeshProUGUI _charText;

		private char _curChar;
		public char CurChar
		{
			get => _curChar;
			set
			{
				_curChar = value;
				_charText.text = _curChar.ToString();
			}
		}

		public void Init(UIKoreanKeyboard koreanKeyboardUI)
		{
			_koreanKeyboardUI = koreanKeyboardUI;
			_charText = GetComponentInChildren<TextMeshProUGUI>();
		}

		public void UpdateShift(bool shifting)
		{
			if (shiftChar != string.Empty)
				CurChar = shifting ? shiftChar[0] : targetChar;
			else
				CurChar = targetChar;
		}

		public void Click()
		{
			_koreanKeyboardUI.Input(_curChar);
		}
	}
}