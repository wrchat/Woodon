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
			int score = mScore.Value;

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
		public void AddScore10() => mScore.AddValue(mScore.IncreaseAmount * 10);
		public void DecreaseScore() => mScore.DecreaseScore();
		public void SubScore10() => mScore.SubValue(mScore.DecreaseAmount * 10);
		public void ResetScore() => mScore.ResetScore();

		public void SetScore0() => mScore.SetValue(0);
		public void SetScore1() => mScore.SetValue(1);
		public void SetScore2() => mScore.SetValue(2);
		public void SetScore3() => mScore.SetValue(3);
		public void SetScore4() => mScore.SetValue(4);
		public void SetScore5() => mScore.SetValue(5);
		public void SetScore6() => mScore.SetValue(6);
		public void SetScore7() => mScore.SetValue(7);
		public void SetScore8() => mScore.SetValue(8);
		public void SetScore9() => mScore.SetValue(9);
		#endregion
	}
}