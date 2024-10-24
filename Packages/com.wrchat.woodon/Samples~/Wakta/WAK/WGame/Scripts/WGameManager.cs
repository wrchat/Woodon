using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615.Project.Wakta.WAK.WGame
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
		[SerializeField] private MBool leftAnimatorStateCustomBool;
		[SerializeField] private MBool rightAnimatorStateCustomBool;

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
		[SerializeField] private MSFXManager sfxManager;

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
				bool active = onWhenSame ? (TurnSeats[i - 1].TurnData == (int)targetAnswerType) : (TurnSeats[i - 1].TurnData != (int)targetAnswerType);

				iconImages[i].gameObject.SetActive(active);
				VRCPlayerApi playerApi = VRCPlayerApi.GetPlayerById(TurnSeats[i - 1].TargetPlayerID);
				// iconImages[i].color = (playerApi == null) ? Color.white : GetColorByNickname(playerApi.displayName);

				Sprite sprite = nullIcon;
				if (playerApi != null)
				{
					int waktaIndex = Waktaverse.GetIndex(playerApi.displayName);
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
				for (int i = 0; i < TurnSeats.Length; i++)
				{
					totalSelectCount += (TurnSeats[i].TurnData != (int)QuizAnswerType.None) ? 1 : 0;
					leftSelectCount += (TurnSeats[i].TurnData == (int)QuizAnswerType.One) ? 1 : 0;
					rightSelectCount += (TurnSeats[i].TurnData == (int)QuizAnswerType.Two) ? 1 : 0;
				}
				totalSelectCountText.text = totalSelectCount.ToString();
				leftSelectCountText.text = leftSelectCount.ToString();
				rightSelectCountText.text = rightSelectCount.ToString();

				QuizSeat localplayerQuizSeat = null;
				foreach (QuizSeat quizSeat in TurnSeats)
					if (IsLocalPlayerID(quizSeat.TargetPlayerID))
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

				foreach (GameObject resultObject in resultObjects)
					resultObject.SetActive(showingResult);

				// UpdateAnim();

				bool allOpened = leftAnimatorStateCustomBool.SyncedValue && rightAnimatorStateCustomBool.SyncedValue;
				// MDebugLog($"allOpened : {allOpened}, {leftAnimatorStateCustomBool.SyncValue}, {rightAnimatorStateCustomBool.SyncValue} = {leftAnimatorStateCustomBool.Value}, {rightAnimatorStateCustomBool.Value}");

				leftSelectButton.interactable = allOpened;
				rightSelectButton.interactable = allOpened;

				// TODO : 애니메이션
				// 띵띵띵 개수 하나씩 올라가면서 수 비교하는,
				// 최종적으로 많이 선택한 쪽 더 넓은 공간 차지하게 하는
			}

			// 디버그
			{
				string debugSeatString = string.Empty;
				for (int i = 0; i < TurnSeats.Length; i++)
					debugSeatString += $"{i} :: {TurnSeats[i].TargetPlayerID}\n";
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
				if (CurGameState == (int)QuizGameState.ShowPlayerAnswer)
					return;
				localPlayerSeat.SelectAnswer(QuizAnswerType.None, force: true);
			}
			else
				localPlayerSeat.SelectAnswer(answerType, force: true);

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

			QuizSeat localplayerQuizSeat = (QuizSeat)TurnSeats[localPlayerUdonIndex];
			MDebugLog(localplayerQuizSeat.ToString());

			if (TurnSeats[localPlayerUdonIndex].TargetPlayerID != Networking.LocalPlayer.playerId)
			{
				MDebugLog("!!! : B");
				debugText2.text = "B";
				TurnSeats[localPlayerUdonIndex].UseSeat();
			}

			return localplayerQuizSeat;
		}

		public override void OnWait()
		{
			base.OnWait();

			if (IsOwner() == false)
				return;

			leftAnimatorStateCustomBool.SetValue(false);
			rightAnimatorStateCustomBool.SetValue(false);
			SetGameState((int)QuizGameState.SelectAnswer);
		}

		public void ToggleShowAnswer()
		{
			if (CurGameState == (int)QuizGameState.SelectAnswer)
			{
				SetGameState((int)QuizGameState.ShowPlayerAnswer);
				sfxManager.PlaySFX_G(1);
			}
			else if (CurGameState == (int)QuizGameState.ShowPlayerAnswer)
				SetGameState((int)QuizGameState.SelectAnswer);
		}
	}
}