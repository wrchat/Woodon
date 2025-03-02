using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-10000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionManager : ContentManager
	{
		[Header("_" + nameof(AuctionManager))]
		[SerializeField] private TextMeshProUGUI debugText;
		[SerializeField] private Timer timer;
		[SerializeField] private TextMeshProUGUI[] maxTryPointTexts;
		[SerializeField] private MSFXManager mSFXManager;

		public int WinnerIndex { get; private set; } = NONE_INT;
		public AuctionSeat MaxTryPointSeat { get; private set; } = null;

		protected override void OnContentStateChange(DataChangeState changeState)
		{
			if (changeState == DataChangeState.Equal)
				return;

			MaxTryPointSeat = GetMaxTryPointSeat();

			switch ((AuctionState)ContentState)
			{
				case AuctionState.Wait:
					// 경매 대기
					WinnerIndex = NONE_INT;
					OnWait();
					break;
				case AuctionState.ShowTarget:
					// 경매 대상 공개
					OnShowTarget();
					break;
				case AuctionState.AuctionTime:
					// 경매 시간
					OnAuctionTime();
					break;
				case AuctionState.WaitForResult:
					// 경매 결과 대기
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

			base.OnContentStateChange(changeState);
		}

		protected virtual void OnWait()
		{
			WDebugLog(nameof(OnWait));

			if (IsOwner() == false)
				return;

			foreach (AuctionSeat seat in Seats)
			{
				seat.SetTryTime(NONE_INT);
				seat.TurnData = 0;
				seat.SerializeData();
			}
		}

		protected virtual void OnShowTarget()
		{
			WDebugLog(nameof(OnShowTarget));

			mSFXManager.PlaySFX_L(0);

			if (IsOwner() == false)
				return;
		}

		protected virtual void OnAuctionTime()
		{
			WDebugLog(nameof(OnAuctionTime));

			mSFXManager.PlaySFX_L(1);

			if (IsOwner() == false)
				return;

			if (timer != null)
				timer.StartTimer();
		}

		protected virtual void OnWaitForResult()
		{
			WDebugLog(nameof(OnWaitForResult));

			mSFXManager.PlaySFX_L(2);

			if (IsOwner() == false)
				return;

			if (timer != null)
				timer.ResetTimer();
		}

		protected virtual void OnCheckResult()
		{
			WDebugLog(nameof(OnCheckResult));

			// 경매 결과 확인 (적용 전)

			if (MaxTryPointSeat != null)
			{
				mSFXManager.PlaySFX_L(3);
			}
			else
			{
				mSFXManager.PlaySFX_L(4);
				debugText.text = $"No Winner.";
				return;
			}

			WinnerIndex = MaxTryPointSeat.Index;
			debugText.text = $"{MaxTryPointSeat.TargetPlayerID} is Winner. ({MaxTryPointSeat.TryPoint})";

			if (IsOwner() == false)
				return;
		}

		protected virtual void OnApplyResult()
		{
			WDebugLog(nameof(OnApplyResult));

			mSFXManager.PlaySFX_L(5);

			if (IsOwner() == false)
				return;

			// 경매 결과 적용
			if (MaxTryPointSeat == null)
			{
				debugText.text = $"No Winner.";
				return;
			}

			MaxTryPointSeat.IntData = MaxTryPointSeat.RemainPoint - MaxTryPointSeat.TryPoint;
			MaxTryPointSeat.SerializeData();

			debugText.text = $"{MaxTryPointSeat.TargetPlayerID} Gets @ by {MaxTryPointSeat.TryPoint} Point";

			// foreach (AuctionSeat auctionSeat in Seats)
			// {
			// 	auctionSeat.SetTryTime(NONE_INT);
			// 	auctionSeat.SetTurnData(0);
			// }
		}

		public override void UpdateContent()
		{
			WDebugLog(nameof(UpdateContent));
			base.UpdateContent();

			int maxPoint = ContentUtil.GetMaxData(this, TurnDataString);
			foreach (TextMeshProUGUI maxTryPointText in maxTryPointTexts)
				maxTryPointText.text = maxPoint.ToString();
		}

		public void NextStateWhenTimeOver()
		{
			WDebugLog(nameof(NextStateWhenTimeOver));

			if (ContentState == (int)AuctionState.AuctionTime)
				SetContentState((int)AuctionState.WaitForResult);
		}

		private AuctionSeat GetMaxTryPointSeat()
		{
			MSeat[] maxTryPointSeats = ContentUtil.GetMaxDataSeats(this, TurnDataString);

			if (maxTryPointSeats.Length == 0)
			{
				debugText.text = $"No Winner.";
				return null;
			}

			int maxTryPoint = maxTryPointSeats[0].TurnData;

			if (maxTryPoint <= 0)
			{
				debugText.text = $"No Winner.";
				return null;
			}

			if (maxTryPointSeats.Length >= 2)
			{
				int fastestTryTime = int.MaxValue;
				AuctionSeat fastestTryTimeSeat = null;

				foreach (AuctionSeat auctionSeat in maxTryPointSeats)
				{
					if (auctionSeat.TryTime < fastestTryTime)
					{
						fastestTryTime = auctionSeat.TryTime;
						fastestTryTimeSeat = auctionSeat;
					}
				}

				return fastestTryTimeSeat;
			}

			return (AuctionSeat)maxTryPointSeats[0];
		}
	}
}