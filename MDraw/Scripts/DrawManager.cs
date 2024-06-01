using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	// 조추첨
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class DrawManager : MBase
	{
		// 문자열 데이터팩 만들어서 싱크
		// 파싱하고 정보랑 엮어서 출력

		// 미리 조추첨 데이터를 랜덤으로 생성해서 저장해두고
		// 이를 하나씩 보여주는 방식

		[SerializeField] private int teamCount = 2; // 팀 수
		[SerializeField] private int teamPlayerCount = 2; // 팀당 인원 수

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
			}
		}
	}
}
