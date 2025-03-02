using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WBoolEventReceiver : WBoolFollower
	{
		[Header("_" + nameof(WBoolEventReceiver))]
		[SerializeField] private UdonSharpBehaviour[] trueEventListeners = new UdonSharpBehaviour[0];
		[SerializeField] private string[] trueEventCallbacks = new string[0];

		[SerializeField] private UdonSharpBehaviour[] falseEventListeners = new UdonSharpBehaviour[0];
		[SerializeField] private string[] falseEventCallbacks = new string[0];

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			WDebugLog($"{nameof(Init)}");

			if (wBool == null)
				return;

			wBool.RegisterListener(this, nameof(RunTrueEvents), WBoolEvent.OnTrue);
			wBool.RegisterListener(this, nameof(RunFalseEvents), WBoolEvent.OnFalse);

			// wBool의 초기 값으로 한 번 실행합니다. 초기화 단계에서 wBool의 값이 바뀔 경우, 이벤트가 중복으로 발생할 수 있습니다.
			if (wBool.Value)
			{
				RunTrueEvents();
			}
			else
			{
				RunFalseEvents();
			}
		}

		public void RunTrueEvents()
		{
			WDebugLog($"{nameof(RunTrueEvents)}");

			for (int i = 0; i < trueEventListeners.Length; i++)
				trueEventListeners[i].SendCustomEvent(trueEventCallbacks[i]);
		}

		public void RunFalseEvents()
		{
			WDebugLog($"{nameof(RunFalseEvents)}");

			for (int i = 0; i < falseEventListeners.Length; i++)
				falseEventListeners[i].SendCustomEvent(falseEventCallbacks[i]);
		}

		public override void SetWBool(WBool wBool)
		{
			WDebugLog($"{nameof(SetWBool)} : {wBool}");

			if (this.wBool != null)
			{
				this.wBool.UnregisterListener(this, nameof(RunTrueEvents), WBoolEvent.OnTrue);
				this.wBool.UnregisterListener(this, nameof(RunFalseEvents), WBoolEvent.OnFalse);
			}

			this.wBool = wBool;
			Init();
		}
	}
}