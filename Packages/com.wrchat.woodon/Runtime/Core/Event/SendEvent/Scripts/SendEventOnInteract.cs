using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnInteract : WEventPublisher
	{
		public override void Interact()
		{
			SendEvents();
		}
	}
}