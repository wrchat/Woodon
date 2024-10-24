using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WakVRC
{
	public class SendEventOnParticleCollision : MCollisionEventSender
	{
		private void OnParticleCollision(GameObject other)
		{
			if (CheckCondition(other))
				SendEvents();
		}
	}
}