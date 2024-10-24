using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// 직접 상속 받아 쓸 수 없음
	// Template으로만 볼 것

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public abstract class CustomValue<T> : MEventSender where T : IComparable
	{
		[Header("_" + nameof(MBool))]
		[SerializeField] protected T defaultValue;
		[SerializeField] private bool useSync = true;

		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private T _syncedValue;
		public T SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}

		private T _value;
		public T Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}

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

		public virtual void SetValue(T newValue, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetValue)}({newValue})");

			if (isReciever == false)
			{
				if (useSync && SyncedValue.Equals(newValue) == false)
				{
					SetOwner();
					SyncedValue = newValue;
					RequestSerialization();

					return;
				}
			}

			Value = newValue;
		}
	}
}
