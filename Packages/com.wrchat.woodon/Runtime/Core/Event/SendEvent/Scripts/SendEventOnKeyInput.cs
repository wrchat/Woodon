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
				WDebugLog($"{nameof(Update)} : {nameof(keyCode)} = {keyCode}");
				SendEvents();
			}

			if (whenGetKeyUp && Input.GetKeyUp(keyCode))
			{
				WDebugLog($"{nameof(Update)} : {nameof(keyCode)} = {keyCode}");
				SendEvents();
			}
		}
	}
}