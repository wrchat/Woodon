using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Video.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Enums;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class TGameManager : MBase
	{
		public const int PLAYER_COUNT = 7;
		private const float POINTING_TIME_BY_SEC = 180;

		private readonly int[] SeatIndexByRanking = new int[] { 0, 1, 5, 6, 4, 2, 3 };

		[field: Header("_" + nameof(TGameManager))]
		[field: SerializeField] public TGameData Data { get; private set; }
		[field: SerializeField] public TGameBank Bank { get; private set; }

		public int[] TotalCoinsByNumber { get; private set; }
		public int[] SortedTotalCoinsByNumber { get; private set; }
		private int[] NumberByRanking;

		private TGameCoinMemo coinMemo;
		private OrderPanel3 orderPanel;

		// Pos
		[SerializeField] private Transform lobbyPos;
		[SerializeField] private Transform stagePos;
		[SerializeField] private Transform jailPos;
		[SerializeField] private Transform gunHidePos;
		[SerializeField] private Transform lobbyStaffAPos;
		[SerializeField] private Transform lobbyStaffBPos;
		[SerializeField] private Transform lobbyStaffCPos;
		[SerializeField] private Transform lowerStaffCPos;
		[SerializeField] private Transform lowerStaffPos;
		[SerializeField] private Transform lowerNonePlayerPos;

		// UI
		private TGameUIRoot[] uiRoots;
		private ThiefSeat[] thiefSeats;
		[SerializeField] private MTarget[] mTargets;
		[SerializeField] private MTarget staffAMTarget;
		[SerializeField] private MTarget staffBMTarget;
		[SerializeField] private MTarget staffCMTarget;
		[SerializeField] private GameObject[] judgeStateInfos;
		private MGun gun;
		[SerializeField] private VoiceRoom[] voiceRooms;
		[SerializeField] private Animator jailAnimator;
		[SerializeField] private Sprite[] playerImages;
		[SerializeField] private Sprite[] playerRouletteImages;
		[SerializeField] private Image[] playerDebugImages;
		[SerializeField] private GameObject endStealButton;
		[SerializeField] private Image curPlayerImage;
		[SerializeField] private ParticleSystem _particleSystem;
		[SerializeField] private MSFXManager SFXManager;
		[SerializeField] private GameObject bgmAudiosource;
		[SerializeField] private TextMeshProUGUI diedResultText;
		private int shootCount = 0;
		private float t = 0;
		[SerializeField] private GameObject pointingTimeSFX;
		[SerializeField] private MBool videoScreenActive;
		[SerializeField] private VRCUnityVideoPlayer unityVideoPlayer;
		[SerializeField] private VRCUrl endingVideoURL;

		private TGameOverlayUI[] overlayUIs;
		private TGameOverlayUI curOverlayUI;
		private bool isVR;
		[SerializeField] private GameObject[] pcObjects;

		public void UpdateVR()
		{
			if (curOverlayUI != null)
				curOverlayUI.gameObject.SetActive(false);
			curOverlayUI = isVR ? overlayUIs[1] : overlayUIs[0];
			curOverlayUI.gameObject.SetActive(Data.CurRound > NONE_INT);

			foreach (GameObject pcObject in pcObjects)
				pcObject.SetActive(isVR == false);
		}

		private void Update()
		{
			if (t >= 0)
				t -= Time.deltaTime;

			pointingTimeSFX.SetActive(t > 0);
			curOverlayUI.UpdateTimeText(t);

			// DEBUG : ToggleVR
			// if (Input.GetKeyDown(KeyCode.F11))
			// {
			// 	isVR = !isVR;
			// 	UpdateVR();
			// }
		}

		private int round = NONE_INT;
		public void OnRoundChange()
		{
			MDebugLog($"{nameof(OnRoundChange)} : {round} -> {Data.CurRound}");

			// Only If Value Changed
			if (round != Data.CurRound)
			{
				round = Data.CurRound;

				// Only If Valid Value
				if (Data.CurRound > NONE_INT)
				{
					MDebugLog($"{nameof(OnRoundChange)}, Popup");

					//HACK
					videoScreenActive.SetValue(false);
					bgmAudiosource.SetActive(false);
					curOverlayUI.InfoPopupModule.Popup(0);
					SFXManager.PlaySFX_L(5);
					coinMemo.ResetMemo();
				}
			}

			curOverlayUI.UpdateUI();
		}

		public void RoundChangePOpup()
		{
			curOverlayUI.InfoPopupModule.Popup(0);
		}

		public bool IsGaming => Data.CurRound > NONE_INT;
		public TGameRoundData CurRoundData
		{
			get
			{
				if (Data.CurRound != NONE_INT)
					return Data.RoundDatas[Data.CurRound];
				else
				{
					MDebugLog($"CurRoundData, Invalid round {Data.CurRound}");
					return null;
				}
			}
		}

		public bool IsLocalPlayerOrder(int order) => mTargets[CurRoundData.NumberByOrder[order]].TargetPlayerID == Networking.LocalPlayer.playerId;
		public bool IsLocalPlayerNumber(int number) => IsLocalPlayerID(mTargets[number].TargetPlayerID);
		public bool IsLastPlayer => IsLocalPlayerNumber(CurRoundData.NumberByOrder[PLAYER_COUNT - 1]);
		public bool IsStaffA => IsLocalPlayerID(staffAMTarget.TargetPlayerID);
		public bool IsStaffB => IsLocalPlayerID(staffBMTarget.TargetPlayerID);
		public bool IsStaffC => IsLocalPlayerID(staffCMTarget.TargetPlayerID);
		public bool IsStaff(VRCPlayerApi player)
		{
			foreach (MTarget target in mTargets)
			{
				if (target.TargetPlayerID == player.playerId)
					return false;
			}
			return true;
		}

		public VRCPlayerApi GetPlayerAPI(int playerNumber)
		{
			return mTargets[playerNumber].GetTargetPlayerAPI();
		}

		public string GetPlayerVRCNickname(int playerNumber)
		{
			VRCPlayerApi targetPlayer = VRCPlayerApi.GetPlayerById(mTargets[playerNumber].TargetPlayerID);
			string targetName = targetPlayer == null ? "None" : targetPlayer.displayName;
			return targetName;
		}

		public string GetPlayerName(int playerNumber, bool withDebug)
		{
			if (playerNumber < 0 || playerNumber >= playerImages.Length)
			{
				Debug.Log($"@@@@@@@@@@@@@@@@ {nameof(GetPlayerName)} : Invalid PlayerNumber = {playerNumber}");
				return null;
			}

			string targetName = playerImages[playerNumber].name;
			return withDebug ? playerNumber + targetName : targetName;
		}

		public Sprite GetPlayerImage(int playerNumber)
		{
			if (playerNumber < 0 || playerNumber >= playerImages.Length)
			{
				Debug.LogError($"@@@@@@@@@@@@@@@@ {nameof(GetPlayerImage)} : Invalid PlayerNumber = {playerNumber}");
				return null;
			}

			return playerImages[playerNumber];
		}
		public Sprite GetPlayerRouletteImage(int playerNumber)
		{
			if (playerNumber < 0 || playerNumber >= playerImages.Length)
			{
				Debug.LogError($"@@@@@@@@@@@@@@@@ {nameof(GetPlayerRouletteImage)} : Invalid PlayerNumber = {playerNumber}");
				return null;
			}

			return playerRouletteImages[playerNumber];
		}

		public void TP_Lobby() => TP(
			IsStaffA ? lobbyStaffAPos :
			IsStaffB ? lobbyStaffBPos :
			IsStaffC ? lobbyStaffCPos : lobbyPos);

		public void TP_StagePos() => TP(stagePos);
		public void TP_Lower() => TP(lowerNonePlayerPos);
		public void ResetRound() => Data.Init();

		private bool isInited = false;

		private void Start()
		{
			MDebugLog($"{nameof(Start)}");

			/*Networking.LocalPlayer.CombatSetup();
			Networking.LocalPlayer.CombatSetDamageGraphic(null);
			Networking.LocalPlayer.CombatSetMaxHitpoints(100);
			Networking.LocalPlayer.CombatSetCurrentHitpoints(100);
			Networking.LocalPlayer.CombatSetRespawn(true, 1.5f, jailPos);
			Networking.LocalPlayer.CombatSetup();*/

			for (int i = 0; i < PLAYER_COUNT; i++)
				playerDebugImages[i].sprite = playerImages[i];

			if (Networking.IsMaster)
			{
				Data.Init();
			}
			else
			{
				OnGameDataChange();
			}

			Init();
		}

		public void CallUpdateColor()
		{
			// Update UI
			SendCustomEventDelayedSeconds(nameof(CallUpdateColor), .1f, EventTiming.LateUpdate);
			foreach (TGameUIRoot thiefGameScreenCanvas in uiRoots)
				thiefGameScreenCanvas.UpdateUI();
		}

		public void Init()
		{
			if (isInited)
				return;
			isInited = true;

			coinMemo = GetComponentInChildren<TGameCoinMemo>(true);
			coinMemo.Init(this);
			coinMemo.ResetMemo();

			orderPanel = GetComponentInChildren<OrderPanel3>(true);
			orderPanel.Init(this);

			thiefSeats = GetComponentsInChildren<ThiefSeat>(true);
			gun = GetComponentInChildren<MGun>(true);

			uiRoots = GetComponentsInChildren<TGameUIRoot>(true);
			foreach (TGameUIRoot uiRoot in uiRoots)
				uiRoot.Init(this);
			CallUpdateColor();

			overlayUIs = GetComponentsInChildren<TGameOverlayUI>(true);
			foreach (TGameOverlayUI overlayUI in overlayUIs)
			{
				overlayUI.Init(this);
				overlayUI.gameObject.SetActive(false);
			}
			isVR = Networking.LocalPlayer.IsUserInVR();
			UpdateVR();
		}

		public void NextRound()
		{
			MDebugLog($"{nameof(NextRound)}");

			// 마지막 라운드
			if (Data.IsLastRound) return;

			Data.NextRound();

			SetOwner();

			Bank.InitCoin();

			// orderPanel.Init();
			orderPanel.Hide();
			RequestSerialization();

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(InitGunPos));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPointingTime));

			// 두 번째 라운드부터 텔레포트
			if (Data.CurRound >= 1)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TP_Lobby));

			if (Data.CurRound == 0)
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(RoundChangePOpup));
		}

		public void InitGunPos() => gun.Respawn();
		public void HideGun() => gun.MoveTo(gunHidePos.position);

		private void CalcTotalCoin()
		{
			TotalCoinsByNumber = new int[PLAYER_COUNT];
			SortedTotalCoinsByNumber = new int[PLAYER_COUNT];
			NumberByRanking = new int[PLAYER_COUNT] { 0, 1, 2, 3, 4, 5, 6 };

			foreach (TGameRoundData roundData in  Data.RoundDatas)
			{
				// Calc TotalCoin
				for (int order = 0; order < PLAYER_COUNT; order++)
				{
					if (roundData.NumberByOrder == null)
						continue;

					if (roundData.NumberByOrder[order] <= NONE_INT)
						continue;

					if (roundData.FinalCoinByOrder[order] <= NONE_INT)
						continue;

					TotalCoinsByNumber[roundData.NumberByOrder[order]] += roundData.FinalCoinByOrder[order];
				}
			}

			Array.Copy(TotalCoinsByNumber, SortedTotalCoinsByNumber, PLAYER_COUNT);

			for (int i = 0; i < PLAYER_COUNT - 1; i++)
			{
				for (int j = 0; j < PLAYER_COUNT - 1; j++)
				{
					if (SortedTotalCoinsByNumber[j] < SortedTotalCoinsByNumber[j + 1])
					{
						int temp = SortedTotalCoinsByNumber[j + 1];
						SortedTotalCoinsByNumber[j + 1] = SortedTotalCoinsByNumber[j];
						SortedTotalCoinsByNumber[j] = temp;

						temp = NumberByRanking[j + 1];
						NumberByRanking[j + 1] = NumberByRanking[j];
						NumberByRanking[j] = temp;
					}
				}
			}
		}

		public void OnGameDataChange()
		{
			Init();

			if (Data.GameData == string.Empty || !Data.GameData.Contains(DATA_PACK_SEPARATOR.ToString()))
				return;

			MDebugLog($"{nameof(OnGameDataChange)}");

			Bank.UpdateUI();

			curOverlayUI.gameObject.SetActive(Data.CurRound > NONE_INT);

			CalcTotalCoin();

			orderPanel.UpdateUI();

			if (IsGaming)
			{
				if ((int)CurRoundData.CurState <= (int)TGameRoundState.Steal6)
				{
					// MDebugLog($"{CurRoundData.PlayerNumberByOrder} = {CurRoundData.PlayerNumberByOrder.Length}");
					// MDebugLog($"AAA = {(int)CurRoundData.CurState - (int)TGameRoundState.Steal0}");
					int curPlayerIndex =
						CurRoundData.NumberByOrder[(int)CurRoundData.CurState - (int)TGameRoundState.Steal0];
					curPlayerImage.sprite = GetPlayerImage(curPlayerIndex);
				}
				foreach (GameObject judgeStateInfo in judgeStateInfos)
					judgeStateInfo.SetActive(CurRoundData.CurState == TGameRoundState.Judge);

				string diedResult = "중복 플레이어\n";
				for (int order = 0; order < PLAYER_COUNT; order++)
				{
					if (CurRoundData.IsPlayerShooted(CurRoundData.NumberByOrder[order]))
						continue;

					if (CurRoundData.DiedNumberByOrder[order])
						diedResult +=
							$"{CurRoundData.CoinByOrder[order]}개" +
							$"'{GetPlayerName(CurRoundData.NumberByOrder[order], false)}'\n";
				}
				diedResultText.text = diedResult;

				coinMemo.UpdateUI();

				// Pointer Or Staff
				gun.CanInteract = IsLocalPlayerNumber(CurRoundData.PointerNumber) || IsStaff(Networking.LocalPlayer);
			}

			if (!IsGaming || ((int)CurRoundData.CurState > (int)TGameRoundState.Judge))
			{
				foreach (TGameUIRoot thiefGameScreenCanvas in uiRoots)
					thiefGameScreenCanvas.LeaderBoardPanel.UpdateUI();
				curOverlayUI.UpdateLeaderBoard();
			}

			jailAnimator.SetBool("Up", IsGaming && (CurRoundData.CurState == TGameRoundState.End));
			endStealButton.SetActive(IsGaming && (CurRoundData.CurState == TGameRoundState.Pointing)
													  && IsLocalPlayerNumber(CurRoundData.PointerNumber) /*&& (shootCount > 0)*/);

			curOverlayUI.UpdateUI();
		}

		public void InfoPopup(TGameRoundState roundState, int infoIndex)
		{
			curOverlayUI.InfoPopupModule.Popup(infoIndex);
			SFXManager.PlaySFX_L(5);
			bgmAudiosource.SetActive(((int)roundState >= (int)TGameRoundState.Pointing) && !((Data.CurRound == 4) && (roundState == TGameRoundState.End)));
		}

		public void StartPointing_G()
		{
			if (IsGaming == false ||
				CurRoundData.CurState != TGameRoundState.Pointing)
				return;

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(StartPointing));
		}

		public void StartPointing()
		{
			MDebugLog(nameof(StartPointing));
			InfoPopup(TGameRoundState.Pointing, 1);

			shootCount = 0;

			for (int i = 0; i < PLAYER_COUNT; i++)
			{
				bool isPointerSeat = CurRoundData.NumberByOrder[i] == CurRoundData.PointerNumber;
				thiefSeats[i].SetShootingTargetActive(isPointerSeat == false);
				thiefSeats[i].SetMemoActive(false);
			}

			if (IsLocalPlayerNumber(CurRoundData.PointerNumber))
			{
				foreach (VoiceRoom voiceRoom in voiceRooms)
					voiceRoom.ResetSync();
				SendCustomEventDelayedSeconds(nameof(TP_StagePos), .3f);
				TP_StagePos();
			}
			else if (IsStaffA) TP(lowerStaffPos);
			else if (IsStaffC) TP(lowerStaffCPos);
			else if (IsStaff(Networking.LocalPlayer)) TP(lowerNonePlayerPos);
			else SitSeat();

			ResetTime();
		}

		public void SitSeat()
		{
			MDebugLog(nameof(SitSeat));
			foreach (ThiefSeat thiefSeat in thiefSeats)
			{
				thiefSeat.SetMemoActive(false);
				thiefSeat.SetShootingTargetActive(true);
			}

			for (int i = 0; i < mTargets.Length; i++)
			{
				// if (IsLocalPlayerIndex(i))
				if (IsLocalPlayerOrder(i))
				{
					thiefSeats[i].UseStation();
					thiefSeats[i].SetMemoActive(true);
					return;
				}
			}

			TP(lowerStaffPos);
		}

		private bool IsEnding => Data.CurRound == 5 && CurRoundData.isRoundEnd;

		private int GetPlayerNumBySeatIndex(int seatIndex)
		{
			// 보통 라운드엔 Order순서대로 Seat에 앉음 (PlayerOrder == SeatIndex)
			if (IsEnding == false)
				return CurRoundData.NumberByOrder[seatIndex];
			// 엔딩엔 순위에 따라 정해진 순서대로 Seat에 앉음 (SeatIndexByRanking[Ranking] == SeatIndex)
			else
			{
				// 자리 인덱스를 통해, 엔딩 장면에서 해당 자리에 앉은 플레이어의 이름을 반환
				for (int ranking = 0; ranking < NumberByRanking.Length; ranking++)
				{
					if (SeatIndexByRanking[ranking] == seatIndex)
						return NumberByRanking[ranking];
				}
			}
			return NONE_INT;
		}

		public void Exit(int seatIndex)
		{
			MDebugLog(nameof(Exit));

			int targetNum = GetPlayerNumBySeatIndex(seatIndex);
			if (targetNum == NONE_INT)
			{
				MDebugLog($"{nameof(Exit)} Invalid TargetNum : {targetNum}");
				return;
			}
			bool isLocalPlayerVictim = IsLocalPlayerNumber(targetNum);

			// Effect
			JailSFX();
			curOverlayUI.KillLog(targetNum);
			if (targetNum == CurRoundData.PointerNumber)
				_particleSystem.Play();
			else
				thiefSeats[seatIndex].ParticleSystem.Play();

			if (isLocalPlayerVictim)
			{
				MDebugLog("Goto Jail");
				thiefSeats[seatIndex].ExitStation();
				TP(jailPos);

				// 래그돌
				// Networking.LocalPlayer.CombatSetCurrentHitpoints(0);
			}
		}

		// When Player(Seat) Shooted
		public void ShootPlayer_G(int seatIndex)
		{
			MDebugLog(nameof(ShootPlayer_G));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ShootPlayer) + seatIndex);
		}

		public void ShootPlayer(int seatIndex)
		{
			// 총을 쏘는 사람 : 지목자 (가장 코인을 적게 가져간 사람), 스태프 (중복 확인 단계)
			// 목적 : 총 맞은 사람을 감옥에 보내고, 게임 진행

			thiefSeats[seatIndex].SetShootingTargetActive(false);
			thiefSeats[seatIndex].SetMemoActive(false);
			CoinSFX();

			int victimNum = GetPlayerNumBySeatIndex(seatIndex);
			if (victimNum == NONE_INT)
			{
				MDebugLog($"{nameof(ShootPlayer)} Invalid TargetNum : {victimNum}, {nameof(seatIndex)} : {seatIndex}");
				return;
			}
			bool isValidShoot = CurRoundData.IsPlayerCoinOrder(victimNum, shootCount);
			bool isLocalPlayerPointer = IsLocalPlayerNumber(CurRoundData.PointerNumber);

			MDebugLog($"{nameof(ShootPlayer)} : " +
				$"{nameof(seatIndex)} = {seatIndex}, " +
				$"{nameof(victimNum)} = {victimNum}/{GetPlayerName(victimNum, false)} " +
				$"{nameof(isValidShoot)} = {isValidShoot}, " +
				$"{nameof(isLocalPlayerPointer)} = {isLocalPlayerPointer} " +
				$"{nameof(shootCount)} = {shootCount}++");

			// 지목자가 쏨
			if (CurRoundData.CurState == TGameRoundState.Pointing)
			{
				shootCount++;

				// 아래는 지목자만
				if (isLocalPlayerPointer == false)
					return;

				// 잘 쏜 경우
				if (isValidShoot)
				{
					MDebugLog($"{nameof(ShootPlayer)}, {victimNum}, Success");

					CurRoundData.ShootedNumbers[shootCount - 1] = victimNum;
					SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Exit) + seatIndex);

					if (shootCount == PLAYER_COUNT - 1)
					{
						CurRoundData.ShootedNumbers[PLAYER_COUNT - 1] = (int)TGamePointingResult.Success_AllKill;

						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(HideGun));
						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPointingTime));
						// SendCustomEventDelayedSeconds(nameof(StartJudge), 3f);
					}
					else
					{
						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetTime));
					}

					Data.SyncGameData();
				}
				// 실패한 경우
				else
				{
					MDebugLog($"{nameof(ShootPlayer)}, {victimNum}, Fail");

					// 스틸 실패
					CurRoundData.ShootedNumbers[PLAYER_COUNT - 1] = (int)TGamePointingResult.Fail;
					Data.SyncGameData();

					// Networking.LocalPlayer.CombatSetCurrentHitpoints(0);

					// 240126 Order로 보내야 함
					int pointerOrder = 0;
					for (int i = 0; i < mTargets.Length; i++)
					{
						// if (IsLocalPlayerIndex(i))
						if (IsLocalPlayerOrder(i))
						{
							pointerOrder = i;
							break;
						}
					}

					SetOwner();
					SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Exit) + pointerOrder);
					SendCustomNetworkEvent(NetworkEventTarget.All, nameof(HideGun));
					SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPointingTime));
				}
			}
			// 스태프가 쏨
			else
			{
				// 오너만
				if (IsOwner() == false)
					return;

				// 엔딩
				if (IsEnding)
					MDebugLog($"{nameof(ShootPlayer)}, Staff Ending");
				// 심판 단계
				else if (CurRoundData.CurState == TGameRoundState.Judge)
					MDebugLog($"{nameof(ShootPlayer)}, Staff Judge");
				else
					MDebugLog($"{nameof(ShootPlayer)}, Staff Invalid ??? ???");

				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Exit) + seatIndex);
			}
		}

		// 지목 중도 포기
		public void StopPointing()
		{
			MDebugLog(nameof(StopPointing));
			CurRoundData.ShootedNumbers[PLAYER_COUNT - 1] = (int)TGamePointingResult.Success;
			Data.SyncGameData();

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(HideGun));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPointingTime));
		}

		public void StartJudge()
		{
			MDebugLog(nameof(StartJudge));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(InitGunPos));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SitSeatOnlyPicker));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(InfoPopup_Jugde));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPointingTime));
		}

		public void SitSeatOnlyPicker()
		{
			MDebugLog(nameof(SitSeatOnlyPicker));
			foreach (ThiefSeat thiefSeat in thiefSeats)
			{
				thiefSeat.SetShootingTargetActive(true);
				thiefSeat.SetMemoActive(false);
			}

			bool isLocalPlayerMinThief = IsLocalPlayerNumber(CurRoundData.PointerNumber);

			for (int i = 0; i < mTargets.Length; i++)
			{
				// if (IsLocalPlayerIndex(i))
				if (IsLocalPlayerOrder(i))
				{
					if (isLocalPlayerMinThief)
					{
						int shootedPlayerCount = 0;
						for (int j = 0; j < PLAYER_COUNT - 1; j++)
						{
							if (CurRoundData.ShootedNumbers[j] != NONE_INT)
								shootedPlayerCount++;
						}

						if ((shootedPlayerCount == PLAYER_COUNT - 1) ||
							(CurRoundData.ShootedNumbers[PLAYER_COUNT - 2] == NONE_INT * 2))
						{
							thiefSeats[i].UseStation();
							thiefSeats[i].SetMemoActive(true);
						}
					}

					return;
				}
			}

			// TP(lowerStaffPos);
		}

		public void StopJudge()
		{
			MDebugLog(nameof(StopJudge));
			CurRoundData.isRoundEnd = true;
			Data.SyncGameData();

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(RoundEnd));
		}

		public void RoundEnd()
		{
			MDebugLog(nameof(RoundEnd));

			SFXManager.PlaySFX_L(3);
			HideGun();
			ResetPointingTime();

			// 지목이 마무리된다면, 전광판 상에서 결과가 표시된 다음, 다음라운드가 시작된다. 

			// OLD: 240106.더이상 마지막 순서 플레이어가 다음 라운드 순서를 정하지 않음.
			// if (IsLastPlayer) TP_StagePos();

			ExitAll();
		}

		public void ShowOrder_G()
		{
			MDebugLog(nameof(ShowOrder_G));
			CurRoundData.isRoundEnd = true;
			Data.SyncGameData();

			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ShowOrder));
		}

		public void ShowOrder()
		{
			MDebugLog(nameof(ShowOrder));
			InfoPopup_End();
			HideGun();
			ResetPointingTime();
		}

		public void ExitAll_G()
		{
			MDebugLog(nameof(ExitAll_G));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ExitAll));
		}

		public void ExitAll()
		{
			MDebugLog(nameof(ExitAll));

			foreach (ThiefSeat thiefSeat in thiefSeats)
			{
				thiefSeat.SetMemoActive(false);
				thiefSeat.SetShootingTargetActive(false);

				// 그냥 전부 Exit 해도 되네?
				thiefSeat.ExitStation();
			}

			// if (IsEnding == false)
			// {
			// 	for (int order = 0; order < NumberByRanking.Length; order++)
			// 	{
			// 		if (IsLocalPlayerNumber(CurRoundData.NumberByOrder[order]))
			// 		{
			// 			thiefSeats[order].ExitStation();
			// 			return;
			// 		}
			// 	}
			// }
			// else
			// {
			// 	// ERROR : 지목 단계 이후 안내려진다
			// 	for (int ranking = 0; ranking < NumberByRanking.Length; ranking++)
			// 	{
			// 		if (IsLocalPlayerNumber(NumberByRanking[ranking]))
			// 		{
			// 			thiefSeats[SeatIndexByRanking[ranking]].ExitStation();
			// 			return;
			// 		}
			// 	}
			// }
		}

		public void Ending_G()
		{
			MDebugLog(nameof(Ending_G));

			if (IsGaming)
			{
				CurRoundData.isRoundEnd = true;
				Data.SyncGameData();
			}
			
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Ending));
		}

		public void Ending()
		{
			MDebugLog(nameof(Ending));

			InitGunPos();
			videoScreenActive.SetValue(true);
			unityVideoPlayer.Play();
			bgmAudiosource.SetActive(false);
			gun.CanInteract = IsStaff(Networking.LocalPlayer);
		}

		public void EndingSeat_G()
		{
			MDebugLog(nameof(EndingSeat_G));
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(EndingSeat));
		}

		public void EndingSeat()
		{
			MDebugLog(nameof(EndingSeat));

			InitGunPos();
			
			foreach (ThiefSeat thiefSeat in thiefSeats)
			{
				thiefSeat.SetShootingTargetActive(true);
				thiefSeat.SetMemoActive(false);
			}

			gun.CanInteract = IsStaff(Networking.LocalPlayer);

			// 1위 ~ 꼴지 순서대로 앉을 자리
			for (int ranking = 0; ranking < NumberByRanking.Length; ranking++)
			{
				if (IsLocalPlayerNumber(NumberByRanking[ranking]))
				{
					thiefSeats[SeatIndexByRanking[ranking]].UseStation();
					thiefSeats[SeatIndexByRanking[ranking]].SetMemoActive(true);
					return;
				}
			}
		}

		public string GetPlayerNameByEndingSeatIndex(int seatIndex)
		{
			// 자리 인덱스를 통해, 엔딩 장면에서 해당 자리에 앉은 플레이어의 이름을 반환
			for (int ranking = 0; ranking < NumberByRanking.Length; ranking++)
			{
				if (SeatIndexByRanking[ranking] == seatIndex)
					return GetPlayerName(NumberByRanking[ranking], false);
			}
			return string.Empty;
		}

		public void ResetPointingTime() => t = 0;

		public void ResetTime()
		{
			// HACK
			pointingTimeSFX.SetActive(false);
			pointingTimeSFX.SetActive(true);
			t = POINTING_TIME_BY_SEC;
		}

		public void Test()
		{
			if (!IsGaming ||
				(int)CurRoundData.CurState > (int)TGameRoundState.Steal6)
				return;

			MDebugLog(nameof(Test));

			CurRoundData.CoinByOrder[0] = 20;
			CurRoundData.CoinByOrder[1] = 20;
			CurRoundData.CoinByOrder[2] = 19;
			CurRoundData.CoinByOrder[3] = 18;
			CurRoundData.CoinByOrder[4] = 0;
			CurRoundData.CoinByOrder[5] = 0;
			CurRoundData.CoinByOrder[6] = 0;
			Data.SyncGameData();
		}

		public void LoadVideo_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(LoadVideo));

		public void LoadVideo()
		{
			MDebugLog(nameof(LoadVideo));
			unityVideoPlayer.LoadURL(endingVideoURL);
		}

		public void JailSFX() => PlaySFX_Local(4);
		public void PlaySFX_Local5() => PlaySFX_Local(5);
		public void CoinSFX() => PlaySFX_Local(7);
		public void PlaySFX_Local(int index) => SFXManager.PlaySFX_L(index);

		#region Horrible Events
		public void InfoPopup_Jugde() => InfoPopup(TGameRoundState.Judge, 2);
		public void InfoPopup_End() => InfoPopup(TGameRoundState.End, 3);
		public void Exit0() => Exit(0);
		public void Exit1() => Exit(1);
		public void Exit2() => Exit(2);
		public void Exit3() => Exit(3);
		public void Exit4() => Exit(4);
		public void Exit5() => Exit(5);
		public void Exit6() => Exit(6);
		public void ShootPlayer0_G() => ShootPlayer_G(0);
		public void ShootPlayer1_G() => ShootPlayer_G(1);
		public void ShootPlayer2_G() => ShootPlayer_G(2);
		public void ShootPlayer3_G() => ShootPlayer_G(3);
		public void ShootPlayer4_G() => ShootPlayer_G(4);
		public void ShootPlayer5_G() => ShootPlayer_G(5);
		public void ShootPlayer6_G() => ShootPlayer_G(6);
		public void ShootPlayer0() => ShootPlayer(0);
		public void ShootPlayer1() => ShootPlayer(1);
		public void ShootPlayer2() => ShootPlayer(2);
		public void ShootPlayer3() => ShootPlayer(3);
		public void ShootPlayer4() => ShootPlayer(4);
		public void ShootPlayer5() => ShootPlayer(5);
		public void ShootPlayer6() => ShootPlayer(6);
		#endregion
	}
}