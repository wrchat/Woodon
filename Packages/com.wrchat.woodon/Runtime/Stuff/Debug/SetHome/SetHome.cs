
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class SetHome : MBase
	{
		private readonly Vector3[] homePositions = new Vector3[4];
		private readonly Quaternion[] homeRotations = new Quaternion[4];

		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKeyDown(KeyCode.F1))
					SetHomeData(0);
				if (Input.GetKeyDown(KeyCode.F2))
					SetHomeData(1);
				if (Input.GetKeyDown(KeyCode.F3))
					SetHomeData(2);
				if (Input.GetKeyDown(KeyCode.F4))
					SetHomeData(3);
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.F1))
					TPTo(0);
				if (Input.GetKeyDown(KeyCode.F2))
					TPTo(1);
				if (Input.GetKeyDown(KeyCode.F3))
					TPTo(2);
				if (Input.GetKeyDown(KeyCode.F4))
					TPTo(3);
			}
		}

		public void SetHomeData(int index)
		{
			MDebugLog($"{nameof(SetHomeData)}, Index : {index}");

			homePositions[index] = Networking.LocalPlayer.GetPosition();
			homeRotations[index] = Networking.LocalPlayer.GetRotation();
		}

		public void TPTo(int index)
		{
			MDebugLog($"{nameof(TPTo)}, Index : {index}");

			if (homePositions[index] == Vector3.zero ||
				homeRotations[index] == Quaternion.identity)
			{
				MDebugLog($"{nameof(TPTo)}, No Home Data");
			}

			Networking.LocalPlayer.TeleportTo(homePositions[index], homeRotations[index]);
		}
	}
}