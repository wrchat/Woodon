using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RayInfo : MBase
	{
		[SerializeField] private Transform rayObject;
		[SerializeField] private float distance;
		private RaycastHit raycastHit;
		private Ray ray;

		[SerializeField] private LayerMask layerMask;
		[SerializeField] private TextMeshProUGUI ui;
		[SerializeField] private GameObject rayOnObject;

		private void Start()
		{
			ray = new Ray();
		}

		protected virtual void Update()
		{
			if (rayObject)
			{
				ray.origin = rayObject.position;
				ray.direction = rayObject.forward;
			}
			else
			{
				ray.origin = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
				ray.direction = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Head) * Vector3.forward;
				Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
			}

			if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, distance, layerMask))
			{
				ui.text = GetString(raycastHit.collider.gameObject);
				rayOnObject.SetActive(true);
			}
			else
			{
				ui.text = string.Empty;
				rayOnObject.SetActive(false);
			}
		}

		protected virtual string GetString(GameObject obj)
		{
			return obj.name;
		}
	}
}