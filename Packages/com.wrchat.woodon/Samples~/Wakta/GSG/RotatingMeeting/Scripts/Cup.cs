using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class Cup : MBase
	{
		[Header("_" + nameof(Cup))]
		[SerializeField] private RotatingMeetingManager manager;

		private DollyCartSync targetDollyCart;
		private Transform targetPosTransform;

		public int OwnerID
		{
			get => _ownerID;
			set
			{
				_ownerID = value;
				VRCPlayerApi OwnerPlayer = VRCPlayerApi.GetPlayerById(_ownerID);
				ownerText.text = $"{_ownerID} : {(OwnerPlayer != null ? OwnerPlayer.displayName : "NONE")}";
				usedAppealTime = false;
				canvas.SetActive(_ownerID == Networking.LocalPlayer.playerId);
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(OwnerID))] private int _ownerID = NONE_INT;

		[SerializeField] private TextMeshProUGUI ownerText;

		[SerializeField] private int index;
		[SerializeField] private GameObject canvas;

		[UdonSynced] public int turn = 0;
		[UdonSynced] public int totalMeetingTime0 = 0;
		[UdonSynced] public int totalMeetingTime1 = 0;
		[UdonSynced] public int totalMeetingTime2 = 0;

		[SerializeField] private float lerpSpeed = 1;
		[SerializeField] private TextMeshProUGUI debugText;
		[SerializeField] private VRCStation _station;
		[SerializeField] private GameObject appealTimeButton;
		[SerializeField] private Animator appealTimeAnimator;
		private bool usedAppealTime = false;

		/// <summary>
		/// Lerp 목표로 하는 위치 데이터
		/// </summary>
		public CupPosition TargetPos
		{
			get => targetPos;
			set
			{
				targetPos = value;
				OnTargetPosChange();
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(TargetPos))] private CupPosition targetPos = CupPosition.None;


		/// <summary>
		/// Lerp 목표로 하는 위치 Transform 설정
		/// </summary>
		private void OnTargetPosChange()
		{
			if (targetPos != CupPosition.None)
			{
				targetPosTransform = manager.GetTargetPosTransform(targetPos);
				targetDollyCart = manager.GetDollyCart(targetPos);
			}
		}

		private void Start()
		{
			OnTargetPosChange();
			canvas.SetActive(OwnerID == Networking.LocalPlayer.playerId);
		}

		private void Update()
		{
			UpdateTransform();
			TryReturnToPlatform();
			UpdateTurn();
			UpdateAppealTimeButton();
		}

		private void UpdateAppealTimeButton()
		{
			bool isInRail = ((int)TargetPos <= (int)CupPosition.Platform7);
			bool canUseAppealTime = manager.CanSetAmplification();

			appealTimeButton.SetActive(isInRail && canUseAppealTime && !usedAppealTime);
		}

		public void SetTurn(int newValue)
		{
			// MDebugLog(nameof(SetTurn) + newValue);
			SetOwner();
			turn = newValue;
			RequestSerialization();
		}

		private void UpdateTurn()
		{
			if ((int)TargetPos > (int)CupPosition.Platform7)
				return;

			bool turnIsOdd = turn % 2 != 0;

			if (turnIsOdd)
			{
				if (targetDollyCart.Position > 8)
				{
					if (!IsOwner(manager.gameObject))
						return;

					SetTurn(turn + 1);
				}
			}
			else
			{
				if (targetDollyCart.Position < 8)
				{
					if (!IsOwner(manager.gameObject))
						return;

					SetTurn(turn + 1);
				}
			}

			if (turn >= 10 * 2)
			{
				// MDebugLog((targetDollyCart.Position > 11).ToString());

				if (targetDollyCart.Position > 11.5f)
				{
					if (!IsOwner(manager.gameObject))
						return;

					SetOwner();
					turn = 0;
					totalMeetingTime0 = 0;
					totalMeetingTime1 = 0;
					totalMeetingTime2 = 0;
					RequestSerialization();
					manager.UploadCup(this);
				}
			}

			if (turn >= 100)
				debugText.text = $"탈락";
			else
				debugText.text = $"{(turn + 1) / 2} / 10 바퀴 째";
		}

		/// <summary>
		/// 위치 Lerp 업데이트 (로컬, 데이터만 싱크됨)
		/// </summary>
		private void UpdateTransform()
		{
			if (!targetPosTransform)
				return;

			transform.position =
				Vector3.Lerp(transform.position, targetPosTransform.position, lerpSpeed * Time.deltaTime);
			transform.rotation =
				Quaternion.Lerp(transform.rotation, targetPosTransform.rotation, lerpSpeed * Time.deltaTime);
		}

		private void TryReturnToPlatform()
		{
			debugText.text = "-";

			// 오너가 아니거나, TargetPos가 초기화되지 않았다면 Return
			if (!IsOwner(manager.gameObject) || (TargetPos == CupPosition.None))
				return;

			// 미팅 중이 아니거나, 진입 대기 중이 아니라면 
			if ((int)TargetPos <= (int)CupPosition.Platform7)
			{
				// debugText.text = "_Not Meeting";
				// MDebugLog(nameof(TryReturnToPlatform) + "_Not Meeting");
				return;
			}

			// 미팅 중이었다면
			if ((int)CupPosition.Meeting0 <= (int)TargetPos && (int)TargetPos <= (int)CupPosition.Meeting2)
			{
				int sushiPickerIndex = (int)TargetPos - (int)(CupPosition.Meeting0);

				// 대기 상태가 아니면 Return
				if (!manager.CupPickers[sushiPickerIndex].CupHaveToReturn)
				{
					// debugText.text = "_No State";
					// MDebugLog(nameof(TryReturnToPlatform) + "_No State");
					return;
				}

				// 판정 범위 내에 플랫폼이 없다면 Return
				if (!manager.TryGetNearestPlatform_Meeting(out CupPosition platformIndex, sushiPickerIndex))
				{
					// debugText.text = "_No Platform";
					// MDebugLog(nameof(TryReturnToPlatform) + "_No Platform");
					return;
				}

				// 플랫폼에 카트가 이미 있다면 Return
				foreach (Cup cup in manager.Cups)
				{
					if (cup.TargetPos == platformIndex)
					{
						// debugText.text = "_Already Cart";
						// MDebugLog(nameof(TryReturnToPlatform) + "_Already Cart");
						return;
					}
				}

				// MDebugLog(nameof(TryReturnToPlatform) + "_ReturnA");
				SetTargetPos(platformIndex);
			}
			// 진입 대기 중이었다면
			else if ((int)CupPosition.Down0 <= (int)TargetPos && (int)TargetPos <= (int)CupPosition.Down3)
			{
				// 대기 중인 컵이라면
				if (!manager.CanDownload(TargetPos))
				{
					// debugText.text = "_Cant Download";
					// MDebugLog(nameof(TryReturnToPlatform) + "_No Platform");
					return;
				}

				// TODO : '정말' 대기 중인지 확인 (아직 내려가고 있는 중이라던지, 올라가고 있는 중이라던지)
				// TODO : 한 번에 한 컵만 업로드 가능하게 (본 레일 진입 시 대기 시간 가능성)

				// 판정 범위 내에 플랫폼이 없다면 Return
				if (!manager.TryGetNearestPlatform_Download(out CupPosition platformIndex))
				{
					// debugText.text = "_No Platform";
					// MDebugLog(nameof(TryReturnToPlatform) + "_No Platform");
					return;
				}

				// 플랫폼에 카트가 이미 있다면 Return
				foreach (Cup sushiCart in manager.Cups)
				{
					if (sushiCart.TargetPos == platformIndex)
					{
						// debugText.text = "_Already Cart";
						// MDebugLog(nameof(TryReturnToPlatform) + "_Already Cart");
						return;
					}
				}

				// MDebugLog(nameof(TryReturnToPlatform) + "_ReturnB");
				SetTargetPos(platformIndex);
			}
		}

		public void SetTargetPos(CupPosition targetPos)
		{
			SetOwner();
			TargetPos = targetPos;
			RequestSerialization();
		}

		public void SetTotalMeetingTime(int time, int index)
		{
			SetOwner();
			switch (index)
			{
				case 0:
					totalMeetingTime0 = time;
					break;
				case 1:
					totalMeetingTime1 = time;
					break;
				case 2:
					totalMeetingTime2 = time;
					break;
			}

			RequestSerialization();
		}

		public int GetTotalMeetingTime(int index)
		{
			switch (index)
			{
				case 0:
					return totalMeetingTime0;
				case 1:
					return totalMeetingTime1;
				case 2:
					return totalMeetingTime2;
				default:
					return NONE_INT;
			}
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

			_station.ExitStation(Networking.LocalPlayer);

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();
		}

		public void ForceReset()
		{
			_station.ExitStation(Networking.LocalPlayer);

			SetOwner();
			OwnerID = NONE_INT;
			RequestSerialization();
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

		public void AppealTime()
		{
			bool canApealTime = manager.CanSetAmplification();
			MDebugLog(nameof(AppealTime) + canApealTime);
			if (canApealTime == false)
				return;

			usedAppealTime = true;
			manager.IsRailStop.SetValue(true);
			manager.SetVoiceAmplification();
			manager.OpenVote_Global(index);
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TriggerAppealTimeAnimator));
		}

		public void TriggerAppealTimeAnimator() => appealTimeAnimator.SetTrigger("POP");
	}
}