using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public abstract class MEventSender : MBase
	{
		// TODO: 특정 클래스의 특정 사건을 뜻하는 Event와, 우동의 함수를 뜻하는 Event간의 표현이 모호함
		public const int DEFAULT_EVENT = -1;

		[Header("_" + nameof(MEventSender))]
		// 기본 이벤트
		[SerializeField] protected UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];

		// TODO: actionNames로 변경
		[SerializeField] protected string[] eventNames = new string[0];

		[Header("_" + nameof(MEventSender) + " - Options")]
		[SerializeField] protected bool sendEventGlobal;

		// 추가 정의 이벤트
		protected UdonSharpBehaviour[][] specificEventTargetUdons = new UdonSharpBehaviour[0][];
		protected string[][] specificEventActionNames = new string[0][];

		// ---- ---- ---- ----

		protected void SendEvents(int eventType = DEFAULT_EVENT)
		{
			MDebugLog($"{nameof(SendEvents)}");

			if (IsEventValid(eventType) == false)
				return;

			UdonSharpBehaviour[] targetUdons = GetTargetUdons(eventType);
			string[] eventNames = GetEventNames(eventType);

			for (int i = 0; i < targetUdons.Length; i++)
			{
				// MDebugLog($"{nameof(SendEvents)} : {nameof(targetUdons)}[{i}] = {targetUdons[i]}, {nameof(eventNames)}[{i}] = {eventNames[i]}");

				if (targetUdons[i] == null)
				{
					MDebugLog($"{nameof(SendEvents)} : {nameof(targetUdons)}[{i}] is null, Skip {nameof(eventNames)}[{i}] = {eventNames[i]}", LogType.Error);
					continue;
				}

				if (sendEventGlobal)
					targetUdons[i].SendCustomNetworkEvent(NetworkEventTarget.All, eventNames[i]);
				else
					targetUdons[i].SendCustomEvent(eventNames[i]);
			}

			if (eventType != DEFAULT_EVENT)
			{
				// 추가 정의된 이벤트가 일어났다면, 기본 이벤트도 한 번 호출해줍니다.
				SendEvents();
			}
		}

		/// <summary>
		/// 호출하는 함수의 접근제한자는 public 이여야 함.
		/// </summary>
		/// <param name="targetUdon"></param>
		/// <param name="actionName"></param>
		public void RegisterListener(UdonSharpBehaviour targetUdon, string actionName, int eventType = DEFAULT_EVENT)
		{
			MDebugLog($"{nameof(RegisterListener)}({targetUdon}, {actionName}, {eventType})");

			if (eventType != DEFAULT_EVENT)
			{
				if (specificEventTargetUdons.Length <= eventType)
				{
					MDataUtil.ResizeArr(ref specificEventTargetUdons, eventType + 1);

					for (int i = 0; i < specificEventTargetUdons.Length; i++)
					{
						if (specificEventTargetUdons[i] == null)
							specificEventTargetUdons[i] = new UdonSharpBehaviour[0];
					}
				}

				if (specificEventActionNames.Length <= eventType)
				{
					MDataUtil.ResizeArr(ref specificEventActionNames, eventType + 1);
					specificEventActionNames[eventType] = new string[0];

					for (int i = 0; i < specificEventActionNames.Length; i++)
					{
						if (specificEventActionNames[i] == null)
							specificEventActionNames[i] = new string[0];
					}
				}
			}

			UdonSharpBehaviour[] _targetUdons = GetTargetUdons(eventType);
			string[] _eventNames = GetEventNames(eventType);

			// 구독자 우동과 이벤트를 추가할 공간
			MDataUtil.ResizeArr(ref _targetUdons, _targetUdons.Length + 1);
			MDataUtil.ResizeArr(ref _eventNames, _eventNames.Length + 1);

			// 새로운 우동과 이벤트를 추가
			_targetUdons[_targetUdons.Length - 1] = targetUdon;
			_eventNames[_eventNames.Length - 1] = actionName;

			// TODO: 중복 등록 처리

			// 원본 배열에 수정된 배열을 대입
			if (eventType != DEFAULT_EVENT)
			{
				specificEventTargetUdons[eventType] = _targetUdons;
				specificEventActionNames[eventType] = _eventNames;
			}
			else
			{
				targetUdons = _targetUdons;
				eventNames = _eventNames;
			}
		}

		public void RemoveListener(UdonSharpBehaviour targetUdon, string actionName, int eventType = DEFAULT_EVENT)
		{
			MDebugLog($"{nameof(RemoveListener)}({targetUdon}, {actionName}, {eventType})");

			if (eventType != DEFAULT_EVENT)
			{
				if (specificEventTargetUdons.Length <= eventType)
					return;

				if (specificEventActionNames.Length <= eventType)
					return;
			}

			UdonSharpBehaviour[] targetUdons = GetTargetUdons(eventType);
			string[] eventNames = GetEventNames(eventType);

			int targetIndex = NONE_INT;
			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (targetUdons[i] == targetUdon && eventNames[i] == actionName)
				{
					targetIndex = i;
					break;
				}
			}

			if (targetIndex == NONE_INT)
			{
				MDebugLog($"{nameof(RemoveListener)} : {nameof(targetUdon)} or {nameof(actionName)} is not found", LogType.Warning);
				return;
			}

			MDataUtil.RemoveAt(ref targetUdons, targetIndex);
			MDataUtil.RemoveAt(ref eventNames, targetIndex);
		}

		#region
		private bool IsEventValid(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
			{
				if (targetUdons == null || eventNames == null)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(targetUdons)} or {nameof(eventNames)} is null", LogType.Error);
					return false;
				}

				if (targetUdons.Length == 0 || eventNames.Length == 0)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(targetUdons)} or {nameof(eventNames)} is empty", LogType.Warning);
					return false;
				}
			}
			else
			{
				if (specificEventTargetUdons == null || specificEventActionNames == null)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(specificEventTargetUdons)} or {nameof(specificEventActionNames)} is null", LogType.Error);
					return false;
				}

				if (specificEventTargetUdons.Length == 0 || specificEventTargetUdons.Length <= eventType || specificEventActionNames.Length == 0 || specificEventActionNames.Length <= eventType)
				{
					MDebugLog($"{nameof(IsEventValid)} : {nameof(specificEventTargetUdons)} or {nameof(specificEventActionNames)} is empty", LogType.Warning);
					return false;
				}
			}

			return true;
		}

		private UdonSharpBehaviour[] GetTargetUdons(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
				return targetUdons;
			else
				return specificEventTargetUdons[eventType];
		}

		private string[] GetEventNames(int eventType = DEFAULT_EVENT)
		{
			if (eventType == DEFAULT_EVENT)
				return eventNames;
			else
				return specificEventActionNames[eventType];
		}
		#endregion
	}
}