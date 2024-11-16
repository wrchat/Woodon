using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnUseDown : WEventPublisher
	{
		public override void OnPickupUseDown()
		{
			SendEvents();
		}
	}
}