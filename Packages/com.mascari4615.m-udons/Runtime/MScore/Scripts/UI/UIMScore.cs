using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMScore : MBase
	{
		[Header("_" + nameof(UIMScore))]
		[SerializeField] private MScore mScore;
		[SerializeField] private TextMeshProUGUI[] scoreTexts;
		[SerializeField] private string format = "{0}";
		[SerializeField] private GameObject[] hardButtons;
		[SerializeField] private float printMultiply = 1;
		[SerializeField] private int printPlus = 0;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			mScore.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void UpdateUI()
		{
			int score = mScore.Score;

			string scoreString = ((int)(score * printMultiply) + printPlus).ToString();
			string finalString = string.Format(format, scoreString);
			finalString = finalString.Replace("MAX", mScore.MaxScore.ToString());

			foreach (TextMeshProUGUI scoreText in scoreTexts)
				scoreText.text = finalString;

			for (int i = 0; i < hardButtons.Length; i++)
				hardButtons[i].SetActive(mScore.MinScore <= i && i <= mScore.MaxScore);
		}

		#region HorribleEvents
		public void IncreaseScore() => mScore.IncreaseScore();
		public void AddScore10() => mScore.AddScore(mScore.IncreaseAmount * 10);
		public void DecreaseScore() => mScore.DecreaseScore();
		public void SubScore10() => mScore.SubScore(mScore.DecreaseAmount * 10);
		public void ResetScore() => mScore.ResetScore();

		public void SetScore0() => mScore.SetScore(0);
		public void SetScore1() => mScore.SetScore(1);
		public void SetScore2() => mScore.SetScore(2);
		public void SetScore3() => mScore.SetScore(3);
		public void SetScore4() => mScore.SetScore(4);
		public void SetScore5() => mScore.SetScore(5);
		public void SetScore6() => mScore.SetScore(6);
		public void SetScore7() => mScore.SetScore(7);
		public void SetScore8() => mScore.SetScore(8);
		public void SetScore9() => mScore.SetScore(9);
		#endregion
	}
}