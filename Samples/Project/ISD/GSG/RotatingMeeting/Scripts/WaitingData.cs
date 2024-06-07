using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class WaitingData : MBase
    {
        public float CurTime => curTime;
        [UdonSynced(UdonSyncMode.Smooth)] private float curTime = 0;
        [UdonSynced] private bool goUp = false;

        public void SetCurTime(float newTime)
        {
            // MDebugLog(nameof(SetCurTime) + newTime);
            
            SetOwner();
            curTime = newTime;
        }

        public void AddCurTime(float amount)
        {
            SetOwner();

            if (curTime > 13 * RotatingMeetingManager.TimeFromPointToPointByMilli)
            {
                if (goUp)
                {
                    // MDebugLog(nameof(AddCurTime) + amount);
                    // MDebugLog(nameof(AddCurTime));
                    curTime += amount;
                }
            }
            else
            {
                // MDebugLog(nameof(AddCurTime) + amount);
                // MDebugLog(nameof(AddCurTime));
                curTime += amount;
            }
        }

        public void SetGoUp(bool value)
        {
            // MDebugLog(nameof(SetGoUp) + value);
            SetOwner();
            goUp = value;
        }
    }
}