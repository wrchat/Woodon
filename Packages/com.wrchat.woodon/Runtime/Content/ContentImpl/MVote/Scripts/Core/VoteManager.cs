using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class VoteManager : ContentManager
	{
		[Header("_" + nameof(VoteManager))]
		[SerializeField] protected TextMeshProUGUI debugText;
		[SerializeField] protected Timer timer;
		[SerializeField] protected MSFXManager mSFXManager;

		public int[] MaxVoteIndexes { get; protected set; } = new int[0];

		protected override void OnGameStateChange(DataChangeState changeState)
		{
			if (changeState == DataChangeState.Equal)
				return;

			MaxVoteIndexes = GetMaxVoteIndex();

			switch ((VoteState)CurGameState)
			{
				case VoteState.Wait:
					// 투표 대기
					OnWait();
					break;
				case VoteState.ShowTarget:
					// 투표 대상 공개
					OnShowTarget();
					break;
				case VoteState.VoteTime:
					// 투표 시간
					OnAuctionTime();
					break;
				case VoteState.WaitForResult:
					// 투표 결과 대기
					OnWaitForResult();
					break;
				case VoteState.CheckResult:
					// 투표 결과 확인
					OnCheckResult();
					break;
				case VoteState.ApplyResult:
					// 투표 결과 적용
					OnApplyResult();
					break;
			}

			base.OnGameStateChange(changeState);
		}

		protected virtual void OnWait()
		{
			MDebugLog(nameof(OnWait));
			
			debugText.text = $"";
		
			if (IsOwner() == false)
				return;

			foreach (VoteSeat voteSeat in MSeats)
			{
				voteSeat.TurnData = NONE_INT;
				voteSeat.SerializeData();
			}
		}

		protected virtual void OnShowTarget()
		{
			MDebugLog(nameof(OnShowTarget));

			mSFXManager.PlaySFX_L(0);
		}

		protected virtual void OnAuctionTime()
		{
			MDebugLog(nameof(OnAuctionTime));

			mSFXManager.PlaySFX_L(1);
			
			if (IsOwner() == false)
				return;
			
			if (timer != null)
				timer.StartTimer();
		}

		protected virtual void OnWaitForResult()
		{
			MDebugLog(nameof(OnWaitForResult));
	
			mSFXManager.PlaySFX_L(2);
			
			if (IsOwner() == false)
				return;

			if (timer != null)
				timer.ResetTimer();
		}

		protected virtual void OnCheckResult()
		{
			MDebugLog(nameof(OnCheckResult));

			// 투표 결과 확인 (적용 전)

			string debugS = string.Empty;

			for (int i = 0; i < TurnDataToString.Length; i++)
				debugS += $"{TurnDataToString[i]} 투표 수 : {GetVoteCount(i)}\n";

			if (MaxVoteIndexes.Length == 0 || (GetVoteCount(MaxVoteIndexes[0]) == 0))
			{
				debugText.text = debugS + $"No Winner.";
				return;
			}
			else if (MaxVoteIndexes.Length == 1)
			{
				debugText.text = debugS + $"{TurnDataToString[MaxVoteIndexes[0]]} is Winner.";
				return;
			}
			else
			{
				debugText.text = debugS + $"Multiple Winners.";
			}
		}

		protected virtual void OnApplyResult()
		{
			MDebugLog(nameof(OnApplyResult));

			mSFXManager.PlaySFX_L(5);
		
			// 투표 결과 적용
			if (MaxVoteIndexes.Length == 0)
			{
				debugText.text = $"No Winner.";

				if (IsOwner() == false)
					return;
			}
		}

		public void NextStateWhenTimeOver()
		{
			MDebugLog(nameof(NextStateWhenTimeOver));
			
			if (CurGameState == (int)VoteState.VoteTime)
				SetGameState((int)VoteState.WaitForResult);
		}

		protected int GetVoteCount(int voteIndex)
		{
			int count = 0;

			foreach (VoteSeat voteSeat in MSeats)
			{
				if (voteSeat.VoteIndex == voteIndex)
					count++;
			}

			return count;
		}

		protected int[] GetMaxVoteIndex()
		{
			int voteSelectionCount = TurnDataToString.Length;
			int[] voteCounts = new int[voteSelectionCount];

			int maxCount = 0;
			for (int i = 0; i < voteSelectionCount; i++)
			{
				voteCounts[i] = GetVoteCount(i);
				if (voteCounts[i] > maxCount)
					maxCount = voteCounts[i];
			}
			
			int[] maxIndexes = new int[voteSelectionCount];
			int maxIndexCount = 0;
			for (int i = 0; i < voteCounts.Length; i++)
			{
				if (voteCounts[i] == maxCount)
					maxIndexes[maxIndexCount++] = i;
			}

			MDataUtil.ResizeArr(ref maxIndexes, maxIndexCount);

			if (DEBUG)
				for (int i = 0; i < maxIndexes.Length; i++)
					MDebugLog($"MaxVoteIndex: {maxIndexes[i]}");

			return maxIndexes;
		}
	}
}
