using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public class SendEventOnParticleCollision : WCollisionEventSender
	{
		private void OnParticleCollision(GameObject other)
		{
			if (CheckCondition(other))
				SendEvents();
		}
	}
}