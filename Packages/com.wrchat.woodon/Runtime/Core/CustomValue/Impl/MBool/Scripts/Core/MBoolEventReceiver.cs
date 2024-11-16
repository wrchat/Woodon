using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MBoolEventReceiver : MBoolFollower
	{
		[Header("_" + nameof(MBoolEventReceiver))]
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

			if (mBool == null)
				return;

			for (int i = 0; i < trueEventUdons.Length; i++)
				mBool.RegisterListener(trueEventUdons[i], trueEventMethodNames[i], MBoolEvent.OnTrue);

			for (int i = 0; i < falseEventUdons.Length; i++)
				mBool.RegisterListener(falseEventUdons[i], falseEventMethodNames[i], MBoolEvent.OnFalse);
		}

		public override void SetMBool(MBool mBool)
		{
			MDebugLog($"{nameof(SetMBool)} : {mBool}");

			if (this.mBool != null)
			{
				for (int i = 0; i < trueEventUdons.Length; i++)
					this.mBool.UnregisterListener(trueEventUdons[i], trueEventMethodNames[i], MBoolEvent.OnTrue);

				for (int i = 0; i < falseEventUdons.Length; i++)
					this.mBool.UnregisterListener(falseEventUdons[i], falseEventMethodNames[i], MBoolEvent.OnFalse);
			}

			this.mBool = mBool;
			Init();
		}
	}
}