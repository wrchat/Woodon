using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class JRRManager : QuizManager
	{
		[Header("⭐" + nameof(JRRManager))]
		[SerializeField] private Image answerImage;
		[SerializeField] private Sprite[] answerSprites;
		[SerializeField] private TextMeshProUGUI answerText;
		[SerializeField] private Image[] detailAnswerButtonImages;

		[SerializeField] private Image[] kakaotalkBackgrounds;
		[SerializeField] private TextMeshProUGUI[] kakaotalkTexts;
		[SerializeField] private Image[] kakaotalkButtonImages;
		[SerializeField] private MTextSync[] kakaotalkTextSyncs;

		public int CurDetailAnswerIndex
		{
			get => _curDetailAnswerType;
			set
			{
				_curDetailAnswerType = value;
				UpdateStuff();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurDetailAnswerIndex))]
		private int _curDetailAnswerType = 6;

		private void SetCurDetailAnswerIndex(int newIndex)
		{
			SetOwner();
			CurDetailAnswerIndex = newIndex;
			RequestSerialization();
		}
		public void SetCurDetailAnswerIndex0() => SetCurDetailAnswerIndex(0);
		public void SetCurDetailAnswerIndex1() => SetCurDetailAnswerIndex(1);
		public void SetCurDetailAnswerIndex2() => SetCurDetailAnswerIndex(2);
		public void SetCurDetailAnswerIndex3() => SetCurDetailAnswerIndex(3);
		public void SetCurDetailAnswerIndex4() => SetCurDetailAnswerIndex(4);
		public void SetCurDetailAnswerIndex5() => SetCurDetailAnswerIndex(5);
		public void SetCurDetailAnswerIndex6() => SetCurDetailAnswerIndex(6);

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			{
				for (int i = 0; i < detailAnswerButtonImages.Length; i++)
					detailAnswerButtonImages[i].color = MColorUtil.GetBlackOrGray(i != CurDetailAnswerIndex);

				string[] qasArr = CurQuizData.QuizAnswerString.Split(DATA_SEPARATOR);

				bool noImage = (CurDetailAnswerIndex == 6) || (CurDetailAnswerIndex >= qasArr.Length);

				answerImage.gameObject.SetActive(!noImage);
				if (!noImage)
				{
					answerImage.sprite = answerSprites[CurDetailAnswerIndex];
					answerText.text = qasArr[CurDetailAnswerIndex].Split('_')[0];
				}

				curQuizImages[0].gameObject.SetActive(CurDetailAnswerIndex == 5);
			}

			{
				for (int i = 0; i < kakaotalkButtonImages.Length; i++)
					kakaotalkButtonImages[i].color = MColorUtil.GetBlackOrGray(i != CurKakaotalkIndex);

				bool noKakao = (CurKakaotalkIndex == 5);

				foreach (var kakaotalkBackground in kakaotalkBackgrounds)
					kakaotalkBackground.gameObject.SetActive(!noKakao);
				if (!noKakao)
					foreach (var kakaotalkText in kakaotalkTexts)
						kakaotalkText.text = kakaotalkTextSyncs[CurKakaotalkIndex].SyncText;
			}

			CanSelectAnsewr = CurGameState == QuizGameState.SelectAnswer;
		}

		public void ResetAnswers()
		{
			if (IsOwner())
				foreach (var quizSeat in QuizSeats)
					quizSeat.ResetAnswer();
		}

		public override void OnQuizIndexChange()
		{
			base.OnQuizIndexChange();

			if (IsOwner())
				foreach (var quizSeat in QuizSeats)
					quizSeat.ResetAnswer();
		}

		public int CurKakaotalkIndex
		{
			get => _curKakaotalkIndex;
			set
			{
				_curKakaotalkIndex = value;
				UpdateStuff();
			}
		}
		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(CurKakaotalkIndex))]
		private int _curKakaotalkIndex = 5;

		private void SetCurKakaotalkIndex(int newIndex)
		{
			SetOwner();
			CurKakaotalkIndex = newIndex;
			RequestSerialization();
		}
		public void SetCurKakaotalkIndex0() => SetCurKakaotalkIndex(0);
		public void SetCurKakaotalkIndex1() => SetCurKakaotalkIndex(1);
		public void SetCurKakaotalkIndex2() => SetCurKakaotalkIndex(2);
		public void SetCurKakaotalkIndex3() => SetCurKakaotalkIndex(3);
		public void SetCurKakaotalkIndex4() => SetCurKakaotalkIndex(4);
		public void SetCurKakaotalkIndex5() => SetCurKakaotalkIndex(5);

		[SerializeField] private MTarget jrrTarget;
		[SerializeField] private GameObject uiPanrt_JRR;

		[SerializeField] private MCameraManager cameraManager;

		public void FullScreenOn_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOn));
		public void FullScreenOn() => cameraManager.SetCamera(4, alwaysOn: true);
		public void FullScreenOff_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FullScreenOff));
		public void FullScreenOff() => cameraManager.TurnOffCamera();
	}
}