using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Cinemachine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatDataSetter : MBase
	{
		[SerializeField] private MSeat mSeat;
		[SerializeField] private MScore mScore;

		public void SetSeatDataByMScore()
		{
			mSeat.SetData(mScore.Score);
		}
	}
}