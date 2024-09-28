using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MString : MEventSender
	{
		[Header("_" + nameof(MString))]

		[Header("_" + nameof(MString) + " - Options")]
		[SerializeField] private string defaultString = string.Empty;
		[SerializeField] private bool useDefaultWhenEmpty = true;
		[SerializeField] private bool useSync;
		[SerializeField] private bool onlyDigit;
		[SerializeField] private int lengthLimit = 5000;
		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private string _syncedValue;
		public string SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}

		private string _value;
		public string Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}

		private void OnValueChange()
		{
			MDebugLog(nameof(OnValueChange));

			SendEvents();
		}

		private void Start()
		{
			Init();
		}
		
		private void Init()
		{
			if (Networking.IsMaster)
			{
				Value = defaultString;
				RequestSerialization();
			}
			else
			{
				OnValueChange();
			}
		}

		public void SetValue(string newValue, bool isReciever = false)
		{
			if (isReciever == false)
			{
				if (useSync && SyncedValue != newValue)
				{
					SetOwner();
					SyncedValue = newValue;
					RequestSerialization();

					return;
				}
			}

			Value = newValue;
		}

		public string GetFormatString()
		{
			string formatString = Value;

			if (formatString == string.Empty || formatString.Length == 0)
				if (useDefaultWhenEmpty)
					formatString = defaultString;

			return formatString;
		}

		public bool IsVaildText(string targetText)
		{
			if (onlyDigit && (IsDigit(targetText) == false))
				return false;

			if (targetText.Length > lengthLimit)
				return false;

			return true;
		}
	}
}