using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	public enum AuctionState
	{
		Wait,
		ShowTarget,
		AuctionTime,
		WaitForResult,
		CheckResult,
		ApplyResult
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionManager : MTurnSeatManager
	{
		[Header("_" + nameof(AuctionManager))]
		[SerializeField] private TextMeshProUGUI debugText;
		public int WinnerIndex { get; private set; } = NONE_INT;

		protected override void OnGameStateChange(int origin, int value)
		{
			AuctionState originState = (AuctionState)origin;
			AuctionState newState = (AuctionState)value;

			if (originState == newState)
				return;

			switch (newState)
			{
				case AuctionState.Wait:
					// 경매 대기
					WinnerIndex = NONE_INT;
					debugText.text = "Wait";
					OnWait();
					break;
				case AuctionState.ShowTarget:
					// 경매 대상 공개
					debugText.text = "ShowTarget";
					OnShowTarget();
					break;
				case AuctionState.AuctionTime:
					// 경매 시간
					debugText.text = "AuctionTime";
					OnAuctionTime();
					break;
				case AuctionState.WaitForResult:
					// 경매 결과 대기
					debugText.text = "WaitForResult";
					OnWaitForResult();
					break;
				case AuctionState.CheckResult:
					// 경매 결과 확인
					OnCheckResult();
					break;
				case AuctionState.ApplyResult:
					// 경매 결과 적용
					OnApplyResult();
					break;
			}

			base.OnGameStateChange(origin, value);
		}

		protected virtual void OnWait()
		{

		}

		protected virtual void OnShowTarget()
		{

		}

		protected virtual void OnAuctionTime()
		{

		}

		protected virtual void OnWaitForResult()
		{

		}

		protected virtual void OnCheckResult()
		{
			// 경매 결과 확인
			int maxTryPoint = GetMaxTurnData();

			foreach (AuctionSeat auctionSeat in TurnSeats)
			{
				if (auctionSeat.TryPoint == maxTryPoint)
				{
					// 최고 입찰자
					WinnerIndex = auctionSeat.Index;
					debugText.text = $"{auctionSeat.OwnerID} is Winner. ({auctionSeat.TryPoint})";
					break;
				}
			}
		}

		protected virtual void OnApplyResult()
		{
			// 경매 결과 적용
			int maxTryPoint = GetMaxTurnData();

			foreach (AuctionSeat auctionSeat in TurnSeats)
			{
				if (auctionSeat.TryPoint == maxTryPoint)
					auctionSeat.SetData(auctionSeat.Point - auctionSeat.TryPoint);

				auctionSeat.SetTurnData(0);
			}
		}
	}
}