using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public class MEventSender : MBase
	{
		[Header("_" + nameof(MEventSender))]
		[SerializeField] protected UdonSharpBehaviour[] targetUdons;
		[SerializeField] protected string[] eventNames;
		[SerializeField] protected bool sendGlobal;

		protected void SendEvents()
		{
			MDebugLog($"{nameof(SendEvents)}");

			if (IsEventValid() == false)
				return;

			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (sendGlobal)
					targetUdons[i].SendCustomNetworkEvent(NetworkEventTarget.All, eventNames[i]);
				else
					targetUdons[i].SendCustomEvent(eventNames[i]);
			}
		}

		protected void SendEvent(int index)
		{
			MDebugLog($"{nameof(SendEvent)}, {nameof(index)} = {index}");

			if (IsEventValid() == false)
				return;

			if (sendGlobal)
				targetUdons[index].SendCustomNetworkEvent(NetworkEventTarget.All, eventNames[index]);
			else
				targetUdons[index].SendCustomEvent(eventNames[index]);
		}

		private bool IsEventValid()
		{
			if (targetUdons == null || targetUdons.Length == 0)
				return false;

			if (eventNames == null || eventNames.Length == 0)
			{
				MDebugLog($"{nameof(SendEvents)} : No Events");
				return false;
			}

			return true;
		}
	}
}