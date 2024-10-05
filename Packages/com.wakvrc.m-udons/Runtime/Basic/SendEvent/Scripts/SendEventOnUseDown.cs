using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WakVRC
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnUseDown : MEventSender
	{
		public override void OnPickupUseDown()
		{
			SendEvents();
		}
	}
}