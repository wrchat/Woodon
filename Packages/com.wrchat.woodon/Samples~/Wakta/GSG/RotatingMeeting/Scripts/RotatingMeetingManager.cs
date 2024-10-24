using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using WRC.Woodon;

namespace Mascari4615.Project.ISD.GSG.RotatingMeeting
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class RotatingMeetingManager : MBase
	{
		[field: Header("_" + nameof(RotatingMeetingManager))]

		[field: SerializeField] public Cup[] Cups { get; private set; }
		[field: SerializeField] public CupPicker[] CupPickers{ get; private set; }

		// 현재 시간 (컵들 위치 계산용)
		[UdonSynced(UdonSyncMode.Smooth)] private float curTime;

		/// <summary>
		/// Loop 한 칸 사이 이동하는 데 걸리는 시간
		/// </summary>
		public const int TIME_FROM_POINT_TO_POINT_BY_MILLI = 2000;

		/// <summary>
		/// Loop 에서 카트를 배치하는 인덱스 배수
		/// </summary>
		private const int POINT_TO_POINT_OFFSET = 2;
		// private readonly int startTimeByMilli = 0;

		/// <summary>
		/// Pick 할 때 Offset
		/// </summary>
		private const float POS_RANGE_OFFSET = .5f;

		[SerializeField] private DollyCartSync[] runningCupPlatforms;
		[SerializeField] private DollyCartSync[] waitingCupPlatforms;
		[SerializeField] private WaitingData[] waitingDatas;
		[SerializeField] private MValue curSpeedFactor;
		[SerializeField] private MSFXManager sfxManager;

		[field: SerializeField] public MBool IsRailStop { get; private set; }

		[SerializeField] private Animator flyingCupDestinationAnimator;

		/// <summary>
		/// 다용도 MTarget
		/// </summary>
		[SerializeField] private MTarget mTarget;

		[SerializeField] private TextMeshProUGUI curOwnerText;

		[SerializeField] private VoiceAmplification_MTarget voiceAmplification;

		/// <summary>
		/// Enum CupPosition 값에 따라 실제 위치 Transform 반환 (컵 위치 Lerp하는 TargetPos 설정용)
		/// </summary>
		public Transform GetTargetPosTransform(CupPosition cupPosition)
		{
			// MDebugLog(nameof(GetTargetPosTransform) + cupPosition);

			if (cupPosition == CupPosition.None)
				return null;

			if ((int)cupPosition <= (int)CupPosition.Platform7)
				return runningCupPlatforms[(int)cupPosition].transform;
			else if ((int)cupPosition <= (int)CupPosition.Meeting2)
				return CupPickers[(int)cupPosition - (int)CupPosition.Meeting0].MeetingPos;
			else
				return waitingCupPlatforms[(int)cupPosition - (int)CupPosition.Down0].transform;
		}

		/// <summary>
		/// Enum CupPosition 값에 따라 실제 위치 DollyCartSync 반환 (컵 위치에 따른 Turn 체크용)
		/// </summary>
		public DollyCartSync GetDollyCart(CupPosition cupPosition)
		{
			// MDebugLog(nameof(GetDollyCart) + cupPosition);

			if ((cupPosition != CupPosition.None) && ((int)cupPosition <= (int)CupPosition.Platform7))
				return runningCupPlatforms[(int)cupPosition];
			else return null;
		}

		private void Start()
		{
			if (Networking.IsMaster == false)
				return;

			// for (var i = 0; i < Cups.Length; i++)
			//     Cups[i].SetTargetPos((CupPosition)(i * 2));

			for (int i = 0; i < Cups.Length; i++)
			{
				Cups[i].SetTargetPos((CupPosition)((int)CupPosition.Down0 + i));
				waitingDatas[i].SetUpload(false);
				waitingDatas[i].SetCurTime(13 * 2000);
			}
		}

		private void Update()
		{
			UpdateRunningCupPos();
			UpdateWaitungCupPos();

			string voteTargetName = string.Empty;

			if (voiceAmplification.TargetPlayers[0].TargetPlayerID != NONE_INT)
			{
				VRCPlayerApi voteTarget =
					VRCPlayerApi.GetPlayerById(voiceAmplification.TargetPlayers[0].TargetPlayerID);

				if (voteTarget != null)
					voteTargetName = voteTarget.displayName;
			}

			foreach (CupPicker cupPicker in CupPickers)
				cupPicker.VoteTargetNameText.text = voteTargetName;

			VRCPlayerApi curOwner = Networking.GetOwner(gameObject);
			curOwnerText.text = curOwner.playerId + curOwner.displayName;
		}

		private void UpdateRunningCupPos()
		{
			if (IsOwner() && (IsRailStop.Value == false))
				curTime += curSpeedFactor.Value * Time.deltaTime * 1000;

			// var curTimeByMilli = Networking.GetServerTimeInMilliseconds();
			// var timeDiffByMilli = (curTimeByMilli - startTimeByMilli) % (timeFromPointToPointByMilli * 16);
			float timeDiffByMilli = curTime % (TIME_FROM_POINT_TO_POINT_BY_MILLI * 16);
			// (timeFromPointToPointByMilli * SushiCarts.Length * 2);
			float posByTime = (float)timeDiffByMilli / TIME_FROM_POINT_TO_POINT_BY_MILLI;

			// MDebugLog(posByTime.ToString());
			// debugText.text = posByTime.ToString();

			for (int i = 0; i < runningCupPlatforms.Length; i++)
				runningCupPlatforms[i].SetPosition((POINT_TO_POINT_OFFSET * i + posByTime) % 16);
		}

		/// <summary>
		/// 컵 아래쪽으로 보내기 (커플 성사 시 or Turn 충족하여 탈락 시)
		/// </summary>
		/// <param name="targetCup"></param>
		public void UploadCup(Cup targetCup)
		{
			MDebugLog(nameof(UploadCup) + targetCup.gameObject.name);
			CupPosition targetPos = CupPosition.None;

			// 루프 수
			for (int i = 0; i < Cups.Length; i++)
			{
				// 먼저 선점된 루프가 있는 지 확인
				bool already = false;

				foreach (Cup cup in Cups)
				{
					if ((int)cup.TargetPos == (int)CupPosition.Down0 + i)
					{
						already = true;
						break;
					}
				}

				if (!already)
				{
					targetPos = (CupPosition)((int)CupPosition.Down0 + i);
					break;
				}
			}

			if (targetPos == CupPosition.None)
				MDebugLog("Something Wrong !!!!!!!!!!!!!!!!!!!!!!!");

			MDebugLog(nameof(UploadCup) + $" - Move to {targetPos}");

			targetCup.SetTargetPos(targetPos);
			waitingDatas[(int)targetPos - (int)CupPosition.Down0].SetCurTime(0);
			waitingDatas[(int)targetPos - (int)CupPosition.Down0].SetUpload(false);
		}

		private void UpdateWaitungCupPos()
		{
			if (IsOwner() && (IsRailStop.Value == false))
			{
				foreach (Cup cup in Cups)
				{
					if (cup.TargetPos == CupPosition.None)
						continue;

					if ((int)cup.TargetPos >= (int)CupPosition.Down0)
						waitingDatas[(int)cup.TargetPos - (int)CupPosition.Down0]
							.AddCurTime(curSpeedFactor.Value * Time.deltaTime * 1000);
				}

				for (int i = 0; i < waitingCupPlatforms.Length; i++)
				{
					float pos = Mathf.Clamp((float)waitingDatas[i].CurTime, 0, (TIME_FROM_POINT_TO_POINT_BY_MILLI * 26)) /
								TIME_FROM_POINT_TO_POINT_BY_MILLI;
					waitingCupPlatforms[i].SetPosition(pos);
				}
			}
		}

		public bool CanDownload(CupPosition cupPosition)
		{
			int index = (int)cupPosition - (int)CupPosition.Down0;
			return waitingCupPlatforms[index].Position >= 25.5f;
		}

		private void UploadCup(int index)
		{
			if (IsOwner())
				waitingDatas[index].SetUpload(true);
		}

		#region HorribleEvents
		public void UploadCup0() => UploadCup(0);
		public void UploadCup1() => UploadCup(1);
		public void UploadCup2() => UploadCup(2);
		public void UploadCup3() => UploadCup(3);
		#endregion

		public bool TryGetNearestPlatform_Download(out CupPosition platformPos)
		{
			// float posRangeMin = 15 + posRangeOffset;
			float posRangeMin = 0f;
			float posRangeMax = .8f;

			return TryGetNearestPlatform(out platformPos, posRangeMin, posRangeMax);
		}

		public bool TryGetNearestPlatform_Meeting(out CupPosition platformPos, int sushiPickerIndex)
		{
			float posRangeMin = 1 + sushiPickerIndex * 4 + POS_RANGE_OFFSET;
			float posRangeMax = 3 + sushiPickerIndex * 4 - POS_RANGE_OFFSET;

			return TryGetNearestPlatform(out platformPos, posRangeMin, posRangeMax);
		}

		private bool TryGetNearestPlatform(out CupPosition platformPos, float min, float max)
		{
			for (int index = 0; index < runningCupPlatforms.Length; index++)
				if (IsInRange(runningCupPlatforms[index], min, max))
				{
					platformPos = (CupPosition)index;
					return true;
				}

			platformPos = CupPosition.None;
			return false;
		}

		private bool IsInRange(DollyCartSync target, float posMin, float posMax)
		{
			float pos = target.Position;
			bool isInRange;
			isInRange = (posMin <= pos) && (pos <= posMax);
			return isInRange;
		}

		public void TriggerBoomAnimation() => flyingCupDestinationAnimator.SetTrigger("BOOM");

		public void SetOwnerCurMTarget_G() =>
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetOwnerCurMTarget));
		public void SetOwnerCurMTarget()
		{
			if (mTarget.IsTargetPlayer())
			{
				SetOwner();

				foreach (WaitingData waitingData in waitingDatas)
					SetOwner(waitingData.gameObject);
			}
		}

		public bool CanSetAmplification() => voiceAmplification.TargetPlayers[0].TargetPlayerID == NONE_INT;

		public void SetVoiceAmplification()
		{
			voiceAmplification.SetPlayer(Networking.LocalPlayer.playerId);
			voiceAmplification.SetEnable(true);
		}

		private int voteTargetCupIndex = 0;
		private int voteTotalCount = 3;
		private int voteCurCount = 0;
		private int voteScore = 0;

		public void OpenVote_Global(int index) =>
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OpenVote) + index);

		public void OpenVote0() => OpenVote(0);
		public void OpenVote1() => OpenVote(1);
		public void OpenVote2() => OpenVote(2);
		public void OpenVote3() => OpenVote(4);

		public void OpenVote(int index)
		{
			MDebugLog(nameof(OpenVote));

			sfxManager.PlaySFX_G(3);
			voteTargetCupIndex = index;
			voteCurCount = 0;
			voteTotalCount = 0;
			foreach (CupPicker cupPicker in CupPickers)
				if (cupPicker.OwnerID != NONE_INT)
					voteTotalCount++;

			foreach (CupPicker cupPicker in CupPickers)
				cupPicker.OpenVote();
		}

		public void CloseVote()
		{
			MDebugLog(nameof(CloseVote));
			foreach (CupPicker cupPicker in CupPickers)
				cupPicker.CloseVote();
		}

		public void Vote(bool yes)
		{
			if (CanSetAmplification())
				return;

			MDebugLog($"yes = {yes}");
			voteCurCount++;
			voteScore += yes ? 1 : 0;

			if (IsOwner() == false)
				return;

			if (voteCurCount >= voteTotalCount)
			{
				if (voteScore > 0)
				{
					sfxManager.PlaySFX_G(5);
					ResetAppealTime();
					MDebugLog("Success");
				}
				else
				{
					Cups[voteTargetCupIndex].SetTurn(100);

					sfxManager.PlaySFX_G(4);
					ResetAppealTime();
					MDebugLog("Fail");
				}
			}
		}

		public void ResetAppealTime()
		{
			voiceAmplification.SetPlayer(NONE_INT);
			voiceAmplification.SetEnable(false);
			IsRailStop.SetValue(false);
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CloseVote));
		}
	}
}