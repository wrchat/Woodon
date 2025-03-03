using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;
using UdonSharp;

namespace Mascari4615.Project.ISD.JRR.DateWithJRR
{
	[DefaultExecutionOrder(-10000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DateWithJRR_Manager : QuizManager
	{
		public const int NO_KAKAOTALK = 5;

		[Header("_" + nameof(DateWithJRR_Manager))]
		[SerializeField] private WInt curDetailAnswerIndex;
		public int CurDetailAnswerIndex => curDetailAnswerIndex.Value;
		[SerializeField] private TextMeshProUGUI answerDetailText;

		[SerializeField] private Sprite[] answerNumSprites;
		[SerializeField] private Image answerNumImage;

		[Header("_" + nameof(DateWithJRR_Manager) + " - KakaoTalk")]
		[SerializeField] private WInt curKakaotalkIndex;
		[SerializeField] private WString[] kakaotalkTextSyncs;
		[SerializeField] private Image[] kakaotalkBackgrounds;
		[SerializeField] private TextMeshProUGUI[] kakaotalkTexts;
		[SerializeField] private Image[] kakaotalkButtonImages;

		protected override void Init()
		{
			base.Init();
			curDetailAnswerIndex.RegisterListener(this, nameof(UpdateContent));
		}

		public override void UpdateContent()
		{
			CanSelectAnswer = IsContentState((int)QuizContentState.SelectAnswer);

			UpdateAnswerDatail();
			UpdateKaKaoTalk();

			base.UpdateContent();
		}

		private void UpdateAnswerDatail()
		{
			string[] quizAnswerStringArr = CurQuizData.QuizAnswerString.Split(DATA_SEPARATOR);
			bool noImage = (curDetailAnswerIndex.Value == 6)
				|| (curDetailAnswerIndex.Value >= quizAnswerStringArr.Length)
				|| (curDetailAnswerIndex.Value < 0);

			answerNumImage.gameObject.SetActive(noImage == false);
			if (noImage == false)
			{
				Sprite sprite = answerNumSprites[curDetailAnswerIndex.Value];
				answerNumImage.color = (sprite == null) ? Color.clear : Color.white;
				answerNumImage.sprite = sprite;

				answerDetailText.text = quizAnswerStringArr[curDetailAnswerIndex.Value];
			}
		}

		private void UpdateKaKaoTalk()
		{
			int curKakaotalkIndex = this.curKakaotalkIndex.Value;
			for (int i = 0; i < kakaotalkButtonImages.Length; i++)
				kakaotalkButtonImages[i].color = WColorUtil.GetBlackOrGray(i != curKakaotalkIndex);

			bool noKakao = curKakaotalkIndex == NO_KAKAOTALK;

			foreach (Image kakaotalkBackground in kakaotalkBackgrounds)
				kakaotalkBackground.gameObject.SetActive(noKakao == false);

			if (noKakao == false)
				foreach (TextMeshProUGUI kakaotalkText in kakaotalkTexts)
					kakaotalkText.text = kakaotalkTextSyncs[curKakaotalkIndex].Value;
		}

		public void ResetAnswers()
		{
			// if (IsOwner())
				foreach (WSeat seat in Seats)
					seat.ResetTurnData();
		}

		public void SetCurGameState_Wait()
		{
			SetContentState((int)QuizContentState.Wait);
		}

		public void SetCurGameState_SelectAnswer()
		{
			SetContentState((int)QuizContentState.SelectAnswer);
		}
	}
}