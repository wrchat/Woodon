using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QvPenSketchbook : MBase
	{
		[SerializeField] private GameObject[] sketchbooks;
		[SerializeField] private UdonSharpBehaviour[] qvPenManagers;

		private Camera[] sketchbookCameras;
		[SerializeField] private float screenShotDelay = .3f;

		private void Start()
		{
			sketchbookCameras = new Camera[sketchbooks.Length];
			for (int i = 0; i < sketchbooks.Length; i++)
				sketchbookCameras[i] = sketchbooks[i].GetComponentInChildren<Camera>();
			SetCameraActive(false);
		}


		public void ScreenShot_G()
		{
			MDebugLog($"{nameof(ScreenShot_G)}");
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ScreenShot));
		}

		public void ScreenShot()
		{
			MDebugLog($"{nameof(ScreenShot)}");
			SetCameraActive(true);
			SendCustomEventDelayedSeconds(nameof(TurnOffCameras), screenShotDelay);
		}

		public void TurnOnCameras() => SetCameraActive(true);
		public void TurnOffCameras() => SetCameraActive(false);

		public void SetCameraActive(bool active)
		{
			foreach (Camera sketchbookCamera in sketchbookCameras)
				sketchbookCamera.gameObject.SetActive(active);
		}

		public void ResetQVPen_G()
		{
			MDebugLog($"{nameof(ResetQVPen_G)}");
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetQVPen));
		}

		public void ResetQVPen()
		{
			MDebugLog($"{nameof(ResetQVPen)}");
			foreach (UdonSharpBehaviour qvPenManager in qvPenManagers)
				qvPenManager.SendCustomEvent("Clear");
		}
	}
}