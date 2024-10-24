using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RespawnObjectSync : MBase
	{
		[SerializeField] private VRCObjectSync[] objectSyncs;
		private Vector3[] originalLocalPositions;
		private Quaternion[] originalLocalRotations;

		private void Start()
		{
			originalLocalPositions = new Vector3[objectSyncs.Length];
			originalLocalRotations = new Quaternion[objectSyncs.Length];
			for (int i = 0; i < objectSyncs.Length; i++)
			{
				originalLocalPositions[i] = objectSyncs[i].transform.localPosition;
				originalLocalRotations[i] = objectSyncs[i].transform.localRotation;
			}
		}

		public void RespawnAll()
		{
			foreach (var objectSync in objectSyncs)
			{
				SetOwner(objectSync.gameObject);
				objectSync.Respawn();
			}
		}

		// LocalPosition 대응
		public void RespawnAll_LocalTransform()
		{
			for (int i = 0; i < objectSyncs.Length; i++)
			{
				Transform target = objectSyncs[i].transform;

				SetOwner(target.gameObject);
				target.SetLocalPositionAndRotation(originalLocalPositions[i], originalLocalRotations[i]);
			}
		}
	}
}