using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WakVRC
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class SendEventOnPickupUse : MEventSender
	{
		public override void OnPickupUseDown()
		{
			SendEvents();
		}
	}
}