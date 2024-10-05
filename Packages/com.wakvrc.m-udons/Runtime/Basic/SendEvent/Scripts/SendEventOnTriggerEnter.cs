using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WakVRC
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnTriggerEnter : MCollisionEventSender
	{
		private void OnTriggerEnter(Collider other)
		{
			if (CheckCondition(other.gameObject))
				SendEvents();
		}
	}
}