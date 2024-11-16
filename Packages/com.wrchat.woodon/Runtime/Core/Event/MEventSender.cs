using System;
using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public abstract class MEventSender : MBase
	{
		public const int DEFAULT_EVENT = -1;

		[Header("_" + nameof(MEventSender))]
		
		// 기본 이벤트
		[SerializeField] protected UdonSharpBehaviour[] listeners = new UdonSharpBehaviour[0];
		[SerializeField] protected string[] actions = new string[0];

		[Header("_" + nameof(MEventSender) + " - Options")]
		[SerializeField] protected bool sendEventGlobal;

		// 추가 정의 이벤트
		protected UdonSharpBehaviour[][] specificEventListeners = new UdonSharpBehaviour[0][];
		protected string[][] specificEventActions = new string[0][];

		// TODO: DataDictionary

		// ---- ---- ---- ----

		protected void SendEvents(Enum eventType = null)
		{
			int eventTypeInt = (eventType == null) ? DEFAULT_EVENT : Convert.ToInt32(eventType);
			if (IsEventValid(eventTypeInt) == false)
				return;

			UdonSharpBehaviour[] _listeners = GetListeners(eventTypeInt);
			string[] _actions = GetActions(eventTypeInt);

			for (int i = 0; i < _listeners.Length; i++)
			{
				MDebugLog($"{nameof(SendEvents)}({eventTypeInt})[{i}] : {_listeners[i]}.{_actions[i]}()");

				if (_listeners[i] == null)
				{
					MDebugLog($"{nameof(SendEvents)} : {nameof(_listeners)}[{i}] is null, Skip {_actions[i]}()", LogType.Error);
					continue;
				}

				if (sendEventGlobal)
					_listeners[i].SendCustomNetworkEvent(NetworkEventTarget.All, _actions[i]);
				else
					_listeners[i].SendCustomEvent(_actions[i]);
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

			if (eventTypeInt != DEFAULT_EVENT)
			{
				if (specificEventListeners.Length <= eventTypeInt)
				{
					MDataUtil.ResizeArr(ref specificEventListeners, eventTypeInt + 1);

					for (int i = 0; i < specificEventListeners.Length; i++)
					{
						if (specificEventListeners[i] == null)
							specificEventListeners[i] = new UdonSharpBehaviour[0];
					}
				}

				if (specificEventActions.Length <= eventTypeInt)
				{
					MDataUtil.ResizeArr(ref specificEventActions, eventTypeInt + 1);
					specificEventActions[eventTypeInt] = new string[0];

					for (int i = 0; i < specificEventActions.Length; i++)
					{
						if (specificEventActions[i] == null)
							specificEventActions[i] = new string[0];
					}
				}
			}

			UdonSharpBehaviour[] _listeners = GetListeners(eventTypeInt);
			string[] _actions = GetActions(eventTypeInt);

			// 중복 등록 처리
			for (int i = 0; i < _listeners.Length; i++)
			{
				bool isSameTarget = _listeners[i] == listener;
				bool isSameAction = _actions[i] == action;

				if (isSameTarget && isSameAction)
				{
					MDebugLog($"{nameof(RegisterListener)} : {nameof(listener)}.{nameof(action)} is already registered", LogType.Warning);
					return;
				}
			}

			// 새로운 우동과 이벤트를 추가
			MDataUtil.Add(ref _listeners, listener);
			MDataUtil.Add(ref _actions, action);

			// 원본 배열에 수정된 배열을 대입
			if (eventTypeInt != DEFAULT_EVENT)
			{
				specificEventListeners[eventTypeInt] = _listeners;
				specificEventActions[eventTypeInt] = _actions;
			}
			else
			{
				listeners = _listeners;
				actions = _actions;
			}
		}

		public void UnregisterListener(UdonSharpBehaviour listener, string action, Enum eventType = null)
		{
			MDebugLog($"{nameof(UnregisterListener)}({listener}, {action}, {eventType})");

			int eventTypeInt = (eventType == null) ? DEFAULT_EVENT : Convert.ToInt32(eventType);

			if (eventTypeInt != DEFAULT_EVENT)
			{
				if (specificEventListeners.Length <= eventTypeInt)
					return;

				if (specificEventActions.Length <= eventTypeInt)
					return;
			}

			UdonSharpBehaviour[] listeners = GetListeners(eventTypeInt);
			string[] actions = GetActions(eventTypeInt);

			int targetIndex = NONE_INT;
			for (int i = 0; i < listeners.Length; i++)
			{
				if (listeners[i] == listener && actions[i] == action)
				{
					targetIndex = i;
					break;
				}
			}

			if (targetIndex == NONE_INT)
			{
				MDebugLog($"{nameof(UnregisterListener)} : {nameof(listener)}.{nameof(action)} is not found", LogType.Warning);
				return;
			}

			MDataUtil.RemoveAt(ref listeners, targetIndex);
			MDataUtil.RemoveAt(ref actions, targetIndex);
		}

		#region
		private bool IsEventValid(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
			{
				if (listeners == null || actions == null)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(listeners)} or {nameof(actions)} is null", LogType.Error);
					return false;
				}

				if (listeners.Length == 0 || actions.Length == 0)
				{
					// MDebugLog($"{nameof(IsEventValid)} : {nameof(targetUdons)} or {nameof(eventNames)} is empty", LogType.Warning);
					return false;
				}
			}
			else
			{
				if (specificEventListeners == null || specificEventActions == null)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(specificEventListeners)} or {nameof(specificEventActions)} is null", LogType.Error);
					return false;
				}

				if (specificEventListeners.Length == 0 || specificEventListeners.Length <= eventType || specificEventActions.Length == 0 || specificEventActions.Length <= eventType)
				{
					// MDebugLog($"{nameof(IsEventValid)} : {nameof(specificEventTargetUdons)} or {nameof(specificEventActionNames)} is empty", LogType.Warning);
					return false;
				}
			}

			return true;
		}

		private UdonSharpBehaviour[] GetListeners(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
				return listeners;
			else
				return specificEventListeners[eventType];
		}

		private string[] GetActions(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
				return actions;
			else
				return specificEventActions[eventType];
		}

		public void DebugAll()
		{
			MDebugLog($"{nameof(listeners)} : {listeners.Length}");
			MDebugLog($"{nameof(actions)} : {actions.Length}");

			for (int i = 0; i < listeners.Length; i++)
				MDebugLog($"{nameof(listeners)}[{i}] : {listeners[i]}, {nameof(actions)}[{i}] : {actions[i]}");

			for (int i = 0; i < specificEventListeners.Length; i++)
			{
				MDebugLog($"{nameof(specificEventListeners)}[{i}] : {specificEventListeners[i].Length}");
				MDebugLog($"{nameof(specificEventActions)}[{i}] : {specificEventActions[i].Length}");

				for (int j = 0; j < specificEventListeners[i].Length; j++)
					MDebugLog($"{nameof(specificEventListeners)}[{i}][{j}] : {specificEventListeners[i][j]}, {nameof(specificEventActions)}[{i}][{j}] : {specificEventActions[i][j]}");
			}
		}
		#endregion
	}
}