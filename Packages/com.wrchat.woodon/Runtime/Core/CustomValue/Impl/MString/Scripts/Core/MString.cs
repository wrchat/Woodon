using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MString : WEventPublisher
	{
		[field: Header("_" + nameof(MString))]
		[field: SerializeField, TextArea(3, 10)] public string DefaultString { get; protected set; } = string.Empty;

		[field: Header("_" + nameof(MString) + " - Options")]
		[SerializeField] private bool useDefaultWhenEmpty = true;
		[SerializeField] private bool useSync;
		[SerializeField] private bool onlyDigit;
		[SerializeField] private int lengthLimit = 2147483647;

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
				string origin = _value;
				_value = value;
				OnValueChange(origin, _value);
			}
		}

		protected virtual void OnValueChange(string origin, string cur)
		{
			MDebugLog($"{nameof(OnValueChange)} : {origin} -> {cur}");
			SendEvents();
		}

		private void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(DefaultString);
			}
			else
			{
				SetValue(DefaultString);
			}

			OnValueChange(string.Empty, Value);
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
			SetValue(DefaultString);
		}

		public string GetFormatString()
		{
			string formatString = Value;

			if ((formatString == string.Empty) || (formatString.Length == 0))
				if (useDefaultWhenEmpty)
					formatString = DefaultString;

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