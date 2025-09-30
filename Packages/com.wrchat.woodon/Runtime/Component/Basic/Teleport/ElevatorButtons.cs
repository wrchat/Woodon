using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ElevatorButtons : WBase
	{
		[Header("_" + nameof(ElevatorButtons))]
		[SerializeField] private TeleportManager teleportManager;

		public void Teleport(int index)
		{
			if (teleportManager == null)
			{
				WDebugLog($"{nameof(teleportManager)} is not assigned!", LogType.Error);
				return;
			}

			teleportManager.Teleport(index);
		}

		#region Horrible UdonEvents
		public void Teleport0() => Teleport(0);
		public void Teleport1() => Teleport(1);
		public void Teleport2() => Teleport(2);
		public void Teleport3() => Teleport(3);
		public void Teleport4() => Teleport(4);
		public void Teleport5() => Teleport(5);
		public void Teleport6() => Teleport(6);
		public void Teleport7() => Teleport(7);
		public void Teleport8() => Teleport(8);
		public void Teleport9() => Teleport(9);
		public void Teleport10() => Teleport(10);
		public void Teleport11() => Teleport(11);
		public void Teleport12() => Teleport(12);
		public void Teleport13() => Teleport(13);
		public void Teleport14() => Teleport(14);
		public void Teleport15() => Teleport(15);
		public void Teleport16() => Teleport(16);
		public void Teleport17() => Teleport(17);
		public void Teleport18() => Teleport(18);
		public void Teleport19() => Teleport(19);
		public void Teleport20() => Teleport(20);
		#endregion
	}
}