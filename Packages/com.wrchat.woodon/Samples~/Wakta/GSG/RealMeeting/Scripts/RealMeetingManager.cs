using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RealMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class RealMeetingManager : MBase
	{
		[SerializeField] private Image screenImage;
		[SerializeField] private Sprite[] screenSprites;
		[SerializeField] private Sprite[] heartSprites;
		[SerializeField] private string[] profileNames;

		[SerializeField] private Transform mScoresParent;
		private MValue[] mScores = new MValue[0];

		[SerializeField] private MBool isPollTargetMale;

		private UIHeartBlock[] heartBlocks;

		private void Start()
		{
			Init();
			UpdateScreen();
		}

		public void Init()
		{
			mScores = new MValue[mScoresParent.childCount];
			for (int i = 0; i < mScoresParent.childCount; i++)
			{
				Transform c = mScoresParent.GetChild(i).GetChild(0);

				TextMeshProUGUI debugNameText = c.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>();
				debugNameText.text = profileNames[i];

				mScores[i] = c.GetComponent<MValue>();
			}

			heartBlocks = GetComponentsInChildren<UIHeartBlock>(true);
			foreach (UIHeartBlock heartBlock in heartBlocks)
				heartBlock.Init();

			MDebugLog($"Initialized {mScores.Length} Scores");
		}

		public void UpdateScreen()
		{
			SetScreen(isPollTargetMale.Value);
		}

		public void SetScreen(bool targetMale)
		{
			// Not Initialized
			if (mScores.Length == 0)
				return;

			screenImage.sprite = screenSprites[targetMale ? 0 : 1];
			Sprite heartSprite = heartSprites[targetMale ? 0 : 1];

			for (int i = 0; i < mScores.Length / 2; i++)
			{
				int actualIndex = targetMale ? i : (i + mScores.Length / 2);
				int score = mScores[actualIndex].Value;

				heartBlocks[i].UpdateUI(heartSprite, score);
			}
		}
	}
}