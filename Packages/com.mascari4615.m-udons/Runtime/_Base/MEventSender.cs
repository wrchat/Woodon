using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public class MEventSender : MBase
	{
		[Header("_" + nameof(MEventSender))]
		[SerializeField] protected UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];
		[SerializeField] protected string[] eventNames = new string[0];
		[SerializeField] protected bool sendGlobal;

		protected UdonSharpBehaviour[][] targetUdonss = new UdonSharpBehaviour[0][];
		protected string[][] eventNamess = new string[0][];

		// ---- ---- ---- ----

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

		// protected void SendEvent(int index)
		// {
		// 	MDebugLog($"{nameof(SendEvent)}, {nameof(index)} = {index}");

		// 	if (IsEventValid() == false)
		// 		return;

		// 	if (sendGlobal)
		// 		targetUdons[index].SendCustomNetworkEvent(NetworkEventTarget.All, eventNames[index]);
		// 	else
		// 		targetUdons[index].SendCustomEvent(eventNames[index]);
		// }

		protected void SendEvents(int index)
		{
			if (IsEventValid() == false)
				return;

			UdonSharpBehaviour[] targetUdons = targetUdonss[index];
			string[] eventNames = eventNamess[index];

			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (sendGlobal)
					targetUdons[i].SendCustomNetworkEvent(NetworkEventTarget.All, eventNames[i]);
				else
					targetUdons[i].SendCustomEvent(eventNames[i]);
			}
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

		/// <summary>
		/// 호출하는 이벤트의 접근제한자는 public 이여야 함.
		/// </summary>
		/// <param name="newUdon"></param>
		/// <param name="eventName"></param>
		public void RegisterListener(UdonSharpBehaviour newUdon, string eventName, int index = NONE_INT)
		{
			if (index == NONE_INT)
				RegisterListener_(ref targetUdons, ref eventNames, newUdon, eventName);
			else
			{
				if (targetUdonss == null)
					targetUdonss = new UdonSharpBehaviour[0][];

				if (eventNamess == null)
					eventNamess = new string[0][];

				if (targetUdonss.Length <= index)
					MDataUtil.ResizeArr(ref targetUdonss, index + 1);

				if (eventNamess.Length <= index)
					MDataUtil.ResizeArr(ref eventNamess, index + 1);

				RegisterListener_(ref targetUdonss[index], ref eventNamess[index], newUdon, eventName);
			}
		}
		
		private void RegisterListener_(ref UdonSharpBehaviour[] targetUdons, ref string[] eventNames, UdonSharpBehaviour newUdon, string eventName)
		{
			if (targetUdons == null)
				targetUdons = new UdonSharpBehaviour[0];

			if (eventNames == null)
				eventNames = new string[0];

			MDebugLog($"{nameof(RegisterListener_)} : {newUdon.name}, {eventName}");

			MDataUtil.ResizeArr(ref targetUdons, targetUdons.Length + 1);
			targetUdons[targetUdons.Length - 1] = newUdon;

			MDataUtil.ResizeArr(ref eventNames, eventNames.Length + 1);
			eventNames[eventNames.Length - 1] = eventName;
		}

		public void RemoveListener(UdonSharpBehaviour targetUdon, string targetEventName, int index = NONE_INT)
		{
			if (index == NONE_INT)
				RemoveListener_(ref targetUdons, ref eventNames, targetUdon, targetEventName);
			else
			{
				if (targetUdonss == null || targetUdonss.Length <= index)
					return;

				if (eventNamess == null || eventNamess.Length <= index)
					return;

				RemoveListener_(ref targetUdonss[index], ref eventNamess[index], targetUdon, targetEventName);
			}
		}

		private void RemoveListener_(ref UdonSharpBehaviour[] targetUdons, ref string[] eventNames, UdonSharpBehaviour targetUdon, string targetEventName)
		{
			if (targetUdons == null || targetUdons.Length == 0)
				return;

			if (eventNames == null || eventNames.Length == 0)
				return;

			MDebugLog($"{nameof(RemoveListener_)} : {targetUdon.name}");

			int targetIndex = 0;
			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (targetUdons[i] == targetUdon && eventNames[i] == targetEventName)
				{
					targetIndex = i;
					break;
				}
			}

			MDataUtil.RemoveAt(ref targetUdons, targetIndex);
			MDataUtil.RemoveAt(ref eventNames, targetIndex);
		}
	}
}