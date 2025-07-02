
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class SetHome : WBase
	{
		private readonly Vector3[] homePositions = new Vector3[4];
		private readonly Quaternion[] homeRotations = new Quaternion[4];
		[SerializeField] private KeyCode homeKey0 = KeyCode.F1;
		[SerializeField] private KeyCode homeKey1 = KeyCode.F2;
		[SerializeField] private KeyCode homeKey2 = KeyCode.F3;
		[SerializeField] private KeyCode homeKey3 = KeyCode.F4;

		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKeyDown(homeKey0))
					SetHomeData(0);
				if (Input.GetKeyDown(homeKey1))
					SetHomeData(1);
				if (Input.GetKeyDown(homeKey2))
					SetHomeData(2);
				if (Input.GetKeyDown(homeKey3))
					SetHomeData(3);
			}
			else
			{
				if (Input.GetKeyDown(homeKey0))
					TPTo(0);
				if (Input.GetKeyDown(homeKey1))
					TPTo(1);
				if (Input.GetKeyDown(homeKey2))
					TPTo(2);
				if (Input.GetKeyDown(homeKey3))
					TPTo(3);
			}
		}

		public void SetHomeData(int index)
		{
			WDebugLog($"{nameof(SetHomeData)}, Index : {index}");

			homePositions[index] = Networking.LocalPlayer.GetPosition();
			homeRotations[index] = Networking.LocalPlayer.GetRotation();
		}

		public void TPTo(int index)
		{
			WDebugLog($"{nameof(TPTo)}, Index : {index}");

			if (homePositions[index] == Vector3.zero ||
				homeRotations[index] == Quaternion.identity)
			{
				WDebugLog($"{nameof(TPTo)}, No Home Data");
			}

			Networking.LocalPlayer.TeleportTo(homePositions[index], homeRotations[index]);
		}
	}
}