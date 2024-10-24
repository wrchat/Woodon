using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WakVRC.MUtil;

namespace WakVRC
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MString : MEventSender
	{
		[Header("_" + nameof(MString))]

		[Header("_" + nameof(MString) + " - Options")]
		[SerializeField, TextArea(3, 10)] private string defaultString = string.Empty;
		public string DefaultString => defaultString;
		[SerializeField] private bool useDefaultWhenEmpty = true;
		[SerializeField] private bool useSync;
		[SerializeField] private bool onlyDigit;
		[SerializeField] private int lengthLimit = 5000;
		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private string _syncedValue = string.Empty;
		public string SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReceiver: true);
			}
		}

		private string _value = string.Empty;
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
			MDebugLog($"{nameof(Init)}");
			
			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(defaultString);
			}
			else
			{
				SetValue(defaultString);
			}

			OnValueChange();
		}

		public void SetValue(string newValue, bool isReceiver = false)
		{
			if (isReceiver == false)
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

		public void ResetValue()
		{
			SetValue(defaultString);
		}
		
		public string GetFormatString()
		{
			string formatString = Value;

			if (formatString == string.Empty || formatString.Length == 0)
				if (useDefaultWhenEmpty)
					formatString = defaultString;

			return formatString;
		}

		public bool IsValidText(string targetText)
		{
			if (onlyDigit && (IsDigit(targetText) == false))
				return false;

			if (targetText.Length > lengthLimit)
				return false;

			return true;
		}
	}
}