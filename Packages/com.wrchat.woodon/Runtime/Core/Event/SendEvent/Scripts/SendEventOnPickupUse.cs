using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class SendEventOnPickupUse : WEventPublisher
	{
		public override void OnPickupUseDown()
		{
			SendEvents();
		}
	}
}