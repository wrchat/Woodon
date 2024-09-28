using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[DefaultExecutionOrder(1000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class KoreanKeyboard : UdonSharpBehaviour
	{
		private const int BASE_CODE = 0xAC00; //BASE 코드
		private const int BASE_INIT = 0x1100; //'ㄱ'
		private const int BASE_VOWEL = 0x1161; //'ㅏ'

		private const string ChoK = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
		private const string JungK = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
		private const string JongK = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";
		private const string JongCombo = "ㄳㄵㄶㄺㄻㄼㄽㄾㄿㅀㅄ";
		private const string JungCombo = "ㅘㅙㅚㅝㅞㅟㅢ";

		/// <summary>
		/// 문자열을 동기화시킬 TMP들
		/// </summary>
		[SerializeField] private TextMeshProUGUI[] curStringTMPs;

		/// <summary>
		/// 키보드 입력과 함께 사용되는 InputField
		/// </summary>
		[SerializeField] private TMP_InputField syncInputField;

		[SerializeField] private TextMeshProUGUI shiftButtonText;
		[SerializeField] private Image shiftButtonImage;

		private readonly string[] JongComboSpreads =
			{ "ㄱㅅ", "ㄴㅈ", "ㄴㅎ", "ㄹㄱ", "ㄹㅁ", "ㄹㅂ", "ㄹㅅ", "ㄹㅌ", "ㄹㅍ", "ㄹㅎ", "ㅂㅅ" };

		private readonly string[] JungComboSpreads = { "ㅗㅏ", "ㅗㅐ", "ㅗㅣ", "ㅜㅓ", "ㅜㅔ", "ㅜㅣ", "ㅡㅣ" };

		private KoreanKey[] _koreanKeys;

		private bool isShifting;

		/// <summary>
		/// 현재 입력된 문자(열)
		/// 다른 스크립트에서 꺼내 쓰시면 됩니다.
		/// </summary>
		public string CurString
		{
			get => _curString;
			set
			{
				_curString = value;

				// InputField의 텍스트를 현재 문자열과 동기화시킵니다.
				if (syncInputField)
					syncInputField.text = _curString;

				// curStringTMPs의 텍스트를 현재 문자열과 동기화시킵니다.
				foreach (var curStringTMP in curStringTMPs)
					curStringTMP.text = _curString;
			}
		}

		private string _curString = string.Empty;

		/// <summary>
		/// Shift가 눌린 상태인가?
		/// true일 경우, 키 버튼 클릭 시 Shift가 눌린 상태의 문자가 입력됩니다.
		/// I.E. true 인 상태에서 'ㄱ' 키 버튼을 클릭하면, 'ㄲ'이 입력됩니다.
		/// </summary>
		public bool IsShifting
		{
			get => isShifting;
			set
			{
				isShifting = value;

				shiftButtonImage.color = GetWhiteOrBlack(isShifting);
				shiftButtonText.color = GetWhiteOrBlack(!isShifting);

				// 키 버튼에 현재 정보를 전달합니다.
				foreach (var koreanKey in _koreanKeys)
					koreanKey.UpdateShift(IsShifting);
			}
		}

		private Color GetWhiteOrBlack(bool boolVar)
		{
			Color WHITE = Color.white;
			Color BLACK = new Color(38f / 255f, 43f / 255f, 68f / 255f);
			return boolVar ? WHITE : BLACK;
		}

		private void Start()
		{
			_koreanKeys = GetComponentsInChildren<KoreanKey>();
			IsShifting = false;
		}

		public void ToggleShift()
		{
			IsShifting = !IsShifting;
		}

		public void InputSpace()
		{
			Input(' ');
		}

		private char Encoding(int cho, int jung, int jong)
		{
			return (char)((cho * 21 + jung) * 28 + jong + BASE_CODE);
		}

		private int ChoIndex(char chr)
		{
			return (chr - BASE_CODE) / 28 / 21;
		}

		private int JungIndex(char chr)
		{
			return (chr - BASE_CODE) / 28 % 21;
		}

		private int JongIndex(char chr)
		{
			return (chr - BASE_CODE) % 28;
		}

		private int GetIndexOf(string[] strs, string str)
		{
			for (var index = 0; index < strs.Length; index++)
				if (strs[index] == str)
					return index;

			return -1;
		}

		private int GetIndexOf(string str, char c)
		{
			for (var index = 0; index < str.Length; index++)
				if (str[index] == c)
					return index;

			return -1;
		}

		public void Input(char input)
		{
			if (CurString == string.Empty)
			{
				CurString = input.ToString();
				return;
			}

			if (IsShifting)
				IsShifting = false;

			var isInputJa = 'ㄱ' <= input && input <= 'ㅎ';
			var isInputMo = 'ㅏ' <= input && input <= 'ㅣ';

			var lastChr = CurString.Substring(CurString.Length - 1)[0];
			var isLastCharJa = 'ㄱ' <= lastChr && lastChr <= 'ㅎ';
			var isLastCharMo = 'ㅏ' <= lastChr && lastChr <= 'ㅣ';

			if (isLastCharJa || isLastCharMo)
			{
				// 자음,모음
				if (isLastCharJa)
				{
					if (isInputMo)
					{
						var i = GetIndexOf(ChoK, lastChr);
						var m = GetIndexOf(JungK, input);
						var t = 0;
						var c = Encoding(i, m, t);
						CurString = CurString.Substring(0, CurString.Length - 1) + c;
						return;
					}
				}
				else if (isLastCharMo)
				{
				}
			}
			else if (lastChr >= BASE_CODE && lastChr <= BASE_CODE + 0x2BA4)
			{
				// 한글
				var cho = ChoIndex(lastChr);
				var jung = JungIndex(lastChr);
				var jong = JongIndex(lastChr);

				if (jong == 0)
				{
					// 종성이 없는경우
					if (isInputJa)
					{
						jong = GetIndexOf(JongK, input);
						if (jong != -1)
						{
							// 없는 종성문자인경우 제외
							var c = Encoding(cho, jung, jong);
							CurString = CurString.Substring(0, CurString.Length - 1) + c;
							return;
						}
					}
					else if (isInputMo)
					{
						// 모음조합문자
						var chkChr = JungK[jung] + input.ToString();
						var combIndex = GetIndexOf(JungComboSpreads, chkChr);
						if (combIndex != -1)
						{
							var combChr = JungCombo[combIndex];
							jung = GetIndexOf(JungK, combChr);
							var c = Encoding(cho, jung, jong);
							CurString = CurString.Substring(0, CurString.Length - 1) + c;
							return;
						}
					}
				}
				else
				{
					// 종성이 있는경우
					if (isInputMo)
					{
						var tChr = JongK[jong];
						// 조합문자일경우 다시 쪼갠다
						var combIndex = GetIndexOf(JongCombo, tChr);
						if (combIndex != -1)
						{
							var partChr = JongComboSpreads[combIndex];
							jong = GetIndexOf(JongK, partChr[0]);
							tChr = partChr[1];
						}
						else
						{
							jong = 0;
						}

						var c1 = Encoding(cho, jung, jong);
						cho = GetIndexOf(ChoK, tChr);
						if (cho != -1)
						{
							jung = GetIndexOf(JungK, input);
							var c2 = Encoding(cho, jung, 0);
							CurString = CurString.Substring(0, CurString.Length - 1) + c1 + c2;
							return;
						}
					}
					else if (isInputJa)
					{
						// 자음조합문자
						var chkChr = JongK[jong] + input.ToString();
						var combIndex = GetIndexOf(JongComboSpreads, chkChr);
						if (combIndex != -1)
						{
							var combChr = JongCombo[combIndex];
							jong = GetIndexOf(JongK, combChr);
							var c = Encoding(cho, jung, jong);
							CurString = CurString.Substring(0, CurString.Length - 1) + c;
							return;
						}
					}
				}
			}

			// 없는 문자
			CurString += input;
		}

		public void InputBackspace()
		{
			// MDebugLog($"{nameof(InputBackspace)} {gameObject.transform.parent.name} : : {CurString}, {CurString == string.Empty}");

			if (CurString == string.Empty) return;

			var lastChr = CurString[CurString.Length - 1];
			var isLastCharJa = 'ㄱ' <= lastChr && lastChr <= 'ㅎ';
			var isLastCharMo = 'ㅏ' <= lastChr && lastChr <= 'ㅣ';

			if (isLastCharJa || isLastCharMo)
			{
				CurString = CurString.Substring(0, CurString.Length - 1);
				return;
			}

			if (lastChr >= BASE_CODE && lastChr <= BASE_CODE + 0x2BA4)
			{
				// 한글
				var cho = ChoIndex(lastChr);
				var jung = JungIndex(lastChr);
				var jong = JongIndex(lastChr);

				if (jong == 0)
				{
					// 받침 X

					// 모음이 조합모음인지 판단
					var combIndex = GetIndexOf(JungCombo, JungK[jung]);
					if (combIndex != -1)
					{
						// 조합모음이면 앞쪽만 남기기
						var combChr = JungComboSpreads[combIndex];
						jung = GetIndexOf(JungK, combChr[0]);

						var c = Encoding(cho, jung, jong);
						CurString = CurString.Substring(0, CurString.Length - 1) + c;
						return;
					}
					else
					{
						var c = ChoK[cho];
						CurString = CurString.Substring(0, CurString.Length - 1) + c;
						return;
					}
				}
				else
				{
					// 받침 O

					// 모음이 조합자음인지 판단
					var combIndex = GetIndexOf(JongCombo, JongK[jong]);
					if (combIndex != -1)
					{
						// 조합자음이면 앞쪽만 남기기
						var combChr = JongComboSpreads[combIndex];
						jong = GetIndexOf(JongK, combChr[0]);
					}
					else
					{
						jong = 0;
					}

					// MDebugLog($"{cho}, {jung}, {jong} = {Encoding(cho, jung, jong)}");

					var c = Encoding(cho, jung, jong);
					CurString = CurString.Substring(0, CurString.Length - 1) + c;
					return;
				}
			}

			// 없는 문자
			CurString = CurString.Substring(0, CurString.Length - 1);

			if (IsShifting)
				IsShifting = false;
		}

		public void UpdateCurStringByInputField()
		{
			CurString = syncInputField.text;
		}
	}
}