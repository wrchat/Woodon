using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public class SendEventIfLocalPlayerIsMTarget : MEventSender
	{
		[SerializeField] private MTarget mTarget;

		public void Check_Global() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Check));

		public void Check()
		{
			if (mTarget.IsLocalPlayerTarget)
			{
				SendEvents();
			}
		}
	}
}