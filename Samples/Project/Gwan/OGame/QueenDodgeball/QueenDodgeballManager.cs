using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class QueenDodgeballManager : OGameManagerBase
    {
        [Header("_" + nameof(QueenDodgeballManager))]
        public Transform aRespawn;
        public Transform bRespawn;
        [SerializeField] private float shootPowerDefault = 30;
        [SerializeField] private SyncedSlider shootPower;
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
            ballPickup.transform.position = shootPos.transform.position;
            ballPickup.transform.rotation = shootPos.transform.rotation;
            ballRigidbody.velocity = ballPickup.transform.forward * shootPower.CurValue * shootPowerDefault;
        }

        public void RespawnBall()
        {
            SetOwner(ballPickup.gameObject);
            ballObjectSync.Respawn();
        }
    }
}