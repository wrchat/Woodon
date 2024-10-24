using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

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
		[SerializeField] private MString[] kakaotalkTextSyncs;

		[SerializeField] private MValue curDetailAnswerIndex;
		public int CurDetailAnswerIndex => curDetailAnswerIndex.Value;

		protected override void Init()
		{
			if (IsInited)
				return;

			base.Init();
			IsInited = true;

			curDetailAnswerIndex.RegisterListener(this, nameof(UpdateStuff));
		}

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			{
				for (int i = 0; i < detailAnswerButtonImages.Length; i++)
					detailAnswerButtonImages[i].color = MColorUtil.GetBlackOrGray(i != curDetailAnswerIndex.Value);

				string[] qasArr = CurQuizData.QuizAnswerString.Split(DATA_SEPARATOR);

				bool noImage = (curDetailAnswerIndex.Value == 6) || (curDetailAnswerIndex.Value >= qasArr.Length);

				answerImage.gameObject.SetActive(!noImage);
				if (!noImage)
				{
					answerImage.sprite = answerSprites[curDetailAnswerIndex.Value];
					answerText.text = qasArr[curDetailAnswerIndex.Value].Split('_')[0];
				}
			}

			{
				for (int i = 0; i < kakaotalkButtonImages.Length; i++)
					kakaotalkButtonImages[i].color = MColorUtil.GetBlackOrGray(i != CurKakaotalkIndex.Value);

				bool noKakao = (CurKakaotalkIndex.Value == 5);

				foreach (Image kakaotalkBackground in kakaotalkBackgrounds)
					kakaotalkBackground.gameObject.SetActive(!noKakao);
				if (!noKakao)
					foreach (TextMeshProUGUI kakaotalkText in kakaotalkTexts)
						kakaotalkText.text = kakaotalkTextSyncs[CurKakaotalkIndex.Value].Value;
			}

			CanSelectAnsewr = CurGameState == (int)QuizGameState.SelectAnswer;
		}

		public void ResetAnswers()
		{
			if (IsOwner())
				foreach (MTurnSeat turnSeat in TurnSeats)
					turnSeat.ResetData();
		}

		[SerializeField] private MValue CurKakaotalkIndex;

		[SerializeField] private M_CCManager cameraManager;

		public void FullScreenOn_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOn));
		public void FullScreenOn() => cameraManager.SetCamera(4, alwaysOn: true);
		public void FullScreenOff_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOff));
		public void FullScreenOff() => cameraManager.TurnOffCamera();
	}
}