using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
    public class PosFixer : MBase
    {
        [SerializeField] private GameObject meshObject;

        // [SerializeField] private bool FreezeRotationX = false;
        // [SerializeField] private bool FreezeRotationY = false;
        // [SerializeField] private bool FreezeRotationZ = false;

        [SerializeField] private bool FreezePositionX;
        [SerializeField] private bool FreezePositionY;
        [SerializeField] private bool FreezePositionZ;

        private Vector3 originPos;
        private Quaternion originRot;
        private VRC_Pickup pickup;
        public Vector3 OriginPos => originPos;

        private void Start()
        {
            pickup = GetComponent<VRC_Pickup>();

            originPos = transform.localPosition;
            originRot = transform.localRotation;
        }

        private void Update()
        {
            meshObject.transform.localPosition = FixLocalPosition();

            if (!IsOwner())
                return;

            FixRotation();

            if (pickup.IsHeld)
            {
            }
            else
            {
                transform.localPosition = FixLocalPosition();
            }
        }

        private void FixRotation()
        {
            var curLocalRot = transform.localRotation.eulerAngles;

            /*if (FreezeRotationX)
                curLocalRot.x = originRot.eulerAngles.x;
            if (FreezeRotationY)
                curLocalRot.y = originRot.eulerAngles.y;
            if (FreezeRotationZ)
                curLocalRot.z = originRot.eulerAngles.z;*/
            // transform.localRotation = Quaternion.Euler(curLocalRot);

            // 그날 본 좌표계의 원리를 난 아직 모른다.

            transform.localRotation = originRot;
        }

        private Vector3 FixLocalPosition()
        {
            var curLocalPos = transform.localPosition;

            if (FreezePositionX)
                curLocalPos.x = originPos.x;

            if (FreezePositionY)
                curLocalPos.y = originPos.y;

            if (FreezePositionZ)
                curLocalPos.z = originPos.z;

            curLocalPos = new Vector3(Mathf.Clamp(curLocalPos.x, -.665f, .665f),
                Mathf.Clamp(curLocalPos.y, 1.004f, 1.836f), curLocalPos.z);

            return curLocalPos;
        }
    }
}