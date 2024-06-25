using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
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

		/// <summary>
		/// 호출하는 이벤트의 접근제한자는 public 이여야 함.
		/// </summary>
		/// <param name="newUdon"></param>
		/// <param name="eventName"></param>
		public void RegisterListener(UdonSharpBehaviour newUdon, string eventName)
		{
			if (targetUdons == null)
				targetUdons = new UdonSharpBehaviour[0];

			MDebugLog($"AAA :: {nameof(targetUdons)} = {targetUdons.Length}");
			MDataUtil.ResizeArr(ref targetUdons, targetUdons.Length + 1);
			targetUdons[targetUdons.Length - 1] = newUdon;
			MDebugLog($"BBB :: {nameof(targetUdons)} = {targetUdons.Length}");

			MDataUtil.ResizeArr(ref eventNames, eventNames.Length + 1);
			eventNames[eventNames.Length - 1] = eventName;
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

			MDataUtil.RemoveAt(ref targetUdons, targetIndex);
			MDataUtil.RemoveAt(ref eventNames, targetIndex);
		}
	}
}