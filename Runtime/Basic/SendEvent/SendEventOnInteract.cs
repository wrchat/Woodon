using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnInteract : MEventSender
	{
		public override void Interact()
		{
			SendEvents();
		}
	}
}