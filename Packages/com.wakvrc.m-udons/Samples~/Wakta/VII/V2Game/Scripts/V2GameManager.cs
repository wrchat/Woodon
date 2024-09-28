
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class V2GameManager : UdonSharpBehaviour
	{
		[SerializeField] private MValue[] v2GameSeats;

		public void ResetAllSeats()
		{
			foreach (MValue v in v2GameSeats)
				v.SetValue(0);
		}
	}
}