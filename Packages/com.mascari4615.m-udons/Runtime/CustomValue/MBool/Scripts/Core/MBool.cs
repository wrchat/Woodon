using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MBool : MEventSender
	{
		[Header("_" + nameof(MBool))]
		[SerializeField] protected bool defaultValue;
		[SerializeField] private bool useSync = true;

		public bool SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private bool _syncedValue;

		public bool Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}
		private bool _value;

		protected virtual void Start()
		{
			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(defaultValue);
			}
			else
			{
				SetValue(defaultValue);
			}

			OnValueChange();
		}

		protected virtual void OnValueChange()
		{
			MDebugLog($"{nameof(OnValueChange)}");

			SendEvents();
		}

		public virtual void SetValue(bool newValue, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetValue)}({newValue})");

			if (useSync)
			{
				if (SyncedValue != newValue)
				{
					if (isReciever == false)
					{
						SetOwner();
						SyncedValue = newValue;
						RequestSerialization();
					}
				}
			}

			Value = newValue;
		}

		// Called By Other Udons
		[ContextMenu(nameof(ToggleValue))]
		public virtual void ToggleValue() => SetValue(!Value);
		
		[ContextMenu(nameof(SetValueTrue))]
		public void SetValueTrue() => SetValue(true);
		
		[ContextMenu(nameof(SetValueFalse))]
		public void SetValueFalse() => SetValue(false);
	}
}
