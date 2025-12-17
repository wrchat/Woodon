using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIVote : WBase
	{
		[SerializeField] protected VoteManager voteManager;
		[SerializeField] protected TextMeshProUGUI[] debugTexts;
		[SerializeField] protected TextMeshProUGUI[] resultTexts;
		[SerializeField] protected TextMeshProUGUI[] elseResultTexts;
		// 0표는 제외하고 출력
		[SerializeField] protected bool excludeZeroVote = true;

		// 결과에 줄바꿈할지
		[SerializeField] protected bool useNewLineInResult = false;

		private void Start()
		{
			voteManager.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			// 1. 1등 투표 결과
			string resultString = string.Empty;
			{
				// 1-1. 문자열 생성
				SeatDataOption temp = voteManager.GetSeatDataOption(nameof(VoteSeat.VoteIndex));
				for (int i = 0; i < voteManager.MaxVoteIndexes.Length; i++)
				{
					int index = voteManager.MaxVoteIndexes[i];

					if (i == 0)
						resultString += $"{temp.DataToString[index]}";
					else
						resultString += useNewLineInResult ? $",\n{temp.DataToString[index]}" : $", {temp.DataToString[index]}";
				}

				// 1-2. UI 업데이트
				for (int i = 0; i < resultTexts.Length; i++)
				{
					resultTexts[i].text = resultString;
				}
			}

			// 2. 나머지 투표 결과
			string elseResultString = string.Empty;
			{
				// 2-1. 문자열 생성
				SeatDataOption temp = voteManager.GetSeatDataOption(nameof(VoteSeat.VoteIndex));
				int[] sortIndexes = voteManager.GetSortVoteIndex();
				for (int i = 0; i < sortIndexes.Length; i++)
				{
					int index = sortIndexes[i];
					if (excludeZeroVote && voteManager.GetVoteCount(index) == 0)
						continue;

					elseResultString += $"{temp.DataToString[index]} \t: {voteManager.GetVoteCount(index)}표\n";
				}

				// 2-2. UI 업데이트
				for (int i = 0; i < elseResultTexts.Length; i++)
				{
					elseResultTexts[i].text = elseResultString;
				}
			}

			// 3. Debug UI 업데이트
			string debugString = voteManager.GetDebugString();
			foreach (TextMeshProUGUI debugText in debugTexts)
				debugText.text = debugString;
		}
	}
}