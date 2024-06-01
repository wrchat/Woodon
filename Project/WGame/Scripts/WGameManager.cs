using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WGameManager : QuizManager
	{
		[SerializeField] private MPlayerUdonIndex mPlayerUdonIndex;
		[SerializeField] private TextMeshProUGUI seatDebugText;
		[SerializeField] private TextMeshProUGUI debugText2;
		[SerializeField] private GameObject[] resultObjects;

		// 문제 UI
		[SerializeField] private Image[] leftImages;
		[SerializeField] private Image[] rightImages;
		[SerializeField] private TextMeshProUGUI leftText;
		[SerializeField] private TextMeshProUGUI rightText;
		[SerializeField] private Button leftSelectButton;
		[SerializeField] private Button rightSelectButton;
		[SerializeField] private MAnimator leftAnimator;
		[SerializeField] private MAnimator rightAnimator;
		[SerializeField] private SyncedBool leftAnimatorStateSyncedBool;
		[SerializeField] private SyncedBool rightAnimatorStateSyncedBool;

		// 어떤 거 선택했는지
		[SerializeField] private TextMeshProUGUI totalSelectCountText;
		[SerializeField] private TextMeshProUGUI leftSelectCountText;
		[SerializeField] private TextMeshProUGUI rightSelectCountText;

		[SerializeField] private Transform totalSelectIconsParent;
		[SerializeField] private Transform leftSelectIconsParent;
		[SerializeField] private Transform rightSelectIconsParent;

		private Image[] totalSelectIcons = new Image[] { };
		private Image[] leftSelectIcons = new Image[] { };
		private Image[] rightSelectIcons = new Image[] { };

		[SerializeField] private Image leftSelectBorder;
		[SerializeField] private Image rightSelectBorder;
		[SerializeField] private Animator selectBlockAnimator;

		[SerializeField] private Sprite[] playerIcons;
		[SerializeField] private Sprite nullIcon;

		// 사운드
		[SerializeField] private AudioClip clickSFX;

		protected override void Start()
		{
			totalSelectIcons = totalSelectIconsParent.GetComponentsInChildren<Image>(true);
			leftSelectIcons = leftSelectIconsParent.GetComponentsInChildren<Image>(true);
			rightSelectIcons = rightSelectIconsParent.GetComponentsInChildren<Image>(true);

			base.Start();
		}

		private void Update()
		{
			UpdateAnim();
		}

		private void UpdateAnim()
		{
			bool showingResult = (int)CurGameState >= (int)QuizGameState.ShowPlayerAnswer;

			int leftCount = answerCount[(int)QuizAnswerType.One];
			int rightCount = answerCount[(int)QuizAnswerType.Two];
			bool isLeftMore = leftCount > rightCount;
			selectBlockAnimator.SetInteger("STATE", (!showingResult || leftCount == rightCount) ? 0 : (isLeftMore ? -1 : 1));

			// MDebugLog($"STATE : {showingResult}, {leftCount}, {rightCount}, {isLeftMore}, {(!showingResult ? 0 : (isLeftMore ? -1 : 1))}");
		}

		private void UpdatePlayerIcon(Image[] iconImages, QuizAnswerType targetAnswerType, bool onWhenSame)
		{
			for (int i = 1; i < iconImages.Length; i++)
			{
				bool active = onWhenSame ? (QuizSeats[i - 1].ExpectedAnswer == targetAnswerType) : (QuizSeats[i - 1].ExpectedAnswer != targetAnswerType);

				iconImages[i].gameObject.SetActive(active);
				VRCPlayerApi playerApi = VRCPlayerApi.GetPlayerById(QuizSeats[i - 1].OwnerID);
				// iconImages[i].color = (playerApi == null) ? Color.white : GetColorByNickname(playerApi.displayName);

				Sprite sprite = nullIcon;
				if (playerApi != null)
				{
					int waktaIndex = WaktaverseNickname.GetIndex(playerApi.displayName);
					if (waktaIndex != NONE_INT)
						sprite = playerIcons[waktaIndex];
				}
				iconImages[i].sprite = sprite;
			}
		}

		public override void UpdateStuff()
		{
			base.UpdateStuff();
			bool showingResult = (int)CurGameState >= (int)QuizGameState.ShowPlayerAnswer;

			// 문제 UI
			{
				foreach (Image leftImage in leftImages)
					leftImage.sprite = CurQuizData.AnswerSprites[0];
				foreach (Image rightImage in rightImages)
					rightImage.sprite = CurQuizData.AnswerSprites[1];
				string[] q = CurQuizData.Quiz.Split('@');
				leftText.text = q[0];
				rightText.text = q[1];
			}

			// 어떤 거 선택했는지
			{
				UpdatePlayerIcon(totalSelectIcons, QuizAnswerType.None, false);
				UpdatePlayerIcon(leftSelectIcons, QuizAnswerType.One, true);
				UpdatePlayerIcon(rightSelectIcons, QuizAnswerType.Two, true);

				int totalSelectCount = 0;
				int leftSelectCount = 0;
				int rightSelectCount = 0;
				for (int i = 0; i < QuizSeats.Length; i++)
				{
					totalSelectCount += (QuizSeats[i].ExpectedAnswer != QuizAnswerType.None) ? 1 : 0;
					leftSelectCount += (QuizSeats[i].ExpectedAnswer == QuizAnswerType.One) ? 1 : 0;
					rightSelectCount += (QuizSeats[i].ExpectedAnswer == QuizAnswerType.Two) ? 1 : 0;
				}
				totalSelectCountText.text = totalSelectCount.ToString();
				leftSelectCountText.text = leftSelectCount.ToString();
				rightSelectCountText.text = rightSelectCount.ToString();

				QuizSeat localplayerQuizSeat = null;
				foreach (QuizSeat quizSeat in QuizSeats)
					if (quizSeat.IsLocalPlayerOwner)
					{
						localplayerQuizSeat = quizSeat;
						break;
					}

				bool showNow = !showingResult && (localplayerQuizSeat != null);
				leftSelectBorder.gameObject.SetActive(showNow && (localplayerQuizSeat.ExpectedAnswer == QuizAnswerType.One));
				rightSelectBorder.gameObject.SetActive(showNow && (localplayerQuizSeat.ExpectedAnswer == QuizAnswerType.Two));
			}

			// 결과
			{
				totalSelectIconsParent.gameObject.SetActive(!showingResult);

				foreach (var resultObject in resultObjects)
					resultObject.SetActive(showingResult);

				// UpdateAnim();

				bool allOpened = leftAnimatorStateSyncedBool.SyncValue && rightAnimatorStateSyncedBool.SyncValue;
				// MDebugLog($"allOpened : {allOpened}, {leftAnimatorStateSyncedBool.SyncValue}, {rightAnimatorStateSyncedBool.SyncValue} = {leftAnimatorStateSyncedBool.Value}, {rightAnimatorStateSyncedBool.Value}");

				leftSelectButton.interactable = allOpened;
				rightSelectButton.interactable = allOpened;

				// TODO : 애니메이션
				// 띵띵띵 개수 하나씩 올라가면서 수 비교하는,
				// 최종적으로 많이 선택한 쪽 더 넓은 공간 차지하게 하는
			}

			// 디버그
			{
				string debugSeatString = string.Empty;
				for (int i = 0; i < QuizSeats.Length; i++)
					debugSeatString += $"{i} :: {QuizSeats[i].OwnerID}\n";
				seatDebugText.text = debugSeatString;
			}
		}
		public override void OnQuizIndexChange()
		{
			leftAnimator.SetBool(false);
			rightAnimator.SetBool(false);
			base.OnQuizIndexChange();
		}

		public void SelectLeft() => SelectAnsnwer(QuizAnswerType.One);
		public void SelectRight() => SelectAnsnwer(QuizAnswerType.Two);
		private void SelectAnsnwer(QuizAnswerType answerType)
		{
			QuizSeat localPlayerSeat = GetLocalPlayerQuizSeat();

			if (localPlayerSeat == null)
				return;

			if (localPlayerSeat.ExpectedAnswer == answerType)
			{
				if (CurGameState == QuizGameState.ShowPlayerAnswer)
					return;
				localPlayerSeat.SelectAnswerForce(QuizAnswerType.None);
			}
			else
				localPlayerSeat.SelectAnswerForce(answerType);

			sfxManager.PlaySFX(clickSFX);
		}

		private QuizSeat GetLocalPlayerQuizSeat()
		{
			int localPlayerUdonIndex = mPlayerUdonIndex.GetUdonIndex(Networking.LocalPlayer);

			if (localPlayerUdonIndex == NONE_INT)
			{
				MDebugLog("!!! : A");
				debugText2.text = "A";
				return null;
			}

			QuizSeat localplayerQuizSeat = QuizSeats[localPlayerUdonIndex];
			MDebugLog(localplayerQuizSeat.ToString());

			if (QuizSeats[localPlayerUdonIndex].OwnerID != Networking.LocalPlayer.playerId)
			{
				MDebugLog("!!! : B");
				debugText2.text = "B";
				QuizSeats[localPlayerUdonIndex].UseSeat();
			}

			return localplayerQuizSeat;
		}

		public override void SetQuizIndex(int newIndex)
		{
			if (0 <= newIndex && newIndex <= QuizDatas.Length - 1)
			{
				if (IsOwner())
					foreach (var quizSeat in QuizSeats)
						quizSeat.ResetAnswer();

				SetOwner();
				leftAnimatorStateSyncedBool.SetValue(false);
				rightAnimatorStateSyncedBool.SetValue(false);
				CurGameState = QuizGameState.SelectAnswer;
				CurQuizIndex = newIndex;
				RequestSerialization();
			}
		}

		public void ToggleShowAnswer()
		{
			if (CurGameState == QuizGameState.SelectAnswer)
			{
				SetCurGameState(QuizGameState.ShowPlayerAnswer);
				sfxManager.PlaySFX_G(1);
			}
			else if (CurGameState == QuizGameState.ShowPlayerAnswer)
				SetCurGameState(QuizGameState.SelectAnswer);
		}
	}
}