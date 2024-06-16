using System;
using QvPen.UdonScript;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QvPenSketchbook : MBase
	{
		[SerializeField] private GameObject[] sketchbooks;
		[SerializeField] private QvPen_PenManager[] team0qvpens;

		private Camera[] sketchbookCameras;

		private void Start()
		{
			sketchbookCameras = new Camera[sketchbooks.Length];
			ScreenShot();
			for (var i = 0; i < sketchbooks.Length; i++)
				sketchbookCameras[i] = sketchbooks[i].GetComponentInChildren<Camera>();
		}

		[SerializeField] private float screenShotDelay = .3f;

		public void ScreenShot_Delay()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ScreenShot2));
		}

		public void ScreenShot2()
		{
			foreach (var sketchbookCamera in sketchbookCameras)
				sketchbookCamera.gameObject.SetActive(true);
			SendCustomEventDelayedSeconds(nameof(ScreenShot), screenShotDelay);
		}

		public void ScreenShot()
		{
			foreach (var sketchbookCamera in sketchbookCameras)
				sketchbookCamera.gameObject.SetActive(false);
		}

		public void ResetQVPen_Global()
		{
			MDebugLog($"{nameof(ResetQVPen_Global)}");

			SetOwner();
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetQVPen));
		}

		public void ResetQVPen()
		{
			MDebugLog($"{nameof(ResetQVPen)}");

			foreach (var team0qvpen in team0qvpens)
				team0qvpen.Clear();
		}

		[Header("TEMP")]
		[SerializeField] private GameObject targetObject;
		[SerializeField] private KeyCode targetKeyCode = KeyCode.Backspace;
		private void Update()
		{
			if (Input.GetKeyDown(targetKeyCode))
			{
				targetObject.SetActive(false);
			}
		}
	}
}