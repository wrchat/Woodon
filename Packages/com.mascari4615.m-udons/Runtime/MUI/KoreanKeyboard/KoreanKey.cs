using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class KoreanKey : UdonSharpBehaviour
	{
		[SerializeField] private KoreanKeyboard _koreanKeyboard;

		[SerializeField] private char targetChar;
		[SerializeField] private string shiftChar;
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

		private void Start()
		{
			// _koreanKeyboard = GameObject.Find(nameof(KoreanKeyboard)).GetComponent<KoreanKeyboard>();
			_charText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
			_koreanKeyboard.Input(_curChar);
		}
	}
}