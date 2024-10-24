using TMPro;
using UdonSharp;
using UnityEngine;

namespace WakVRC
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
		/// 키보드 입력과 함께 사용되는 InputField
		/// </summary>
		[SerializeField] private TMP_InputField syncInputField;

		private readonly string[] JongComboSpreads =
			{ "ㄱㅅ", "ㄴㅈ", "ㄴㅎ", "ㄹㄱ", "ㄹㅁ", "ㄹㅂ", "ㄹㅅ", "ㄹㅌ", "ㄹㅍ", "ㄹㅎ", "ㅂㅅ" };

		private readonly string[] JungComboSpreads = { "ㅗㅏ", "ㅗㅐ", "ㅗㅣ", "ㅜㅓ", "ㅜㅔ", "ㅜㅣ", "ㅡㅣ" };

		private UIKoreanKeyboard[] uis = new UIKoreanKeyboard[0];

		public void RegisterUI(UIKoreanKeyboard ui)
		{
			// 기존 배열에 추가
			MDataUtil.Add(ref uis, ui);

			// 새로 추가된 UI에 현재 정보 전달
			ui.UpdateShift(IsShifting);
		}

		private bool isShifting;

		/// <summary>
		/// 현재 입력된 문자(열)
		/// 다른 스크립트에서 꺼내 쓰시면 됩니다.
		/// </summary>
		[SerializeField] private MString curMString;

		public string CurString => curMString.Value;

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

				// UI에 현재 정보를 전달합니다.
				foreach (UIKoreanKeyboard ui in uis)
					ui.UpdateShift(IsShifting);
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			curMString.RegisterListener(this, nameof(OnStringChanged));
			IsShifting = false;
		}

		public void OnStringChanged()
		{
			// InputField의 텍스트를 현재 문자열과 동기화시킵니다.
			if (syncInputField)
				syncInputField.text = CurString;

			// UI에 현재 정보를 전달합니다.
			foreach (UIKoreanKeyboard ui in uis)
				ui.UpdateText(CurString);
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
			for (int index = 0; index < strs.Length; index++)
				if (strs[index] == str)
					return index;

			return -1;
		}

		private int GetIndexOf(string str, char c)
		{
			for (int index = 0; index < str.Length; index++)
				if (str[index] == c)
					return index;

			return -1;
		}

		public void Input(char input)
		{
			if (CurString == string.Empty)
			{
				SetString(input.ToString());
				return;
			}

			if (IsShifting)
				IsShifting = false;

			bool isInputJa = 'ㄱ' <= input && input <= 'ㅎ';
			bool isInputMo = 'ㅏ' <= input && input <= 'ㅣ';

			char lastChr = CurString.Substring(CurString.Length - 1)[0];
			bool isLastCharJa = 'ㄱ' <= lastChr && lastChr <= 'ㅎ';
			bool isLastCharMo = 'ㅏ' <= lastChr && lastChr <= 'ㅣ';

			if (isLastCharJa || isLastCharMo)
			{
				// 자음,모음
				if (isLastCharJa)
				{
					if (isInputMo)
					{
						int i = GetIndexOf(ChoK, lastChr);
						int m = GetIndexOf(JungK, input);
						int t = 0;
						char c = Encoding(i, m, t);
						SetString(CurString.Substring(0, CurString.Length - 1) + c);
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
				int cho = ChoIndex(lastChr);
				int jung = JungIndex(lastChr);
				int jong = JongIndex(lastChr);

				if (jong == 0)
				{
					// 종성이 없는경우
					if (isInputJa)
					{
						jong = GetIndexOf(JongK, input);
						if (jong != -1)
						{
							// 없는 종성문자인경우 제외
							char c = Encoding(cho, jung, jong);
							SetString(CurString.Substring(0, CurString.Length - 1) + c);
							return;
						}
					}
					else if (isInputMo)
					{
						// 모음조합문자
						string chkChr = JungK[jung] + input.ToString();
						int combIndex = GetIndexOf(JungComboSpreads, chkChr);
						if (combIndex != -1)
						{
							char combChr = JungCombo[combIndex];
							jung = GetIndexOf(JungK, combChr);
							char c = Encoding(cho, jung, jong);
							SetString(CurString.Substring(0, CurString.Length - 1) + c);
							return;
						}
					}
				}
				else
				{
					// 종성이 있는경우
					if (isInputMo)
					{
						char tChr = JongK[jong];
						// 조합문자일경우 다시 쪼갠다
						int combIndex = GetIndexOf(JongCombo, tChr);
						if (combIndex != -1)
						{
							string partChr = JongComboSpreads[combIndex];
							jong = GetIndexOf(JongK, partChr[0]);
							tChr = partChr[1];
						}
						else
						{
							jong = 0;
						}

						char c1 = Encoding(cho, jung, jong);
						cho = GetIndexOf(ChoK, tChr);
						if (cho != -1)
						{
							jung = GetIndexOf(JungK, input);
							char c2 = Encoding(cho, jung, 0);
							SetString(CurString.Substring(0, CurString.Length - 1) + c1 + c2);
							return;
						}
					}
					else if (isInputJa)
					{
						// 자음조합문자
						string chkChr = JongK[jong] + input.ToString();
						int combIndex = GetIndexOf(JongComboSpreads, chkChr);
						if (combIndex != -1)
						{
							char combChr = JongCombo[combIndex];
							jong = GetIndexOf(JongK, combChr);
							char c = Encoding(cho, jung, jong);
							SetString(CurString.Substring(0, CurString.Length - 1) + c);
							return;
						}
					}
				}
			}

			// 없는 문자
			SetString(CurString + input);
		}

		public void InputBackspace()
		{
			// MDebugLog($"{nameof(InputBackspace)} {gameObject.transform.parent.name} : : {CurString}, {CurString == string.Empty}");

			if (CurString == string.Empty) return;

			char lastChr = CurString[CurString.Length - 1];
			bool isLastCharJa = 'ㄱ' <= lastChr && lastChr <= 'ㅎ';
			bool isLastCharMo = 'ㅏ' <= lastChr && lastChr <= 'ㅣ';

			if (isLastCharJa || isLastCharMo)
			{
				SetString(CurString.Substring(0, CurString.Length - 1));
				return;
			}

			if (lastChr >= BASE_CODE && lastChr <= BASE_CODE + 0x2BA4)
			{
				// 한글
				int cho = ChoIndex(lastChr);
				int jung = JungIndex(lastChr);
				int jong = JongIndex(lastChr);

				if (jong == 0)
				{
					// 받침 X

					// 모음이 조합모음인지 판단
					int combIndex = GetIndexOf(JungCombo, JungK[jung]);
					if (combIndex != -1)
					{
						// 조합모음이면 앞쪽만 남기기
						string combChr = JungComboSpreads[combIndex];
						jung = GetIndexOf(JungK, combChr[0]);

						char c = Encoding(cho, jung, jong);
						SetString(CurString.Substring(0, CurString.Length - 1) + c);
						return;
					}
					else
					{
						char c = ChoK[cho];
						SetString(CurString.Substring(0, CurString.Length - 1) + c);
						return;
					}
				}
				else
				{
					// 받침 O

					// 모음이 조합자음인지 판단
					int combIndex = GetIndexOf(JongCombo, JongK[jong]);
					if (combIndex != -1)
					{
						// 조합자음이면 앞쪽만 남기기
						string combChr = JongComboSpreads[combIndex];
						jong = GetIndexOf(JongK, combChr[0]);
					}
					else
					{
						jong = 0;
					}

					// MDebugLog($"{cho}, {jung}, {jong} = {Encoding(cho, jung, jong)}");

					char c = Encoding(cho, jung, jong);
					SetString(CurString.Substring(0, CurString.Length - 1) + c);
					return;
				}
			}

			// 없는 문자
			SetString(CurString.Substring(0, CurString.Length - 1));

			if (IsShifting)
				IsShifting = false;
		}

		public void UpdateCurStringByInputField()
		{
			if (syncInputField == null)
				return;

			SetString(syncInputField.text);
		}

		private void SetString(string newString)
		{
			curMString.SetValue(newString);
		}

		public void Clear()
		{
			SetString(string.Empty);
		}
	}
}