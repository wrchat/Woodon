using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnPickup : WEventPublisher
	{
		public override void OnPickup()
		{
			SendEvents();
		}
	}
}