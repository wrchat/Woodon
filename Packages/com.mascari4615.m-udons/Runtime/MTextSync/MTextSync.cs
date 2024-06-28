using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTextSync : MEventSender
	{
		[Header("_" + nameof(MTextSync))]
		[SerializeField] private bool useDefaultWhenEmpty = true;
		[SerializeField] private string defaultString = string.Empty;
		[SerializeField] private TextMeshProUGUI[] texts;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private bool sync;
		[SerializeField] private bool onlyDigit;
		// [SerializeField] private int lengthLimit = 5000;

		[UdonSynced]
		[FieldChangeCallback(nameof(SyncText))]
		private string syncText;

		public string SyncText
		{
			get => syncText;
			set
			{
				syncText = value;
				OnSyncTextChange();
			}
		}

		private void Start()
		{
			if (Networking.IsMaster)
			{
				SyncText = defaultString;
				RequestSerialization();
			}
			else
			{
				OnSyncTextChange();
			}
		}

		private void OnSyncTextChange()
		{
			MDebugLog(nameof(OnSyncTextChange));

			string newText = SyncText;

			if (newText == string.Empty || newText.Length == 0)
				if (useDefaultWhenEmpty)
					newText = defaultString;

			inputField.text = newText;

			foreach (TextMeshProUGUI child in texts)
				child.text = newText;

			SendEvents();
		}

		public void SyncInputFieldText()
		{
			string newText = inputField.text;
			newText = newText.TrimStart('\n', ' ');
			newText = newText.TrimEnd('\n', ' ');

			if (IsVaildText(newText))
				SetSyncText(newText);
		}

		public void SetSyncText(string newText)
		{
			if (sync)
			{
				SetOwner();
				SyncText = newText;
				RequestSerialization();
			}
			else
			{
				SyncText = newText;
			}
		}

		public bool IsVaildText(string targetText)
		{
			if (onlyDigit)
				if (!IsDigit(targetText))
				{
					inputField.text = "숫자가 아닙니다";
					return false;
				}

			/*if (inputField.text.Length > lengthLimit)
			{
				inputField.text = $"글자 제한이 있습니다. ({lengthLimit})";
				return false;
			}*/

			return true;
		}
	}
}