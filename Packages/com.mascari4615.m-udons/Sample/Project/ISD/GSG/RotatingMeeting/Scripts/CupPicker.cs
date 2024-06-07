using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CupPicker : MBase
	{
		[Header("_" + nameof(CupPicker))]
		[SerializeField]
		private int index;

		[UdonSynced, FieldChangeCallback(nameof(OwnerID))]
		private int _ownerID = NONE_INT;

		public int OwnerID
		{
			get => _ownerID;
			set
			{
				_ownerID = value;
				voteUI.SetActive(false);
				canvas.SetActive(_ownerID == Networking.LocalPlayer.playerId);
			}
		}

		[UdonSynced] private int targetCupIndex = NONE_INT;

		[UdonSynced, FieldChangeCallback(nameof(MeetingStartTime))]
		private int _meetingMeetingStartTime = NONE_INT;

		[SerializeField] private RotatingMeetingManager manager;
		[SerializeField] private TextMeshProUGUI totalTimeText;
		[SerializeField] private int targetTotalTime;
		[SerializeField] private GameObject pickButton;
		[SerializeField] private GameObject returnButton;
		[SerializeField] private Transform meetingPos;
		[SerializeField] private VRCStation pickerStation;
		[SerializeField] private SyncedStation tempStation;
		[SerializeField] private Animator tableAnimator;
		[SerializeField] private MSFXManager sfxManager;
		[SerializeField] private TextMeshProUGUI[] buttonTexts;
		[SerializeField] private GameObject canvas;
		[SerializeField] private GameObject voteUI;
		public TextMeshProUGUI voteTargetNameText;
		private bool isCooltime = false;
		private bool voted;

		public Transform MeetingPos => meetingPos;
		public bool CupHaveToReturn => MeetingStartTime == NONE_INT;
		public int TargetCupIndex => targetCupIndex;
		public Cup TargetCup => manager.Cups[targetCupIndex];

		private int MeetingStartTime
		{
			get => _meetingMeetingStartTime;
			set
			{
				_meetingMeetingStartTime = value;
				OnStartTimeChange();
			}
		}

		private void OnStartTimeChange()
		{
			pickButton.SetActive(MeetingStartTime == NONE_INT);
			returnButton.SetActive(MeetingStartTime != NONE_INT);
		}

		private int TotalMeetingTime()
		{
			int pastMeetingTime = TargetCup.GetTotalMeetingTime(index);
			int curMeetingTime = ((Networking.GetServerTimeInMilliseconds() - MeetingStartTime) / 1000);
			int totalMeetingTime = pastMeetingTime + curMeetingTime;

			return totalMeetingTime;
		}

		private void Start()
		{
			canvas.SetActive(OwnerID == Networking.LocalPlayer.playerId);
			OnStartTimeChange();
		}

		private void Update() => CheckTime();

		private void CheckTime()
		{
			if (MeetingStartTime == NONE_INT)
			{
				totalTimeText.text = "-";
				totalTimeText.color = Color.white;
			}
			else
			{
				// MDebugLog(nameof(CheckTime) + TotalMeetingTime());

				int totalMeetingTime = TotalMeetingTime();
				totalTimeText.text = totalMeetingTime <= targetTotalTime ? TimeSpan.FromSeconds(totalMeetingTime).ToString(@"mm\:ss") : "-";
				totalTimeText.color = Color.Lerp(Color.red, Color.green, (float)totalMeetingTime / targetTotalTime);

				if (totalMeetingTime > targetTotalTime)
					Success();
			}
		}

		public void PickNearestCup()
		{
			if (manager.IsStop.Value)
				return;

			if (MeetingStartTime != NONE_INT)
				return;

			// 아직 레일로 돌아가지 못한 컵이 있다면 Return
			foreach (var cup in manager.Cups)
			{
				int cupPos = (int)cup.TargetPos;
				if (cupPos == (int)CupPosition.Meeting0 + index)
					return;
			}

			// 범위 내 플랫폼이 없다면 Return
			if (!manager.TryGetNearestPlatform_Meeting(out var platformPos, index))
			{
				// DEBUG_Text.text = "_No Platform";
				MDebugLog(nameof(PickNearestCup) + "_No Platform");
				return;
			}

			// if (nearestSushi.OwnerID != NONE_INT)

			// 플랫폼에 컵이 있다면
			for (int i = 0; i < manager.Cups.Length; i++)
			{
				Cup cup = manager.Cups[i];
				if (cup.TargetPos != platformPos)
					continue;

				if (cup.turn >= 20)
					continue;

				if (isCooltime) return;
				else StartCoolTime();

				StartMeeting(i);
				return;
			}

			// DEBUG_Text.text = "_No Cup";
			MDebugLog(nameof(PickNearestCup) + "_No Cup");
		}

		private void StartMeeting(int _targetCupIndex)
		{
			MDebugLog(nameof(StartMeeting) + ((int)CupPosition.Meeting0 + index));
			// DEBUG_Text.text = "StartMeeting";

			SetOwner();
			targetCupIndex = _targetCupIndex;
			MeetingStartTime = Networking.GetServerTimeInMilliseconds();
			RequestSerialization();

			// 컵을 당겨옴
			TargetCup.SetTargetPos((CupPosition)((int)CupPosition.Meeting0 + index));
		}

		private void Success()
		{
			foreach (var cup in manager.Cups)
			{
				if ((int)cup.TargetPos == (int)CupPosition.Meeting0 + index)
					if (cup.OwnerID == Networking.LocalPlayer.playerId)
					{
						MDebugLog(nameof(Success));

						TargetCup.SetTotalMeetingTime(0, index);
						TargetCup.SetTurn(100);

						cup.ExitCup();
						// TODO : 조금의 딜레이?
						tempStation.UseStation();

						SetOwner();
						targetCupIndex = NONE_INT;
						MeetingStartTime = NONE_INT;
						RequestSerialization();

						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TriggerAnimator));
						break;
					}
			}
		}

		public void TriggerAnimator()
		{
			// MDebugLog(nameof(TriggerAnimator));
			tableAnimator.SetTrigger("GO");
			sfxManager.PlaySFX_G(0);
			sfxManager.PlaySFX_G(2);
		}

		public void ExitOwners()
		{
			manager.TriggerBoomAnimation();
			sfxManager.PlaySFX_G(1);

			if (OwnerID == Networking.LocalPlayer.playerId)
				ExitCup();

			if (tempStation.OwnerID == Networking.LocalPlayer.playerId)
				tempStation.ExitStation();
		}

		public void SitCup()
		{
			// MDebugLog(nameof(SitCup));
			if (OwnerID != NONE_INT)
				return;

			SetOwner();
			OwnerID = Networking.LocalPlayer.playerId;
			RequestSerialization();
		}

		public void ExitCup()
		{
			// MDebugLog(nameof(ExitCup));
			if (OwnerID != Networking.LocalPlayer.playerId)
				return;

			if (voteUI.activeSelf)
			{
				VoteNo();
			}

			pickerStation.ExitStation(Networking.LocalPlayer);

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();
		}

		public void ReturnCup()
		{
			if (isCooltime) return;
			else StartCoolTime();

			if (OwnerID != Networking.LocalPlayer.playerId)
				return;

			// MDebugLog(nameof(ReturnCup));

			// 시간이 끝나고 컵이 돌아감
			// 시간을 컵에 기록

			TargetCup.SetTotalMeetingTime(TotalMeetingTime(), index);

			SetOwner();
			targetCupIndex = NONE_INT;
			MeetingStartTime = NONE_INT;
			RequestSerialization();
		}

		private void StartCoolTime()
		{
			isCooltime = true;
			foreach (var buttonText in buttonTexts)
				buttonText.color = Color.gray;

			SendCustomEventDelayedSeconds(nameof(EndCoolTime), 2f);
		}

		public void EndCoolTime()
		{
			// if (manager.IsStop)
			// {
			//     SendCustomEventDelayedSeconds(nameof(EndCoolTime), 1.5f);
			//     return;
			// }

			isCooltime = false;
			foreach (var buttonText in buttonTexts)
				buttonText.color = Color.white;
		}

		public override void OnPlayerRespawn(VRCPlayerApi player)
		{
			if (!player.isLocal)
				return;

			if (player.playerId == OwnerID)
			{
				SetOwner();
				OwnerID = NONE_INT;
				RequestSerialization();
			}
		}

		[SerializeField] private GameObject[] hiders;

		public void OpenVote()
		{
			voted = false;
			voteUI.SetActive(true);
			hiders[0].SetActive(false);
			hiders[1].SetActive(false);
		}

		public void CloseVote()
		{
			voted = false;
			voteUI.SetActive(false);
			hiders[0].SetActive(false);
			hiders[1].SetActive(false);
		}

		public void VoteYes_Global()
		{
			if (OwnerID != Networking.LocalPlayer.playerId)
				return;
			if (voted)
				return;
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(VoteYes));
		}

		public void VoteYes()
		{
			voted = true;
			hiders[0].SetActive(true);
			manager.Vote(true);
		}

		public void VoteNo_Global()
		{
			if (OwnerID != Networking.LocalPlayer.playerId)
				return;
			if (voted)
				return;
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(VoteNo));
		}

		public void VoteNo()
		{
			voted = true;
			manager.Vote(false);
			hiders[1].SetActive(true);
		}
	}
}