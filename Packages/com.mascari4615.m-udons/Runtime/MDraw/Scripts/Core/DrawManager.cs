using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DrawManager : MBase
	{
		[SerializeField] private int teamCount = 2; // 팀 수
		[SerializeField] private int teamPlayerCount = 2; // 팀당 인원 수

		[SerializeField] private UIDraw[] drawUIs;
		[SerializeField] private UIDrawController[] drawControllers;

		private DrawElementData[] drawElementDatas;

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
			for (int i = 0; i < drawElementDatas.Length; i++)
				drawElementDatas[i].ParseDataPack(dataPacks[i]);

			foreach (UIDraw drawUI in drawUIs)
				drawUI.UpdateUI(drawElementDatas);
		}

		public void SyncData()
		{
			SetOwner();

			string data = string.Empty;
			foreach (DrawElementData drawElementData in drawElementDatas)
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
			drawElementDatas = GetComponentsInChildren<DrawElementData>(true);

			foreach (UIDraw drawUI in drawUIs)
				drawUI.Init();

			foreach (UIDrawController drawController in drawControllers)
				drawController.Init(this);

			InitData();
			SyncData();
		}

		private void InitData()
		{
			// 팀당 인원 수 * 팀 수 만큼 데이터 생성
			
			// 1. 사전 데이터 설정
			for (int i = 0; i < drawElementDatas.Length; i++)
			{
				drawElementDatas[i].Index = i;
				drawElementDatas[i].TeamType = drawElementDatas[i].InitTeamType;
				drawElementDatas[i].Role = drawElementDatas[i].InitRole;

				drawElementDatas[i].IsShowing = true;
			}

			// 2. 랜덤으로 데이터 생성해서 남은 자리 채워넣기
			int[] remainTeamPlayerCounts = new int[teamCount];
			for (int i = 0; i < teamCount; i++)
				remainTeamPlayerCounts[i] = teamPlayerCount;

			// 각 플레이어에 대해 팀과 역할 설정
			for (int i = 0; i < teamCount * teamPlayerCount; i++)
			{
				// 이미 설정된 팀이 있으면 패스
				if (drawElementDatas[i].TeamType != TeamType.None)
				{
					remainTeamPlayerCounts[(int)drawElementDatas[i].TeamType]--;
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

				drawElementDatas[i].TeamType = (TeamType)randomTeamIndex;
				drawElementDatas[i].Role = DrawRole.Normal;

				drawElementDatas[i].IsShowing = false;
			}
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
			foreach (DrawElementData drawElementData in drawElementDatas)
			{
				if (drawElementData.TeamType == teamType)
					drawElementData.IsShowing = true;
			}
			SyncData();
		}
	}
}
