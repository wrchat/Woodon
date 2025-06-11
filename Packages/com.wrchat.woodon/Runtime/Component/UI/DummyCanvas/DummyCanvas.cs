﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DummyCanvas : UdonSharpBehaviour
	{
		[SerializeField] private GameObject dummyCanvas;
		[SerializeField] private bool followRoot;
		[SerializeField] private bool usePlayerRot;
		[SerializeField] private Vector3 targetOffset = Vector3.zero;
		[SerializeField] private HumanBodyBones targetBone = HumanBodyBones.Head;
		[SerializeField] private float distance = 2;

		[SerializeField] private KeyCode keyCode;
		[SerializeField] private bool useKey;
		[SerializeField] private bool enableWhenGetKey;
		[SerializeField] private bool enableWhenGetKeyDown;

		private void Start()
		{
			if (Networking.LocalPlayer.IsUserInVR())
				gameObject.SetActive(false);
		}

		private void LateUpdate()
		{
			if (!useKey)
				return;

			if (enableWhenGetKey)
			{
				if (Input.GetKeyDown(keyCode))
					dummyCanvas.SetActive(true);
				else if (Input.GetKeyUp(keyCode))
					dummyCanvas.SetActive(false);

				if (Input.GetKey(keyCode))
					UpdatePos();
			}
			else
			{
				if (Input.GetKeyDown(keyCode))
				{
					UpdatePos();

					if (enableWhenGetKeyDown)
						dummyCanvas.SetActive(dummyCanvas.gameObject.activeSelf == false);
				}
			}
		}

		public void ToggleCanvas()
		{
			UpdatePos();
			dummyCanvas.SetActive(!dummyCanvas.gameObject.activeSelf);
		}

		public void UpdatePos()
		{
			if (followRoot)
			{
				dummyCanvas.transform.SetPositionAndRotation(
					Networking.LocalPlayer.GetPosition() + targetOffset,
					usePlayerRot ? Networking.LocalPlayer.GetRotation() : Networking.LocalPlayer.GetRotation());

				dummyCanvas.transform.position += dummyCanvas.transform.forward * distance;
			}
			else
			{
				dummyCanvas.transform.SetPositionAndRotation(
					Networking.LocalPlayer.GetBonePosition(targetBone) + targetOffset,
					usePlayerRot ? Networking.LocalPlayer.GetRotation() : Networking.LocalPlayer.GetBoneRotation(targetBone));

				dummyCanvas.transform.position += dummyCanvas.transform.forward * distance;
			}
		}
	}
}