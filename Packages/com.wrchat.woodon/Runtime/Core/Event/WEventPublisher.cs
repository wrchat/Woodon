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
		[SerializeField] private string[] actions = new string[0];

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
					string action = block["action"].String;

					MDebugLog($"{nameof(SendEvents)}({eventTypeInt}) : {listener}.{action}()");

					if (sendEventGlobal)
						listener.SendCustomNetworkEvent(NetworkEventTarget.All, action);
					else
						listener.SendCustomEvent(action);
				}
			}
		}

		/// <summary>
		/// 호출하는 함수의 접근제한자는 public 이여야 함.
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="action"></param>
		public void RegisterListener(UdonSharpBehaviour listener, string action, Enum eventType = null)
		{
			int eventTypeInt = (eventType == null) ? DEFAULT_EVENT : Convert.ToInt32(eventType);
			MDebugLog($"{nameof(RegisterListener)}({listener}, {action}, {eventTypeInt})");

			if (eventData.ContainsKey(eventTypeInt) == false)
				eventData[eventTypeInt] = new DataList();

			DataList blocks = eventData[eventTypeInt].DataList;

			DataDictionary block = new DataDictionary();
			block.Add("listener", listener);
			block.Add("action", action);

			// 중복 등록 처리
			if (blocks.Contains(block))
			{
				MDebugLog($"{nameof(RegisterListener)} : {nameof(listener)}.{nameof(action)} is already registered", LogType.Warning);
				return;
			}

			blocks.Add(block);
		}

		public void UnregisterListener(UdonSharpBehaviour listener, string action, Enum eventType = null)
		{
			MDebugLog($"{nameof(UnregisterListener)}({listener}, {action}, {eventType})");

			int eventTypeInt = (eventType == null) ? DEFAULT_EVENT : Convert.ToInt32(eventType);

			if (eventData.TryGetValue(eventTypeInt, out DataToken dataToken))
			{
				DataDictionary actionBlock = new DataDictionary();
				actionBlock.Add("listener", listener);
				actionBlock.Add("action", action);

				DataList blocks = dataToken.DataList;
				blocks.Remove(actionBlock);
			}
		}

		#region
		private void InitBaseEvent()
		{
			if (isInited)
				return;

			for (int i = 0; i < listeners.Length; i++)
				RegisterListener(listeners[i], actions[i]);

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
					DataDictionary actionBlock = _listeners[j].DataDictionary;
					UdonSharpBehaviour listener = (UdonSharpBehaviour)actionBlock["listener"].Reference;
					string action = actionBlock["action"].String;

					MDebugLog($"[{key}] {listener}.{action}");
				}
			}
		}
		#endregion
	}
}