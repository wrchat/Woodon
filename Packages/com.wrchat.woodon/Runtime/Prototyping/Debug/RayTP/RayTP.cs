using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RayTP : MBase
	{
		[SerializeField] private float distance;
		private RaycastHit raycastHit;
		private Ray ray;

		[SerializeField] private LayerMask layerMask;

		public void TryRayTP()
		{
			MDebugLog($"{nameof(TryRayTP)}");

			// Ray 쏴서 해당 위치로 TP
			ray.origin = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
			ray.direction = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Head) * Vector3.forward;
			Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

			if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, distance, layerMask))
			{
				Networking.LocalPlayer.TeleportTo(raycastHit.point, raycastHit.transform.rotation);
			}
		}
	}
}