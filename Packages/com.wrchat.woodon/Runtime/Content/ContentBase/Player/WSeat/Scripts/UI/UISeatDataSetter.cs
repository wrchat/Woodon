using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatDataSetter : WBase
	{
		[SerializeField] private WSeat wSeat;
		[SerializeField] private WInt wInt;

		public void SetSeatDataByWInt()
		{
			wSeat.IntData = wInt.Value;
			wSeat.SerializeData();
		}
	}
}