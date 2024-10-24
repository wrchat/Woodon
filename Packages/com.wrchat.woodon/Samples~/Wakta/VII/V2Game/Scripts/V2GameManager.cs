using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.VII.V2Game
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