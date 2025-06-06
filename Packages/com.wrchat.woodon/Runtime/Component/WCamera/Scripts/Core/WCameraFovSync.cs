﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class WCameraFovSync : WBase
	{
		[Header("_" + nameof(WCameraFovSync))]
		[SerializeField] private bool useSync = true;
		[SerializeField] private float fovDefault = 60;
		[SerializeField] private float fovSpeed = 1;
		[SerializeField] private float fovMin = 20;
		[SerializeField] private float fovMax = 60;

		public float Value { get; private set; }
		
		[UdonSynced(UdonSyncMode.Smooth), FieldChangeCallback(nameof(SyncedValue))] private float _syncedValue;
		public float SyncedValue
		{
			get => _syncedValue;
			set
			{
				_syncedValue = value;
				Value = value;
			}
		}

		private WCameraController cameraController;

		public void Init(WCameraController cameraController)
		{
			this.cameraController = cameraController;
			SetValue(fovDefault);
		}

		public void SetValue(float value)
		{
			if (useSync)
			{
				SetOwner();
				SyncedValue = value;
			}
			else
			{
				Value = value;
			}
		}

		private void Update()
		{
			UpdateFov();
		}

		private void UpdateFov()
		{
			if (cameraController == null)
				return;

			if (cameraController.IsHolding() == false)
				return;

			// float scroll = Input.GetAxis("Mouse ScrollWheel");
			float scroll = 0;
			scroll += Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
			scroll -= Input.GetKey(KeyCode.DownArrow) ? 1 : 0;

			float newValue = SyncedValue + scroll * fovSpeed;
			newValue = Mathf.Clamp(newValue, fovMin, fovMax);

			SetValue(newValue);
			WDebugLog($"{nameof(UpdateFov)} : +{scroll} = {newValue}");
		}
	}
}