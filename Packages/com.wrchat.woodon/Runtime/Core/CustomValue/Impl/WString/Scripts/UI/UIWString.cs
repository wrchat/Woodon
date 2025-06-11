using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class UIWString : WBase
	{
		[Header("_" + nameof(UIWString))]
		[SerializeField] private WString wString;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private TextMeshProUGUI[] texts;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (wString == null)
				return;

			wString.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			if (wString == null)
				return;

			string newText = wString.GetFormatString();

			if (inputField != null)
				inputField.text = newText;

			foreach (TextMeshProUGUI child in texts)
				child.text = newText;
		}

		public void SubmitInputField()
		{
			if (inputField == null)
				return;

			string newText = inputField.text;
			newText = newText.TrimStart('\n', ' ');
			newText = newText.TrimEnd('\n', ' ');

			bool IsValidText = wString.IsValidText(newText);

			if (IsValidText)
				wString.SetValue(newText);
			else
			{
				// TODO: 직접 InputField에 표시하지 않고 (기존 입력이 날라가니까) 다른 방법으로
				inputField.text = "유효한 문자열이 아닙니다.";
			}
		}
	}
}