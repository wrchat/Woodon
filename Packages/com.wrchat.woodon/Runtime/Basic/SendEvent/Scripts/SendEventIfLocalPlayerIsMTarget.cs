using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public class SendEventIfLocalPlayerIsMTarget : MEventSender
	{
		[SerializeField] private MTarget mTarget;

		public void Check_Global() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Check));

		public void Check()
		{
			if (mTarget.IsTargetPlayer())
			{
				SendEvents();
			}
		}
	}
}