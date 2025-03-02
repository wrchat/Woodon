using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WBoolEventReceiver : WBoolFollower
	{
		[Header("_" + nameof(WBoolEventReceiver))]
		[SerializeField] private UdonSharpBehaviour[] trueEventUdons = new UdonSharpBehaviour[0];
		[SerializeField] private string[] trueEventMethodNames = new string[0];

		[SerializeField] private UdonSharpBehaviour[] falseEventUdons = new UdonSharpBehaviour[0];
		[SerializeField] private string[] falseEventMethodNames = new string[0];

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog($"{nameof(Init)}");

			if (wBool == null)
				return;

			for (int i = 0; i < trueEventUdons.Length; i++)
				wBool.RegisterListener(trueEventUdons[i], trueEventMethodNames[i], WBoolEvent.OnTrue);

			for (int i = 0; i < falseEventUdons.Length; i++)
				wBool.RegisterListener(falseEventUdons[i], falseEventMethodNames[i], WBoolEvent.OnFalse);
		}

		public override void SetWBool(WBool wBool)
		{
			MDebugLog($"{nameof(SetWBool)} : {wBool}");

			if (this.wBool != null)
			{
				for (int i = 0; i < trueEventUdons.Length; i++)
					this.wBool.UnregisterListener(trueEventUdons[i], trueEventMethodNames[i], WBoolEvent.OnTrue);

				for (int i = 0; i < falseEventUdons.Length; i++)
					this.wBool.UnregisterListener(falseEventUdons[i], falseEventMethodNames[i], WBoolEvent.OnFalse);
			}

			this.wBool = wBool;
			Init();
		}
	}
}