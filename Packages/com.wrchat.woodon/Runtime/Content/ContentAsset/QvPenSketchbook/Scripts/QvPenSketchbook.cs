﻿using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QvPenSketchbook : WBase
	{
		[field: Header("_" + nameof(QvPenSketchbook))]
		[field: SerializeField] public RenderTexture SketchbookRenderTexture { get; private set; }
		[SerializeField] private UdonSharpBehaviour[] qvPenManagers;
		[SerializeField] private RawImage[] sketchbookRawImages;
		[SerializeField] private Camera sketchbookCamera;

		[Header("_" + nameof(QvPenSketchbook) + " - Options")]
		[SerializeField] private float screenShotDelay = .3f;
		[SerializeField] private bool defaultCameraActive = false;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			sketchbookCamera.targetTexture = SketchbookRenderTexture;
			foreach (RawImage sketchbookRawImage in sketchbookRawImages)
				sketchbookRawImage.texture = SketchbookRenderTexture;

			SetCameraActive(defaultCameraActive);
		}

		public void ScreenShot_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ScreenShot));
		public void ScreenShot()
		{
			WDebugLog($"{nameof(ScreenShot)}");
			SetCameraActive(true);
			SendCustomEventDelayedSeconds(nameof(TurnOffCamera), screenShotDelay);
		}

		public void TurnOffCamera() => SetCameraActive(false);
		public void SetCameraActive(bool active)
		{
			sketchbookCamera.gameObject.SetActive(active);
		}

		public void ResetQvPen_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetQvPen));
		public void ResetQvPen()
		{
			WDebugLog($"{nameof(ResetQvPen)}");
			ClearQvPen();
			RespawnQvPen();
		}

		public void ClearQvPen_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ClearQvPen));
		public void ClearQvPen()
		{
			WDebugLog($"{nameof(ClearQvPen)}");
			foreach (UdonSharpBehaviour qvPenManager in qvPenManagers)
				qvPenManager.SendCustomEvent("Clear");
		}

		public void RespawnQvPen()
		{
			WDebugLog($"{nameof(RespawnQvPen)}");
			foreach (UdonSharpBehaviour qvPenManager in qvPenManagers)
				qvPenManager.SendCustomEvent("Respawn");
		}
	}
}