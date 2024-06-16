
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class PositionBool : CustomBool
	{
		[Header("_" + nameof(PositionBool))]
		// HACK, TODO (x, y, z)
		[SerializeField] private float yPos;

		private void Update()
		{
			if (NotOnline)
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