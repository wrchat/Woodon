using UdonSharp;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public class MEventSender : MBase
	{
		[Header("_" + nameof(MEventSender))]
		[SerializeField] protected UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];
		[SerializeField] protected string[] eventNames = new string[0];
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

		public void RegisterListener(UdonSharpBehaviour newUdon, string eventName)
		{
			if (targetUdons == null)
				targetUdons = new UdonSharpBehaviour[0];

			UdonSharpBehaviour[] newListeners = new UdonSharpBehaviour[targetUdons.Length + 1];
			targetUdons.CopyTo(newListeners, 0);
			newListeners[newListeners.Length - 1] = newUdon;
			targetUdons = newListeners;

			string[] newEventNames = new string[eventNames.Length + 1];
			eventNames.CopyTo(newEventNames, 0);
			newEventNames[newEventNames.Length - 1] = eventName;
			eventNames = newEventNames;
		}

		public void RemoveListener(UdonSharpBehaviour newUdon)
		{
			if (targetUdons == null)
				return;

			int targetIndex = 0;
			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (targetUdons[i] == newUdon)
				{
					targetIndex = i;
					break;
				}
			}

			UdonSharpBehaviour[] newListeners = new UdonSharpBehaviour[targetUdons.Length - 1];
			int index = 0;
			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (i == targetIndex)
					continue;

				newListeners[index] = targetUdons[i];
				index++;
			}
			targetUdons = newListeners;

			string[] newEventNames = new string[eventNames.Length - 1];
			index = 0;
			for (int i = 0; i < eventNames.Length; i++)
			{
				if (i == targetIndex)
					continue;

				newEventNames[index] = eventNames[i];
				index++;
			}
			eventNames = newEventNames;
		}
	}
}