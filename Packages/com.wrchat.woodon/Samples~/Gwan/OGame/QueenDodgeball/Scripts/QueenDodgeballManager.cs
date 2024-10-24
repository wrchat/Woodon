using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QueenDodgeballManager : OGameManagerBase
	{
		[Header("_" + nameof(QueenDodgeballManager))]
		public Transform aRespawn;
		public Transform bRespawn;
		[SerializeField] private float shootPowerDefault = 30;
		[SerializeField] private MValue shootPower; // TODO: Slider
		[SerializeField] private VRC_Pickup shootPos;
		[SerializeField] private VRC_Pickup ballPickup;
		private VRCObjectSync ballObjectSync;
		private Rigidbody ballRigidbody;

		private void Start()
		{
			ballObjectSync = ballPickup.GetComponent<VRCObjectSync>();
			ballRigidbody = ballPickup.GetComponent<Rigidbody>();
		}

		public void ShootTest()
		{
			SetOwner(ballPickup.gameObject);
			ballPickup.transform.SetPositionAndRotation(shootPos.transform.position, shootPos.transform.rotation);
			ballRigidbody.velocity = shootPower.Value * shootPowerDefault * ballPickup.transform.forward;
		}

		public void RespawnBall()
		{
			SetOwner(ballPickup.gameObject);
			ballObjectSync.Respawn();
		}
	}
}