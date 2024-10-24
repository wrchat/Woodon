using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DistanceBool : MBool
	{
		[Header("_" + nameof(DistanceBool))]
		[SerializeField] private float distance = 10f;

		private void Update()
		{
			if (Vector3.Distance(Networking.LocalPlayer.GetPosition(), transform.position) > distance)
			{
				SetValue(false);
			}
			else
			{
				SetValue(true);
			}
		}
	}
}