using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	public enum VoteState
	{
		Wait,
		ShowTarget,
		VoteTime,
		WaitForResult,
		CheckResult,
		ApplyResult
	}

	[DefaultExecutionOrder(-9000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class VoteManager : ContentManager
	{
		[Header("_" + nameof(VoteManager))]
		[SerializeField] protected TextMeshProUGUI[] debugTexts;
		[SerializeField] protected Timer timer;
		[SerializeField] protected WSFXManager wSFXManager;

		public int[] MaxVoteIndexes { get; protected set; } = new int[0];

		public override void OnSeatUpdate()
		{
			WDebugLog($"{nameof(OnSeatUpdate)}");
			UpdateDebug();
		}

		public override void UpdateContent()
		{
			base.UpdateContent();
			UpdateDebug();
		}

		protected override void OnContentStateChange(DataChangeState changeState)
		{
			if (changeState == DataChangeState.Equal)
				return;

			MaxVoteIndexes = GetMaxVoteIndex();

			switch ((VoteState)ContentState)
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
					OnVoteTime();
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

			base.OnContentStateChange(changeState);
		}

		protected virtual void OnWait()
		{
			WDebugLog(nameof(OnWait));

			if (IsOwner() == false)
				return;

			foreach (WSeat seat in Seats)
			{
				seat.SeatData.SetData(nameof(VoteSeat.VoteIndex), NONE_INT); // 초기화
				seat.SerializeData();
			}
		}

		private void UpdateDebug()
		{
			foreach (TextMeshProUGUI debugText in debugTexts)
				debugText.text = GetDebugString();
		}

		public string GetDebugString()
		{
			string debugString = string.Empty;

			switch ((VoteState)ContentState)
			{
				case VoteState.Wait:
				case VoteState.ShowTarget:
					break;
				case VoteState.VoteTime:
					// 누가 투표했는지 확인
					foreach (WSeat seat in Seats)
					{
						VRCPlayerApi targetPlayer = seat.GetTargetPlayerAPI();
						string targetPlayerName = targetPlayer == null ? "-" : targetPlayer.displayName;
						debugString += $"{targetPlayerName} {(IsVoted(seat) ? "투표함" : "투표 안함")}.\n";
					}
					break;
				case VoteState.WaitForResult:
				case VoteState.CheckResult:
					string[] turnDataToString = GetSeatDataOption(nameof(VoteSeat.VoteIndex)).DataToString;
					for (int i = 0; i < turnDataToString.Length; i++)
						debugString += $"{turnDataToString[i]} 투표 수 : {GetVoteCount(i)}\n";

					if (MaxVoteIndexes.Length == 0 || (GetVoteCount(MaxVoteIndexes[0]) == 0))
					{
						debugString += $"No Winner.";
					}
					else if (MaxVoteIndexes.Length == 1)
					{
						debugString += $"{turnDataToString[MaxVoteIndexes[0]]} is Winner.";
					}
					else
					{
						debugString += $"Multiple Winners.";
					}
					break;
				case VoteState.ApplyResult:
					if (MaxVoteIndexes.Length == 0)
						debugString = $"No Winner.";
					break;
			}

			return debugString;
		}

		protected virtual void OnShowTarget()
		{
			WDebugLog(nameof(OnShowTarget));

			if (wSFXManager != null)
				wSFXManager.PlaySFX_L(0);
		}

		protected virtual void OnVoteTime()
		{
			WDebugLog(nameof(OnVoteTime));

			if (wSFXManager != null)
				wSFXManager.PlaySFX_L(1);

			if (IsOwner() == false)
				return;

			if (timer != null)
				timer.StartTimer();
		}

		protected virtual void OnWaitForResult()
		{
			WDebugLog(nameof(OnWaitForResult));

			if (wSFXManager != null)
				wSFXManager.PlaySFX_L(2);

			if (IsOwner() == false)
				return;

			if (timer != null)
				timer.ResetTimer();
		}

		protected virtual void OnCheckResult()
		{
			WDebugLog(nameof(OnCheckResult));
		}

		protected virtual void OnApplyResult()
		{
			WDebugLog(nameof(OnApplyResult));

			if (wSFXManager != null)
				wSFXManager.PlaySFX_L(5);
		}

		public void NextStateWhenTimeOver()
		{
			WDebugLog(nameof(NextStateWhenTimeOver));

			if (ContentState == (int)VoteState.VoteTime)
				SetContentState((int)VoteState.WaitForResult);
		}

		public int GetVoteCount(int voteIndex)
		{
			int defaultValue = GetSeatDataOption(nameof(VoteSeat.VoteIndex)).DefaultValue;

			int count = 0;
			foreach (WSeat voteSeat in Seats)
			{
				if (voteSeat.SeatData.GetData(nameof(VoteSeat.VoteIndex), defaultValue) == voteIndex)
					count++;
			}

			return count;
		}

		public int[] GetMaxVoteIndex()
		{
			SeatDataOption turnDataOption = GetSeatDataOption(nameof(VoteSeat.VoteIndex));
			int voteSelectionCount = turnDataOption.DataToString.Length;
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

			WUtil.Resize(ref maxIndexes, maxIndexCount);

			if (DEBUG)
				for (int i = 0; i < maxIndexes.Length; i++)
					WDebugLog($"MaxVoteIndex: {maxIndexes[i]}");

			return maxIndexes;
		}

		public int[] GetSortVoteIndex()
		{
			SeatDataOption turnDataOption = GetSeatDataOption(nameof(VoteSeat.VoteIndex));
			int voteSelectionCount = turnDataOption.DataToString.Length;
			int[] voteCounts = new int[voteSelectionCount];

			for (int i = 0; i < voteSelectionCount; i++)
				voteCounts[i] = GetVoteCount(i);

			int[] sortIndexes = new int[voteSelectionCount];
			for (int i = 0; i < voteCounts.Length; i++)
				sortIndexes[i] = i;

			// System.Array.Sort(voteCounts, sortIndexes);

			for (int i = 0; i < voteCounts.Length - 1; i++)
			{
				for (int j = 0; j < voteCounts.Length - i - 1; j++)
				{
					if (voteCounts[j] < voteCounts[j + 1])
					{
						int tempCount = voteCounts[j];
						voteCounts[j] = voteCounts[j + 1];
						voteCounts[j + 1] = tempCount;

						int tempIndex = sortIndexes[j];
						sortIndexes[j] = sortIndexes[j + 1];
						sortIndexes[j + 1] = tempIndex;
					}
				}
			}

			return sortIndexes;
		}

		public bool IsVoted(WSeat seat)
		{
			int defaultValue = GetSeatDataOption(nameof(VoteSeat.VoteIndex)).DefaultValue;
			return seat.SeatData.GetData(nameof(VoteSeat.VoteIndex), defaultValue) != defaultValue;
		}
	}
}
