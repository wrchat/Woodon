using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	// [DefaultExecutionOrder(100)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionDraw : MBase
	{
		[field: SerializeField] public DrawManager DrawManager { get; private set; }
		[field: SerializeField] public AuctionManager AuctionManager { get; private set; }
		[SerializeField] private MUI[] uis;

		public bool IsInited { get; private set; } = false;

		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(TargetIndex))] private int _targetIndex = NONE_INT;
		public int TargetIndex
		{
			get => _targetIndex;
			set
			{
				_targetIndex = value;
				OnTargetIndexChanged();
			}
		}

		private void OnTargetIndexChanged()
		{
			MDebugLog($"{nameof(OnTargetIndexChanged)}, TargetIndex : {TargetIndex}");

			if (DrawManager.DrawElementDatas == null)
				return;

			UpdateUI();
		}

		private void Update()
		{
			if (IsInited == false)
			{
				if (AuctionManager.IsInited && DrawManager.IsInited)
				{
					Init();
					UpdateUI();
				}
			}
		}

		private void Init()
		{
			MDebugLog(nameof(Init));
		
			if (IsInited)
				return;
			IsInited = true;

			foreach (MUI ui in uis)
				ui.Init(this);

			DrawManager.RegisterListener(this, nameof(UpdateUI));
			DrawManager.RegisterListener(AuctionManager, nameof(AuctionManager.UpdateStuff));

			if (Networking.IsMaster)
				OnWait();
		}

		public void UpdateUI()
		{
			MDebugLog(nameof(UpdateUI));

			if (IsInited == false)
				return;

			foreach (MUI ui in uis)
				ui.UpdateUI(this);
		}

		public void UpdateDrawByAuction()
		{
			MDebugLog(nameof(UpdateDrawByAuction));

			switch ((AuctionState)AuctionManager.CurGameState)
			{
				case AuctionState.Wait:
					// 아직 팀이 정해지지 않은 랜덤한 한 명 (미리 설정)
					OnWait();
					break;
				case AuctionState.ShowTarget:
					// 경매 대상 공개
					break;
				case AuctionState.AuctionTime:
					break;
				case AuctionState.WaitForResult:
					break;
				case AuctionState.CheckResult:
					OnCheckResult();
					break;
				case AuctionState.ApplyResult:
					// 경매 결과 적용
					OnApplyResult();
					break;
			}

			UpdateUI();
		}

		protected virtual void OnCheckResult()
		{

		}

		private void OnWait()
		{
			MDebugLog(nameof(OnWait));

			if (IsOwner() == false)
				return;

			DrawElementData noneTeamDrawElementData = FindNoneTeamDrawElementData();

			if (noneTeamDrawElementData != null)
			{
				DrawElementData randomNoneTeamDrawElementData = GetRandomNoneTeamDrawElementData();
				SetTargetIndex(randomNoneTeamDrawElementData.Index);
			}
			else
			{
				MDebugLog("NoneTeamDrawElementData is null");
			}
		}

		private void OnApplyResult()
		{
			MDebugLog(nameof(OnApplyResult));

			if (IsOwner(DrawManager.gameObject) == false)
				return;

			if (AuctionManager.WinnerIndex == NONE_INT)
				return;

			// HACK: AuctionSeat와 DrawElementData의 Index가 같다면, 둘 다 동일한 플레이어를 대상으로 한다고 가정
			TeamType teamType = DrawManager.DrawElementDatas[AuctionManager.WinnerIndex].TeamType;
			int point = AuctionManager.GetMaxTurnData();
			DrawManager.SetElementData(TargetIndex, teamType, DrawRole.Normal, true, point.ToString());
			DrawManager.SyncData();
		}

		private DrawElementData FindNoneTeamDrawElementData()
		{
			if (DrawManager.DrawElementDatas == null)
			{
				MDebugLog("DrawElementDatas is null");
				return null;
			}

			foreach (DrawElementData drawElementData in DrawManager.DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					return drawElementData;
			}

			return null;
		}

		private DrawElementData GetRandomNoneTeamDrawElementData()
		{
			if (DrawManager.DrawElementDatas == null)
				return null;

			DrawElementData[] noneTeamDrawElementDatas = new DrawElementData[DrawManager.DrawElementDatas.Length];
			int count = 0;

			foreach (DrawElementData drawElementData in DrawManager.DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					noneTeamDrawElementDatas[count++] = drawElementData;
			}

			if (count == 0)
				return null;

			return noneTeamDrawElementDatas[Random.Range(0, count)];
		}

		public void SetTargetIndex(int targetIndex)
		{
			MDebugLog($"{nameof(SetTargetIndex)}, TargetIndex : {targetIndex}");

			SetOwner();
			TargetIndex = targetIndex;
			RequestSerialization();
		}

		public void SetAllRemainRandomAndSync()
		{
			MDebugLog(nameof(SetAllRemainRandomAndSync));

			DrawManager.SetAllRemainRandom(true, "0");
			DrawManager.SyncData();
		}
	}
}
