using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class EventBlock : MBase
	{
		[Header("_" + nameof(EventBlock))]
		[SerializeField] private UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];
		[SerializeField] private string[] methodNames = new string[0];

		public void Invoke()
		{
			MDebugLog($"{nameof(Invoke)}");

			for (int i = 0; i < targetUdons.Length; i++)
				targetUdons[i].SendCustomEvent(methodNames[i]);
		}

		public void Invoke_G()
		{
			MDebugLog($"{nameof(Invoke_G)}");
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Invoke));
		}
	}
}