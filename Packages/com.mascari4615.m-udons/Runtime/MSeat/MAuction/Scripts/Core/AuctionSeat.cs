using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionSeat : MTurnSeat
	{
		public int RemainPoint => Data;
		public int TryPoint => TurnData;

		[field: Header("_" + nameof(AuctionSeat))]
		[SerializeField] private MScore tryPointMScore;
		[SerializeField] private TimeEvent timeEvent;
		[SerializeField] private MSFXManager mSFXManager;

		public void UpdateTryPoint()
		{
			if (seatManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			AuctionManager auctionManager = (AuctionManager)seatManager;

			if (auctionManager.GetMaxTurnData() >= tryPointMScore.Score)
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

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			base.OnTurnDataChange(changeState);

			if (seatManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			if (changeState != DataChangeState.Greater)
				return;

			if (!IsOwner(seatManager.gameObject))
				return;

			if (timeEvent != null)
			{
				timeEvent.SetTime();
				mSFXManager.PlaySFX_G(1);
			}
		}
	}
}