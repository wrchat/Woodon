using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public enum DrawType
	{
		AllRandom,
		Auction
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DrawManager : MBase
	{
		[SerializeField] private int teamCount = 2; // 팀 수
		[SerializeField] private int teamPlayerCount = 2; // 팀당 인원 수

		[SerializeField] private UIDraw[] drawUIs;
		[SerializeField] private UIDrawController[] drawControllers;

		[SerializeField] private DrawType drawType = DrawType.AllRandom;

		public DrawElementData[] DrawElementDatas { get; private set; }

		[UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(DataPacks))] private string dataPacks = string.Empty;
		public string DataPacks
		{
			get => dataPacks;
			set
			{
				dataPacks = value;
				OnDataChanged();
			}
		}

		private void OnDataChanged()
		{
			MDebugLog($"{nameof(OnDataChanged)}, DataPack : {DataPacks}");

			if (string.IsNullOrEmpty(DataPacks))
				return;

			string[] dataPacks = DataPacks.Split(DATA_PACK_SEPARATOR);
			for (int i = 0; i < DrawElementDatas.Length; i++)
				DrawElementDatas[i].ParseDataPack(dataPacks[i]);

			foreach (UIDraw drawUI in drawUIs)
				drawUI.UpdateUI(DrawElementDatas);
		}

		public void SyncData()
		{
			SetOwner();

			string data = string.Empty;
			foreach (DrawElementData drawElementData in DrawElementDatas)
				data += drawElementData.ToStringData() + DATA_PACK_SEPARATOR;
			DataPacks = data;

			RequestSerialization();
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			MDebugLog(nameof(Init));
			
			DrawElementDatas = GetComponentsInChildren<DrawElementData>(true);

			foreach (UIDraw drawUI in drawUIs)
				drawUI.Init();

			foreach (UIDrawController drawController in drawControllers)
				drawController.Init(this);

			InitData();
			SyncData();
		}

		private void InitData()
		{
			MDebugLog(nameof(InitData));

			// 팀당 인원 수 * 팀 수 만큼 데이터 생성
			
			// 1. 사전 정의 데이터 설정
			for (int i = 0; i < DrawElementDatas.Length; i++)
			{
				DrawElementDatas[i].Index = i;
				DrawElementDatas[i].TeamType = DrawElementDatas[i].InitTeamType;
				DrawElementDatas[i].Role = DrawElementDatas[i].InitRole;

				DrawElementDatas[i].IsShowing = DrawElementDatas[i].TeamType != TeamType.None;
			}
		
			switch (drawType)
			{
				case DrawType.AllRandom:
					SetAllRemainRandom();
					break;
				case DrawType.Auction:
					// 하나씩 뽑아서 팀 설정
					break;
			}
		}

		private void SetAllRemainRandom()
		{
			// 랜덤으로 데이터 생성해서 남은 자리 채워넣기
			int[] remainTeamPlayerCounts = new int[teamCount];
			for (int i = 0; i < teamCount; i++)
				remainTeamPlayerCounts[i] = teamPlayerCount;

			// 각 플레이어에 대해 팀과 역할 설정
			for (int i = 0; i < teamCount * teamPlayerCount; i++)
			{
				// 이미 설정된 팀이 있으면 패스
				if (DrawElementDatas[i].TeamType != TeamType.None)
				{
					remainTeamPlayerCounts[(int)DrawElementDatas[i].TeamType]--;
					continue;
				}

				// 랜덤으로 팀 뽑기
				int randomTeamIndex;
				while (true)
				{
					randomTeamIndex = Random.Range(0, teamCount);
					if (remainTeamPlayerCounts[randomTeamIndex] > 0)
					{
						remainTeamPlayerCounts[randomTeamIndex]--;
						break;
					}
				}

				SetElementData(i, (TeamType)randomTeamIndex, DrawRole.Normal, false);
			}
		}

		public void SetElementData(int index, TeamType teamType, DrawRole role, bool IsShowing)
		{
			MDebugLog($"{nameof(SetElementData)}, Index : {index}, TeamType : {teamType}, Role : {role}, IsShowing : {IsShowing}");
			
			DrawElementDatas[index].TeamType = teamType;
			DrawElementDatas[index].Role = role;
			DrawElementDatas[index].IsShowing = IsShowing;
		}

		[ContextMenu(nameof(ShowTeamA))]
		public void ShowTeamA() => ShowTeam(TeamType.A);
		public void ShowTeamB() => ShowTeam(TeamType.B);
		public void ShowTeamC() => ShowTeam(TeamType.C);
		public void ShowTeamD() => ShowTeam(TeamType.D);
		public void ShowTeamE() => ShowTeam(TeamType.E);
		public void ShowTeamF() => ShowTeam(TeamType.F);
		public void ShowTeamG() => ShowTeam(TeamType.G);
		public void ShowTeamH() => ShowTeam(TeamType.H);
		public void ShowTeamI() => ShowTeam(TeamType.I);
		public void ShowTeamJ() => ShowTeam(TeamType.J);
		public void ShowTeamK() => ShowTeam(TeamType.K);
		public void ShowTeamL() => ShowTeam(TeamType.L);
		public void ShowTeamM() => ShowTeam(TeamType.M);
		public void ShowTeamN() => ShowTeam(TeamType.N);
		public void ShowTeamO() => ShowTeam(TeamType.O);
		public void ShowTeamP() => ShowTeam(TeamType.P);
		public void ShowTeamQ() => ShowTeam(TeamType.Q);
		public void ShowTeamR() => ShowTeam(TeamType.R);
		public void ShowTeamS() => ShowTeam(TeamType.S);
		public void ShowTeamT() => ShowTeam(TeamType.T);
		public void ShowTeamU() => ShowTeam(TeamType.U);
		public void ShowTeamV() => ShowTeam(TeamType.V);
		public void ShowTeamW() => ShowTeam(TeamType.W);
		public void ShowTeamX() => ShowTeam(TeamType.X);
		public void ShowTeamY() => ShowTeam(TeamType.Y);
		public void ShowTeamZ() => ShowTeam(TeamType.Z);

		public void ShowTeam(TeamType teamType)
		{
			foreach (DrawElementData drawElementData in DrawElementDatas)
			{
				if (drawElementData.TeamType == teamType)
					drawElementData.IsShowing = true;
			}
			SyncData();
		}
	}
}
