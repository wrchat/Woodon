
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615.Project.ISD.GSG.RealMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class RealMeetingManager : MBase
	{
		[SerializeField] private TextMeshProUGUI screenTitleText;
		[SerializeField] private TextMeshProUGUI screenText;
		[SerializeField] private string[] profileNames;

		[SerializeField] private Transform mScoresParent;
		private MScore[] mScores = new MScore[0];

		[SerializeField] private CustomBool isPollTargetMale;
		[SerializeField] private CustomBool isSecondPoll;

		private void Start()
		{
			mScores = new MScore[mScoresParent.childCount];

			for (int i = 0; i < mScoresParent.childCount; i++)
			{
				Transform c = mScoresParent.GetChild(i);

				TextMeshProUGUI nameText = c.GetChild(0).GetComponent<TextMeshProUGUI>();
				nameText.text = profileNames[i];

				mScores[i] = c.GetChild(1).GetComponent<MScore>();
			}

			MDebugLog($"Initialized {mScores.Length} Scores");

			UpdateScreen();
		}

		public void SetScreen(bool targetMale, bool secondPoll)
		{
			if (secondPoll)
				screenTitleText.text = $"{(targetMale ? "남성" : "여성")} 받은 하트 수";
			else
				screenTitleText.text = $"{(targetMale ? "남성" : "여성")} 첫인상 득표수";

			// Not Initialized
			if (mScores.Length == 0)
				return;

			string s = string.Empty;

			// TODO: Heart Image
			for (int i = 0; i < mScores.Length / 2; i++)
			{
				int actualIndex = targetMale ? i : (i + mScores.Length / 2);

				string name = profileNames[actualIndex];
				s += $"{name}: "; 
				
				int score = mScores[actualIndex].Score;
				for (int j = 0; j < score; j++)
					s += "❤️";

				s += "\n";
			}

			screenText.text = s;
		}

		public void UpdateScreen()
		{
			SetScreen(isPollTargetMale.Value, isSecondPoll.Value);
		}
	}
}