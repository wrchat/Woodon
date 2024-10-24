using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class TransformSync : MBase
    {
        [UdonSynced] private Vector3 syncPos;
        [UdonSynced] private Quaternion syncRot;

        private void Start()
        {
            UpdateTransform();
        }

        private void Update()
        {
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            transform.position = syncPos;
            transform.rotation = syncRot;
        }

        public void SetSyncPos(Vector3 newPos)
        {
            SetOwner();
            syncPos = newPos;
        }

        private void SetSyncRot(Quaternion newRot)
        {
            SetOwner();
            syncRot = newRot;
        }
    }
}