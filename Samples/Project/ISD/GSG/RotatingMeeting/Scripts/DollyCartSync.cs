using System;
using Cinemachine;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
    // [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DollyCartSync : MBase
    {
        public float Position { get; private set; }

        [SerializeField] private CinemachineDollyCart cinemachineDollyCart;
        [SerializeField] private RotatingMeetingManager rotatingMeetingManager;
        [SerializeField] private TextMeshPro asd;
        
        public void SetPosition(float newPosition)
        {
            Position = newPosition;
            asd.text = Position.ToString();
            
            cinemachineDollyCart.m_Position = Position;
            
            bool isManager = IsOwner(rotatingMeetingManager.gameObject);
            cinemachineDollyCart.enabled = isManager;
            if (isManager)
                SetOwner();
        }
    }
}