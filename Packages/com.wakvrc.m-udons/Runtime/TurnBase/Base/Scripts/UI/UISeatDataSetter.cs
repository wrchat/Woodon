using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatDataSetter : MBase
	{
		[SerializeField] private MSeat mSeat;
		[SerializeField] private MValue mValue;

		public void SetSeatDataByMValue()
		{
			mSeat.SetData(mValue.Value);
		}
	}
}