using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceRoom : VoiceTagger
	{
		[Header("_" + nameof(VoiceRoom))]
		[SerializeField] private MTarget[] mTargets;
		[field: SerializeField] public MBool[] IsPlayerInside { get; private set; }

		// [SerializeField] private MBool isLocked;
		[SerializeField] private Timer isLocked_Timer;

		public override bool IsCondition(VRCPlayerApi player)
		{
			for (int j = 0; j < mTargets.Length; j++)
			{
				if (mTargets[j].TargetPlayerID == player.playerId && IsPlayerInside[j].Value)
				{
					return true;
				}
			}
			
			return false;
		}

		public void GoRoom()
		{
			// MDebugLog(nameof(GoRoom));

			int localPlayerNum = GetLocalPlayerNum();
			if (localPlayerNum == NONE_INT)
				return;

			int inPlayerCount = 0;
			for (int i = 0; i < mTargets.Length; i++)
			{
				if (IsPlayerInside[i].Value)
					inPlayerCount++;
			}

			if (IsPlayerInside[localPlayerNum].Value)
			{
				IsPlayerInside[localPlayerNum].SetValue(false);

				// if ((inPlayerCount == 1) && (isLocked.Value == true))
				if ((inPlayerCount == 1) && (isLocked_Timer.IsExpiredOrStoped == false))
				{
					// isLocked.SetValue(false);
					isLocked_Timer.ResetTimer();
				}
			}
			else
			{
				// if (isLocked.Value)
				if (isLocked_Timer.IsExpiredOrStoped == false)
					return;

				IsPlayerInside[localPlayerNum].SetValue(true);
			}
		}

		private int GetLocalPlayerNum()
		{
			for (int i = 0; i < mTargets.Length; i++)
			{
				if (mTargets[i].IsTargetPlayer())
					return i;
			}

			return NONE_INT;
		}

		public void Lock()
		{
			// isLocked.SetValue(true);
			isLocked_Timer.StartTimer();
		}
		public void Unlock()
		{
			// isLocked.SetValue(false);
			isLocked_Timer.ResetTimer();
		}

		public void ResetSync()
		{
			for (int i = 0; i < mTargets.Length; i++)
				IsPlayerInside[i].SetValue(false);

			Unlock();
		}
	}
}