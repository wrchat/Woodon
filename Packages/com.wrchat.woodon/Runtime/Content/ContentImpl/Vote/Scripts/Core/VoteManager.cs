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
		[SerializeField] protected WSFXManager wSFXManager;
		[SerializeField] protected TextMeshProUGUI[] resultTexts;
		[SerializeField] protected TextMeshProUGUI[] elseResultTexts;

		public int[] MaxVoteIndexes { get; protected set; } = new int[0];

		protected override void OnContentStateChange(DataChangeState changeState)
		{
			if (changeState == DataChangeState.Equal)
				return;

			MaxVoteIndexes = GetMaxVoteIndex();

			// 1등 투표 결과
			{
				string resultString = string.Empty;
				SeatDataOption temp = GetSeatDataOption("TurnData");
				for (int i = 0; i < MaxVoteIndexes.Length; i++)
				{
					int index = MaxVoteIndexes[i];

					if (i == 0)
						resultString += $"{temp.DataToString[index]}";
					else
						resultString += $",\n{temp.DataToString[index]}";
				}

				for (int j = 0; j < resultTexts.Length; j++)
				{
					resultTexts[j].text = resultString;
				}
			}

			// 나머지 투표 결과
			{
				string resultString = string.Empty;
				SeatDataOption temp = GetSeatDataOption("TurnData");
				int[] sortIndexes = GetSortVoteIndex();
				for (int i = 0; i < sortIndexes.Length; i++)
				{
					int index = sortIndexes[i];
					resultString += $"{temp.DataToString[index]} \t: {GetVoteCount(index)}표\n";
				}

				for (int j = 0; j < elseResultTexts.Length; j++)
				{
					elseResultTexts[j].text = resultString;
				}
			}

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

			debugText.text = $"";

			if (IsOwner() == false)
				return;

			foreach (VoteSeat seat in Seats)
			{
				seat.TurnData = NONE_INT;
				seat.SerializeData();
			}
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

			// 투표 결과 확인 (적용 전)

			string debugS = string.Empty;

			SeatDataOption turnDataOption = GetSeatDataOption(TurnDataString);
			for (int i = 0; i < turnDataOption.DataToString.Length; i++)
				debugS += $"{turnDataOption.DataToString[i]} 투표 수 : {GetVoteCount(i)}\n";

			if (MaxVoteIndexes.Length == 0 || (GetVoteCount(MaxVoteIndexes[0]) == 0))
			{
				debugText.text = debugS + $"No Winner.";
				return;
			}
			else if (MaxVoteIndexes.Length == 1)
			{
				debugText.text = debugS + $"{turnDataOption.DataToString[MaxVoteIndexes[0]]} is Winner.";
				return;
			}
			else
			{
				debugText.text = debugS + $"Multiple Winners.";
			}
		}

		protected virtual void OnApplyResult()
		{
			WDebugLog(nameof(OnApplyResult));

			if (wSFXManager != null)
				wSFXManager.PlaySFX_L(5);

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
			WDebugLog(nameof(NextStateWhenTimeOver));

			if (ContentState == (int)VoteState.VoteTime)
				SetContentState((int)VoteState.WaitForResult);
		}

		protected int GetVoteCount(int voteIndex)
		{
			int count = 0;

			foreach (VoteSeat voteSeat in Seats)
			{
				if (voteSeat.VoteIndex == voteIndex)
					count++;
			}

			return count;
		}

		protected int[] GetMaxVoteIndex()
		{
			SeatDataOption turnDataOption = GetSeatDataOption(TurnDataString);
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

		protected int[] GetSortVoteIndex()
		{
			SeatDataOption turnDataOption = GetSeatDataOption(TurnDataString);
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
	}
}
