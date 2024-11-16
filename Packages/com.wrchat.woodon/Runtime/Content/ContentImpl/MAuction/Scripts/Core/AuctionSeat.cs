using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionSeat : MSeat
	{
		[field: Header("_" + nameof(AuctionSeat))]
		[field: UdonSynced] public int TryTime { get; private set; } = NONE_INT;
		public int RemainPoint => IntData;
		public int TryPoint => TurnData;
		
		[SerializeField] private MValue tryPoint_MValue;
		[SerializeField] private Timer timer;
		[SerializeField] private MSFXManager mSFXManager;

		public void UpdateTryPoint()
		{
			if (contentManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			AuctionManager auctionManager = (AuctionManager)contentManager;

			if (auctionManager.GetMaxTurnData() >= tryPoint_MValue.Value)
				return;

			SetTryTime(Networking.GetServerTimeInMilliseconds());
			TurnData = tryPoint_MValue.Value;
			SerializeData();
		}

		public void SetTryTime(int newTryTime)
		{
			SetOwner();
			TryTime = newTryTime;
			RequestSerialization();
		}

		protected override void OnDataChanged(DataChangeState changeState)
		{
			base.OnDataChanged(changeState);

			if (changeState != DataChangeState.None)
			{
				tryPoint_MValue.SetMinMaxValue(0, IntData, IsOwner());
			}
		}

		protected override void OnTargetChanged(DataChangeState changeState)
		{
			base.OnTargetChanged(changeState);
			
			if (changeState != DataChangeState.None)
			{
				if (IsTargetPlayer())
					tryPoint_MValue.SetValue(0);
			}
		}

		protected override void OnTurnDataChange(DataChangeState changeState)
		{
			base.OnTurnDataChange(changeState);

			if (contentManager.CurGameState != (int)AuctionState.AuctionTime)
				return;

			if (changeState != DataChangeState.Greater)
				return;

			if (IsOwner(contentManager.gameObject) == false)
				return;

			if (timer != null)
			{
				timer.StartTimer();
				mSFXManager.PlaySFX_G(1);
			}
		}
	}
}