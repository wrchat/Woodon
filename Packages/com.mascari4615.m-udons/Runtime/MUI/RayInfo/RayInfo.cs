using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RayInfo : MBase
	{
		[Header("_" + nameof(RayInfo))]
		[SerializeField] private Transform rayObject;
		[SerializeField] private float distance;
		private RaycastHit raycastHit;
		private Ray ray;
		private bool lastFrameStringChanged = false;

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
				string newString = GetString(raycastHit.collider.gameObject);
				bool stringChanged = ui.text != newString;

				ui.text = newString;

				if (lastFrameStringChanged)
				{
					lastFrameStringChanged = false;
					
					rayOnObject.SetActive(false);
				}

				rayOnObject.SetActive(true);

				if (stringChanged)
				{
					lastFrameStringChanged = true;

					rayOnObject.SetActive(false);
					rayOnObject.SetActive(true);
				}
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