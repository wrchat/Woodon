using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class PositionBool : MBool
	{
		[Header("_" + nameof(PositionBool))]
		// HACK, TODO (x, y, z)
		[SerializeField] private float yPos;

		private void Update()
		{
			if (IsNotOnline())
				return;

			if (Networking.LocalPlayer.GetPosition().y < yPos)
			{
				if (Value == true)
					SetValue(false);
			}
			else
			{
				if (Value == false)
					SetValue(true);
			}
		}
	}
}