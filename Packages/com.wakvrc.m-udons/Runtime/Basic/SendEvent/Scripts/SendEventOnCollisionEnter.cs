using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WakVRC
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnCollisionEnter : MCollisionEventSender
	{
		private void OnCollisionEnter(Collision collision)
		{
			if (CheckCondition(collision.gameObject))
				SendEvents();
		}
	}
}