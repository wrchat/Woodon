using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MCameraFovSync : MBase
	{
		private MCameraController mCameraController;

		[SerializeField] private float fovDefault = 60;
		[SerializeField] private float fovSpeed = 1;
		[SerializeField] private float fovMin = 20;
		[SerializeField] private float fovMax = 100;

		private bool isInited = false;

		[UdonSynced(UdonSyncMode.Smooth), FieldChangeCallback(nameof(FovValue))] private float fovValue;
		public float FovValue
		{
			get => fovValue;
			set
			{
				MDebugLog($"{nameof(FovValue)} : {value}");

				fovValue = value;

				if (mCameraController != null)
					mCameraController.FovValue = fovValue;
			}
		}

		public void Init(MCameraController cameraController)
		{
			if (isInited)
				return;
			isInited = true;

			mCameraController = cameraController;
			FovValue = fovDefault;
		}

		private void Update()
		{
			if (isInited == false)
				return;

			UpdateFov();
		}

		private void UpdateFov()
		{
			if (mCameraController.IsPlayerHolding(Networking.LocalPlayer) == false)
				return;
			SetOwner();

			// float scroll = Input.GetAxis("Mouse ScrollWheel");
			float scroll = 0;
			scroll += Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
			scroll -= Input.GetKey(KeyCode.DownArrow) ? 1 : 0;
			
			fovValue += scroll * fovSpeed;
			MDebugLog($"{nameof(UpdateFov)} : +{scroll}");
		
			FovValue = Mathf.Clamp(fovValue, fovMin, fovMax);
		}
	}
}