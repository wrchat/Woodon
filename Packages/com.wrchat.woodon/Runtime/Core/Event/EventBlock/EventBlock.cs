using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class EventBlock : MBase
	{
		[Header("_" + nameof(EventBlock))]
		
		[Tooltip("이벤트를 발생시킬 Udon들입니다.")]
		[SerializeField] private UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];
		
		[Tooltip("targetUdons와 같은 인덱스에 있는 Udon에게 호출할 메소드 이름입니다.")]
		[SerializeField] private string[] methodNames = new string[0];

		[Header("_" + nameof(EventBlock) + " - Options")]

		[Tooltip("만약 비어있지 않다면, OwnerObject의 오너일 경우에만 이벤트를 발생시킵니다.")]
		[SerializeField] private UdonSharpBehaviour ownerObject = null;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (targetUdons.Length != methodNames.Length)
			{
				MDebugLog($"{nameof(targetUdons)}.Length != {nameof(methodNames)}.Length", LogType.Error);
				return;
			}

			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (targetUdons[i] == null)
				{
					MDebugLog($"{nameof(targetUdons)}[{i}] == null", LogType.Error);
					return;
				}

				if (string.IsNullOrEmpty(methodNames[i]))
				{
					MDebugLog($"{nameof(methodNames)}[{i}] == null", LogType.Warning);
					return;
				}
			}
		}

		[ContextMenu(nameof(Invoke))]
		public void Invoke()
		{
			MDebugLog($"{nameof(Invoke)}");

			if ((ownerObject != null) && (IsOwner(ownerObject.gameObject) == false))
				return;

			for (int i = 0; i < targetUdons.Length; i++)
				targetUdons[i].SendCustomEvent(methodNames[i]);
		}

		[ContextMenu(nameof(Invoke_G))]
		public void Invoke_G()
		{
			MDebugLog($"{nameof(Invoke_G)}");

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Invoke));
		}
	}
}