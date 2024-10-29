using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// 직접 상속 받아 쓸 수 없음
	// Template으로만 볼 것

	public abstract class CustomValue<T> : MEventSender
	{
		[field: Header("_" + nameof(MBool))]
		[field: SerializeField] public T DefaultValue { get; protected set; }
		[SerializeField] protected bool useSync = true;

		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] protected T _syncedValue;
		public T SyncedValue
		{
			get => _syncedValue;
			protected set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}

		protected T _value;
		public T Value
		{
			get => _value;
			protected set
			{
				_value = value;
				OnValueChange();
			}
		}

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (useSync)
			{
				if (Networking.IsMaster)
					SetValue(DefaultValue);
			}
			else
			{
				SetValue(DefaultValue);
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
			// MDebugLog($"{nameof(SetValue)}({newValue})");

			if (isReciever == false)
			{
				if (useSync && IsEqual(Value, newValue) == false)
				{
					SetOwner();
					SyncedValue = newValue;
					RequestSerialization();

					return;
				}
			}

			Value = newValue;
		}

		protected abstract bool IsEqual(T a, T b);
	}
}
