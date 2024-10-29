using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	public class SendEventOnAnimationEvent : MBase
	{
		[Header("_" + nameof(SendEventOnAnimationEvent))]
		[SerializeField] private UdonSharpBehaviour[] targetUdons = new UdonSharpBehaviour[0];
		[SerializeField] private string[] eventNames = new string[0];

		// TODO: Editor로 보여주기
		private void Start()
		{
			MDebugLog($"{nameof(Start)}");

			if (targetUdons.Length != eventNames.Length)
			{
				MDebugLog($"{nameof(Start)} : {nameof(targetUdons)}.Length != {nameof(eventNames)}.Length", LogType.Error);
				return;
			}

			for (int i = 0; i < targetUdons.Length; i++)
			{
				if (targetUdons[i] == null)
				{
					MDebugLog($"{nameof(Start)} : {nameof(targetUdons)}[{i}] is null, Skip {nameof(eventNames)}[{i}] = {eventNames[i]}", LogType.Error);
					continue;
				}

				if (string.IsNullOrEmpty(eventNames[i]))
				{
					MDebugLog($"{nameof(Start)} : {nameof(eventNames)}[{i}] is null or empty, Skip {nameof(targetUdons)}[{i}]", LogType.Warning);
					continue;
				}
			}
		}

		public void SendEvent(int index)
		{
			MDebugLog($"{nameof(SendEvent)} : {index}");

			if (index < 0 || index >= targetUdons.Length)
			{
				MDebugLog($"{nameof(SendEvent)} : Invalid index {index}", LogType.Error);
				return;
			}

			targetUdons[index].SendCustomEvent(eventNames[index]);
		}

		#region HorribleEvents
		public void SendEvent0() => SendEvent(0);
		public void SendEvent1() => SendEvent(1);
		public void SendEvent2() => SendEvent(2);
		public void SendEvent3() => SendEvent(3);
		public void SendEvent4() => SendEvent(4);
		public void SendEvent5() => SendEvent(5);
		public void SendEvent6() => SendEvent(6);
		public void SendEvent7() => SendEvent(7);
		public void SendEvent8() => SendEvent(8);
		public void SendEvent9() => SendEvent(9);
		public void SendEvent10() => SendEvent(10);
		public void SendEvent11() => SendEvent(11);
		public void SendEvent12() => SendEvent(12);
		public void SendEvent13() => SendEvent(13);
		public void SendEvent14() => SendEvent(14);
		public void SendEvent15() => SendEvent(15);
		public void SendEvent16() => SendEvent(16);
		#endregion
	}
}