using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
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