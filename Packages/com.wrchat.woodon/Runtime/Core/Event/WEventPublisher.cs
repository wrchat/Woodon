using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public abstract class WEventPublisher : MBase
	{
		public const int DEFAULT_EVENT = -1;

		[Header("_" + nameof(WEventPublisher))]

		// 아래 두 필드는 eventData에 '-1 eventType'으로 저장됩니다. (SendEvents(null) 호출 시 실행됨)
		[SerializeField] private UdonSharpBehaviour[] listeners = new UdonSharpBehaviour[0];
		[SerializeField] private string[] callbacks = new string[0];

		[Header("_" + nameof(WEventPublisher) + " - Options")]
		[SerializeField] private bool sendEventGlobal;

		private readonly DataDictionary eventData = new DataDictionary();
		private bool isInited = false;

		protected void SendEvents(Enum eventType = null)
		{
			InitBaseEvent();

			int eventTypeInt = (eventType == null) ? DEFAULT_EVENT : Convert.ToInt32(eventType);

			if (eventData.TryGetValue(eventTypeInt, out DataToken dataToken))
			{
				DataList blocks = dataToken.DataList;

				for (int i = 0; i < blocks.Count; i++)
				{
					DataDictionary block = blocks[i].DataDictionary;
					UdonSharpBehaviour listener = (UdonSharpBehaviour)block["listener"].Reference;
					string callback = block["callback"].String;

					MDebugLog($"{nameof(SendEvents)}({eventTypeInt}) : {listener}.{callback}()");

					if (sendEventGlobal)
						listener.SendCustomNetworkEvent(NetworkEventTarget.All, callback);
					else
						listener.SendCustomEvent(callback);
				}
			}
		}

		/// <summary>
		/// 호출하는 함수의 접근제한자는 public 이여야 함.
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="callback"></param>
		public void RegisterListener(UdonSharpBehaviour listener, string callback, Enum eventType = null)
		{
			MDebugLog($"{nameof(RegisterListener)}({listener}, {callback}, {eventType})");
			
			int eventTypeInt = ConvertEventTypeToInt(eventType);
			if (eventData.ContainsKey(eventTypeInt) == false)
				eventData[eventTypeInt] = new DataList();

			DataList eventBlocks = eventData[eventTypeInt].DataList;
			DataDictionary eventBlock = new DataDictionary();
			eventBlock.Add("listener", listener);
			eventBlock.Add("callback", callback);

			// 중복 등록 처리
			if (AlreadyContainsEvent(eventBlocks, eventBlock))
			{
				MDebugLog($"{nameof(RegisterListener)} : {listener}.{callback} is already registered", LogType.Warning);
				return;
			}

			eventBlocks.Add(eventBlock);
		}

		private bool AlreadyContainsEvent(DataList eventBlocks, DataDictionary eventBlock)
		{
			for (int i = 0; i < eventBlocks.Count; i++)
			{
				DataDictionary block = eventBlocks[i].DataDictionary;
				if (block["listener"].Reference == eventBlock["listener"].Reference && block["callback"].String == eventBlock["callback"].String)
					return true;
			}

			return false;
		}

		public void UnregisterListener(UdonSharpBehaviour listener, string callback, Enum eventType = null)
		{
			MDebugLog($"{nameof(UnregisterListener)}({listener}, {callback}, {eventType})");

			int eventTypeInt = ConvertEventTypeToInt(eventType);
			if (eventData.TryGetValue(eventTypeInt, out DataToken dataToken))
			{
				DataDictionary eventBlock = new DataDictionary();
				eventBlock.Add("listener", listener);
				eventBlock.Add("callback", callback);

				DataList eventBlocks = dataToken.DataList;
				eventBlocks.Remove(eventBlock);
			}
		}

		private int ConvertEventTypeToInt(Enum eventType)
		{
			return eventType == null ? DEFAULT_EVENT : Convert.ToInt32(eventType);
		}

		#region
		private void InitBaseEvent()
		{
			if (isInited)
				return;

			for (int i = 0; i < listeners.Length; i++)
				RegisterListener(listeners[i], callbacks[i]);

			isInited = true;
		}

		public void DebugAll()
		{
			InitBaseEvent();

			DataList keys = eventData.GetKeys();

			for (int i = 0; i < keys.Count; i++)
			{
				DataToken key = keys[i];
				DataList _listeners = eventData[key].DataList;

				for (int j = 0; j < _listeners.Count; j++)
				{
					DataDictionary eventBlock = _listeners[j].DataDictionary;
					UdonSharpBehaviour listener = (UdonSharpBehaviour)eventBlock["listener"].Reference;
					string callback = eventBlock["callback"].String;

					MDebugLog($"[{key}].{j} : {listener}.{callback}");
				}
			}
		}
		#endregion
	}
}