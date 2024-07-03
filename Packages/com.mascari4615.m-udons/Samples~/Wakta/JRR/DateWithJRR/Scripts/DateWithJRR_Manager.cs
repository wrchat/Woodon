using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615.Project.ISD.JRR.DateWithJRR
{
	public class DateWithJRR_Manager : QuizManager
	{
		[Header("⭐" + nameof(DateWithJRR_Manager))]
		[SerializeField] private Image answerImage;
		[SerializeField] private Sprite[] answerSprites;
		[SerializeField] private TextMeshProUGUI answerText;
		[SerializeField] private Image[] detailAnswerButtonImages;

		[SerializeField] private Image[] kakaotalkBackgrounds;
		[SerializeField] private TextMeshProUGUI[] kakaotalkTexts;
		[SerializeField] private Image[] kakaotalkButtonImages;
		[SerializeField] private MTextSync[] kakaotalkTextSyncs;

		[SerializeField] private MScore CurDetailAnswerIndex;

		protected override void Init()
		{
			if (IsInited)
				return;

			base.Init();
			IsInited = true;

			CurDetailAnswerIndex.RegisterListener(this, nameof(UpdateStuff));
		}

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			{
				for (int i = 0; i < detailAnswerButtonImages.Length; i++)
					detailAnswerButtonImages[i].color = MColorUtil.GetBlackOrGray(i != CurDetailAnswerIndex.Score);

				string[] qasArr = CurQuizData.QuizAnswerString.Split(DATA_SEPARATOR);

				bool noImage = (CurDetailAnswerIndex.Score == 6) || (CurDetailAnswerIndex.Score >= qasArr.Length);

				answerImage.gameObject.SetActive(!noImage);
				if (!noImage)
				{
					answerImage.sprite = answerSprites[CurDetailAnswerIndex.Score];
					answerText.text = qasArr[CurDetailAnswerIndex.Score].Split('_')[0];
				}

				curQuizImages[0].gameObject.SetActive(CurDetailAnswerIndex.Score == 5);
			}

			{
				for (int i = 0; i < kakaotalkButtonImages.Length; i++)
					kakaotalkButtonImages[i].color = MColorUtil.GetBlackOrGray(i != CurKakaotalkIndex.Score);

				bool noKakao = (CurKakaotalkIndex.Score == 5);

				foreach (var kakaotalkBackground in kakaotalkBackgrounds)
					kakaotalkBackground.gameObject.SetActive(!noKakao);
				if (!noKakao)
					foreach (var kakaotalkText in kakaotalkTexts)
						kakaotalkText.text = kakaotalkTextSyncs[CurKakaotalkIndex.Score].SyncText;
			}

			CanSelectAnsewr = CurGameState == (int)QuizGameState.SelectAnswer;
		}

		public void ResetAnswers()
		{
			if (IsOwner())
				foreach (MTurnSeat turnSeat in TurnSeats)
					turnSeat.ResetData();
		}

		public override void OnQuizIndexChange()
		{
			base.OnQuizIndexChange();

			if (IsOwner())
				foreach (MTurnSeat turnSeat in TurnSeats)
					turnSeat.ResetData();
		}

		[SerializeField] private MScore CurKakaotalkIndex;

		[SerializeField] private M_CCManager cameraManager;

		public void FullScreenOn_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOn));
		public void FullScreenOn() => cameraManager.SetCamera(4, alwaysOn: true);
		public void FullScreenOff_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOff));
		public void FullScreenOff() => cameraManager.TurnOffCamera();
	}
}