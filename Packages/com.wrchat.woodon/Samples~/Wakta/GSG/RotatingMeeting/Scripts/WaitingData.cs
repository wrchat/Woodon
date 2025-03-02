using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class WaitingData : MBase
	{
		[field: Header("_" + nameof(WaitingData))]
		[field: UdonSynced(UdonSyncMode.Smooth)] public float CurTime { get; private set; } = 0;
		[UdonSynced] private bool isUploading = false;

		public void SetCurTime(float newTime)
		{
			// WDebugLog(nameof(SetCurTime) + newTime);

			SetOwner();
			CurTime = newTime;
		}

		public void AddCurTime(float amount)
		{
			SetOwner();

			if (CurTime > 13 * RotatingMeetingManager.TIME_FROM_POINT_TO_POINT_BY_MILLI)
			{
				if (isUploading)
				{
					// WDebugLog(nameof(AddCurTime) + amount);
					// WDebugLog(nameof(AddCurTime));
					CurTime += amount;
				}
			}
			else
			{
				// WDebugLog(nameof(AddCurTime) + amount);
				// WDebugLog(nameof(AddCurTime));
				CurTime += amount;
			}
		}

		public void SetUpload(bool value)
		{
			// WDebugLog(nameof(SetGoUp) + value);
			SetOwner();
			isUploading = value;
		}
	}
}