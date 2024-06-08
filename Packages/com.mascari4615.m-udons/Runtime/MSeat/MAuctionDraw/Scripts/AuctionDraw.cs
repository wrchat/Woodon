using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AuctionDraw : MBase
	{
		[SerializeField] private AuctionManager auctionManager;
		[SerializeField] private DrawManager drawManager;

		// TODO: UI
		[SerializeField] private TextMeshProUGUI targetNameText;

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

			if (drawManager.DrawElementDatas == null)
				return;

			if (TargetIndex == NONE_INT)
			{
				targetNameText.text = string.Empty;
				return;
			}

			foreach (DrawElementData drawElementData in drawManager.DrawElementDatas)
			{
				if (drawElementData.Index == TargetIndex)
				{
					targetNameText.text = drawElementData.Name;
					break;
				}
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			targetNameText.text = string.Empty;
			targetNameText.gameObject.SetActive(false);
			OnTargetIndexChanged();
		}

		public void UpdateDrawByAuction()
		{
			MDebugLog(nameof(UpdateDrawByAuction));

			switch ((AuctionState)auctionManager.CurGameState)
			{
				case AuctionState.Wait:
					// 아직 팀이 정해지지 않은 랜덤한 한 명 (미리 설정)
					targetNameText.gameObject.SetActive(false);
					OnWait();
					break;
				case AuctionState.ShowTarget:
					// 경매 대상 공개
					targetNameText.gameObject.SetActive(true);
					break;
				case AuctionState.AuctionTime:
					break;
				case AuctionState.WaitForResult:
					break;
				case AuctionState.CheckResult:
					break;
				case AuctionState.ApplyResult:
					// 경매 결과 적용
					OnApplyResult();
					break;
			}
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
				SetOwner();
				TargetIndex = randomNoneTeamDrawElementData.Index;
				RequestSerialization();
			}
			else
			{
				MDebugLog("NoneTeamDrawElementData is null");
			}
		}

		private void OnApplyResult()
		{
			MDebugLog(nameof(OnApplyResult));

			if (IsOwner(drawManager.gameObject) == false)
				return;

			if (auctionManager.WinnerIndex == NONE_INT)
				return;

			// HACK: AuctionSeat와 DrawElementData의 Index가 같다면, 둘 다 동일한 플레이어를 대상으로 한다고 가정
			TeamType teamType = drawManager.DrawElementDatas[auctionManager.WinnerIndex].TeamType;
			drawManager.SetElementData(TargetIndex, teamType, DrawRole.Normal, true);
			drawManager.SyncData();
		}

		private DrawElementData FindNoneTeamDrawElementData()
		{
			if (drawManager.DrawElementDatas == null)
				return null;

			foreach (DrawElementData drawElementData in drawManager.DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					return drawElementData;
			}

			return null;
		}

		private DrawElementData GetRandomNoneTeamDrawElementData()
		{
			if (drawManager.DrawElementDatas == null)
				return null;

			DrawElementData[] noneTeamDrawElementDatas = new DrawElementData[drawManager.DrawElementDatas.Length];
			int count = 0;

			foreach (DrawElementData drawElementData in drawManager.DrawElementDatas)
			{
				if (drawElementData.TeamType == TeamType.None)
					noneTeamDrawElementDatas[count++] = drawElementData;
			}

			if (count == 0)
				return null;

			return noneTeamDrawElementDatas[Random.Range(0, count)];
		}
	}
}
