using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionSeat : MTurnSeat
	{
		public int Point => Data;
		public int TryPoint => TurnData;

		[field: Header("_" + nameof(AuctionSeat))]
		[SerializeField] private MScore tryPointMScore;
		
		public void UpdateTryPoint()
		{
			if (seatManager.CurGameState != (int)AuctionState.AuctionTime)
				return;
			
			SetTurnData(tryPointMScore.Score);
		}

		protected override void OnDataChange()
		{
			base.OnDataChange();
			tryPointMScore.SetMinMaxScore(0, Data);
		}

		protected override void OnOwnerChange()
		{
			base.OnOwnerChange();

			if (IsLocalPlayerID(OwnerID))
				tryPointMScore.SetScore(0);
		}
	}
}