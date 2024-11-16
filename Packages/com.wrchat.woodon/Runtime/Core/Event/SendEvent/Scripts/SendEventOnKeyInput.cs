using UnityEngine;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SendEventOnKeyInput : WEventPublisher
	{
		[Header("_" + nameof(SendEventOnKeyInput))]
		[SerializeField] private KeyCode keyCode;

		[SerializeField] private bool whenGetKeyDown = true;
		[SerializeField] private bool whenGetKeyUp;

		private void Update()
		{
			if (whenGetKeyDown && Input.GetKeyDown(keyCode))
			{
				MDebugLog($"{nameof(Update)} : {nameof(keyCode)} = {keyCode}");
				SendEvents();
			}

			if (whenGetKeyUp && Input.GetKeyUp(keyCode))
			{
				MDebugLog($"{nameof(Update)} : {nameof(keyCode)} = {keyCode}");
				SendEvents();
			}
		}
	}
}