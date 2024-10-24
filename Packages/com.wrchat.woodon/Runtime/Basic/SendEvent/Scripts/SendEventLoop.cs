
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class SendEventLoop : MEventSender
	{
		[Header("_" + nameof(SendEventLoop))]
		[SerializeField] private float minDelay = .5f;
		[SerializeField] private float maxDelay = .5f;

		private void Start()
		{
			SendCustomEventDelayedSeconds(nameof(Loop), Random.Range(minDelay, maxDelay));
		}

		public void Loop()
		{
			SendCustomEventDelayedSeconds(nameof(Loop), Random.Range(minDelay, maxDelay));
			SendEvents();
		}
	}
}