using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionSeat : MTurnSeat
	{
		[field: Header("_" + nameof(AuctionSeat))]
		[field: UdonSynced] public int TryTime { get; private set; } = NONE_INT;
		public int RemainPoint => Data;
		public int TryPoint => TurnData;
		
		[SerializeField] private MValue tryPoint_MValue;
		[SerializeField] private Timer timer;
		[SerializeField] private MSFXManager mSFXManager;

		public void UpdateTryPoint()
		{
			if (turnBaseManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			AuctionManager auctionManager = (AuctionManager)turnBaseManager;

			if (auctionManager.GetMaxTurnData() >= tryPoint_MValue.Value)
				return;

			SetTryTime(Networking.GetServerTimeInMilliseconds());
			SetTurnData(tryPoint_MValue.Value);
		}

		public void SetTryTime(int newTryTime)
		{
			SetOwner();
			TryTime = newTryTime;
			RequestSerialization();
		}

		protected override void OnDataChange(DataChangeState changeState)
		{
			base.OnDataChange(changeState);

			if (changeState != DataChangeState.None)
			{
				tryPoint_MValue.SetMinMaxValue(0, Data, IsOwner());
			}
		}

		protected override void OnTargetChange(DataChangeState changeState)
		{
			base.OnTargetChange(changeState);
			
			if (changeState != DataChangeState.None)
			{
				if (IsTargetPlayer())
					tryPoint_MValue.SetValue(0);
			}
		}

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			base.OnTurnDataChange(changeState);

			if (turnBaseManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			if (changeState != DataChangeState.Greater)
				return;

			if (IsOwner(turnBaseManager.gameObject) == false)
				return;

			if (timer != null)
			{
				timer.StartTimer();
				mSFXManager.PlaySFX_G(1);
			}
		}
	}
}