using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
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