using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RayInfo : MBase
	{
		[Header("_" + nameof(RayInfo))]
		[SerializeField] private HumanBodyBones bone = HumanBodyBones.Head;
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
				ray.origin = Networking.LocalPlayer.GetBonePosition(bone);
				ray.direction = Networking.LocalPlayer.GetBoneRotation(bone) * Vector3.forward;
				Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
			}

			if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, distance, layerMask))
			{
				GameObject targetObject = raycastHit.collider.gameObject;

				if (IsTarget(targetObject) == false)
				{
					ui.text = string.Empty;
					rayOnObject.SetActive(false);
					return;
				}

				string newString = GetString(targetObject);
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

		protected virtual bool IsTarget(GameObject obj)
		{
			return true;
		}

		protected virtual string GetString(GameObject obj)
		{
			return obj.name;
		}
	}
}