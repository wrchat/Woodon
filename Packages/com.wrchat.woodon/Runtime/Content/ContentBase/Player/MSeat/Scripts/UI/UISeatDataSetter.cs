using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatDataSetter : WBase
	{
		[SerializeField] private MSeat mSeat;
		[SerializeField] private WInt wInt;

		public void SetSeatDataByWInt()
		{
			mSeat.IntData = wInt.Value;
			mSeat.SerializeData();
		}
	}
}