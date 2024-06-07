using System;
using Cinemachine;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public enum CupPosition
	{
		Platform0,
		Platform1,
		Platform2,
		Platform3,
		Platform4,
		Platform5,
		Platform6,
		Platform7,
		Meeting0,
		Meeting1,
		Meeting2,
		Down0,
		Down1,
		Down2,
		Down3,
		None
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class RotatingMeetingManager : MBase
	{
		/// <summary>
		/// 컵들
		/// </summary>
		public Cup[] Cups => cups;

		[SerializeField] private Cup[] cups;

		/// <summary>
		/// 지목자들
		/// </summary>
		public CupPicker[] CupPickers => cupPickers;

		[SerializeField] private CupPicker[] cupPickers;

		/// <summary>
		/// 현재 시간 (컵들 위치 계산용)
		/// </summary>
		[UdonSynced(UdonSyncMode.Smooth)] private float curTime;

		/// <summary>
		/// Loop 한 칸 사이 이동하는 데 걸리는 시간
		/// </summary>
		public const int TimeFromPointToPointByMilli = 2000;

		/// <summary>
		/// Loop 에서 카트를 배치하는 인덱스 배수
		/// </summary>
		private const int PointToPointOffset = 2;
		// private readonly int startTimeByMilli = 0;

		/// <summary>
		/// 위쪽 플랫폼
		/// </summary>
		[SerializeField] private DollyCartSync[] cupPlatforms;

		/// <summary>
		/// 아래쪽 플랫폼
		/// </summary>
		[SerializeField] private DollyCartSync[] waitingPlatforms;

		/// <summary>
		/// 대기 시 사용하는 데이터들 모음
		/// </summary>
		[SerializeField] private WaitingData[] waitingDatas;

		/// <summary>
		/// Pick 할 때 Offset
		/// </summary>
		private const float PosRangeOffset = .5f;

		/// <summary>
		/// 속도 슬라이더
		/// </summary>
		[SerializeField] private SyncedSlider curSpeedFactor;

		/// <summary>
		/// 레일이 멈춰있는가?
		/// </summary>
		[SerializeField] private SyncedBool isStop;

		public SyncedBool IsStop => isStop;

		/// <summary>
		/// 테라스 터지는 애니메이터
		/// </summary>
		[SerializeField] private Animator boomAnimator;

		/// <summary>
		/// 다용도 MTarget
		/// </summary>
		[SerializeField] private MTarget mTarget;

		[SerializeField] private TextMeshProUGUI curOwnerText;

		[SerializeField] private VoiceAmplification_MTarget voiceAmplification;

		/// <summary>
		/// Enum CupPosition 값에 따라 실제 위치 Transform 반환 (컵 위치 Lerp하는 TargetPos 설정용)
		/// </summary>
		/// <param name="cupPosition"></param>
		/// <returns></returns>
		public Transform GetTargetPosTransform(CupPosition cupPosition)
		{
			// MDebugLog(nameof(GetTargetPosTransform) + cupPosition);

			if (cupPosition == CupPosition.None)
				return null;

			if ((int)cupPosition <= (int)CupPosition.Platform7)
				return cupPlatforms[(int)cupPosition].transform;
			else if ((int)cupPosition <= (int)CupPosition.Meeting2)
				return cupPickers[(int)cupPosition - (int)CupPosition.Meeting0].MeetingPos;
			else
				return waitingPlatforms[(int)cupPosition - (int)CupPosition.Down0].transform;
		}

		/// <summary>
		/// Enum CupPosition 값에 따라 실제 위치 DollyCartSync 반환 (컵 위치에 따른 Turn 체크용)
		/// </summary>
		/// <param name="cupPosition"></param>
		/// <returns></returns>
		public DollyCartSync GetDollyCart(CupPosition cupPosition)
		{
			// MDebugLog(nameof(GetDollyCart) + cupPosition);

			if ((cupPosition != CupPosition.None) && ((int)cupPosition <= (int)CupPosition.Platform7))
				return cupPlatforms[(int)cupPosition];
			else return null;
		}

		private void Start()
		{
			if (!Networking.IsMaster)
				return;

			// for (var i = 0; i < Cups.Length; i++)
			//     Cups[i].SetTargetPos((CupPosition)(i * 2));         

			for (var i = 0; i < Cups.Length; i++)
			{
				Cups[i].SetTargetPos((CupPosition)((int)CupPosition.Down0 + i));
				waitingDatas[i].SetGoUp(false);
				waitingDatas[i].SetCurTime(13 * 2000);
			}
		}

		private void Update()
		{
			UpdateMainCupPos();
			UpdateDownCupPos();

			string voteTargetName = string.Empty;

			if (voiceAmplification.MTargets[0].CurTargetPlayerID != NONE_INT)
			{
				VRCPlayerApi voteTarget =
					VRCPlayerApi.GetPlayerById(voiceAmplification.MTargets[0].CurTargetPlayerID);

				if (voteTarget != null)
					voteTargetName = voteTarget.displayName;
			}

			foreach (var cupPicker in cupPickers)
				cupPicker.voteTargetNameText.text = voteTargetName;

			VRCPlayerApi curOwner = Networking.GetOwner(gameObject);
			curOwnerText.text = curOwner.playerId + curOwner.displayName;
		}

		/// <summary>
		/// 위쪽 컵 위치 갱신
		/// </summary>
		private void UpdateMainCupPos()
		{
			if (IsOwner() && (isStop.Value == false))
				curTime += curSpeedFactor.CalcValue * Time.deltaTime * 1000;

			// var curTimeByMilli = Networking.GetServerTimeInMilliseconds();
			// var timeDiffByMilli = (curTimeByMilli - startTimeByMilli) % (timeFromPointToPointByMilli * 16);
			var timeDiffByMilli = curTime % (TimeFromPointToPointByMilli * 16);
			// (timeFromPointToPointByMilli * SushiCarts.Length * 2);
			var posByTime = (float)timeDiffByMilli / TimeFromPointToPointByMilli;

			// MDebugLog(posByTime.ToString());
			// debugText.text = posByTime.ToString();

			for (var i = 0; i < cupPlatforms.Length; i++)
				cupPlatforms[i].SetPosition((PointToPointOffset * i + posByTime) % 16);
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
			for (int i = 0; i < cups.Length; i++)
			{
				// 먼저 선점된 루프가 있는 지 확인
				bool already = false;

				foreach (var cup in Cups)
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
			waitingDatas[(int)targetPos - (int)CupPosition.Down0].SetGoUp(false);
		}

		/// <summary>
		/// 아래쪽 컵 위치 갱신
		/// </summary>
		private void UpdateDownCupPos()
		{
			if (IsOwner() && (isStop.Value == false))
			{
				foreach (var cup in Cups)
				{
					if (cup.TargetPos == CupPosition.None)
						continue;

					if ((int)cup.TargetPos >= (int)(CupPosition.Down0))
						waitingDatas[(int)cup.TargetPos - (int)CupPosition.Down0]
							.AddCurTime(curSpeedFactor.CalcValue * Time.deltaTime * 1000);
				}

				for (int i = 0; i < waitingPlatforms.Length; i++)
				{
					float pos = Mathf.Clamp((float)waitingDatas[i].CurTime, 0, (TimeFromPointToPointByMilli * 26)) /
								TimeFromPointToPointByMilli;
					waitingPlatforms[i].SetPosition(pos);
				}
			}
		}

		public bool CanDownload(CupPosition cupPosition)
		{
			int index = (int)cupPosition - (int)CupPosition.Down0;
			return waitingPlatforms[index].Position >= 25.5f;
		}

		private void GoUp(int index)
		{
			if (IsOwner())
				waitingDatas[index].SetGoUp(true);
		}

		public void GoUp0() => GoUp(0);
		public void GoUp1() => GoUp(1);
		public void GoUp2() => GoUp(2);
		public void GoUp3() => GoUp(3);

		public bool TryGetNearestPlatform_Download(out CupPosition platformPos)
		{
			// float posRangeMin = 15 + posRangeOffset;
			float posRangeMin = 0f;
			float posRangeMax = .8f;

			return TryGetNearestPlatform(out platformPos, posRangeMin, posRangeMax);
		}

		public bool TryGetNearestPlatform_Meeting(out CupPosition platformPos, int sushiPickerIndex)
		{
			float posRangeMin = 1 + sushiPickerIndex * 4 + PosRangeOffset;
			float posRangeMax = 3 + sushiPickerIndex * 4 - PosRangeOffset;

			return TryGetNearestPlatform(out platformPos, posRangeMin, posRangeMax);
		}

		private bool TryGetNearestPlatform(out CupPosition platformPos, float min, float max)
		{
			for (var index = 0; index < cupPlatforms.Length; index++)
				if (IsInRange(cupPlatforms[index], min, max))
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

		public void TriggerBoomAnimation() => boomAnimator.SetTrigger("BOOM");

		public void SetOwnerCurMTarget_Global() =>
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetOwnerCurMTarget));

		public void SetOwnerCurMTarget()
		{
			if (mTarget.IsLocalPlayerTarget)
			{
				SetOwner();

				foreach (var waitingData in waitingDatas)
					SetOwner(waitingData.gameObject);
			}
		}

		public bool CanSetAmplification() => voiceAmplification.MTargets[0].CurTargetPlayerID == NONE_INT;

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
			foreach (var cupPicker in cupPickers)
				if (cupPicker.OwnerID != NONE_INT)
					voteTotalCount++;

			foreach (var cupPicker in cupPickers)
				cupPicker.OpenVote();
		}

		public void CloseVote()
		{
			MDebugLog(nameof(CloseVote));
			foreach (var cupPicker in cupPickers)
				cupPicker.CloseVote();
		}

		[SerializeField] private MSFXManager sfxManager;

		public void Vote(bool yes)
		{
			if (CanSetAmplification())
				return;

			MDebugLog($"yes = {yes}");
			voteCurCount++;
			voteScore += yes ? 1 : 0;

			if (!IsOwner())
				return;

			if (voteCurCount >= voteTotalCount)
			{
				if (voteScore > 0)
				{
					sfxManager.PlaySFX_G(5);
					ResetUlt();
					MDebugLog("Success");
				}
				else
				{
					Cups[voteTargetCupIndex].SetTurn(100);

					sfxManager.PlaySFX_G(4);
					ResetUlt();
					MDebugLog("Fail");
				}
			}
		}

		public void ResetUlt()
		{
			voiceAmplification.SetPlayer(NONE_INT);
			voiceAmplification.SetEnable(false);
			IsStop.SetValue(false);
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CloseVote));
		}
	}
}