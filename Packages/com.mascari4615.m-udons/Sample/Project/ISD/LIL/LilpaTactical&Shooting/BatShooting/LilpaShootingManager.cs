using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using Random = UnityEngine.Random;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class LilpaShootingManager : MBase
	{
		[SerializeField] private SyncedBool[] syncedBools;
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private TextMeshProUGUI timeStateText;

		// =====================================================================

		[SerializeField] private MeshRenderer gameStateMeshRenderer;
		[SerializeField] private int waitTime = 10;
		[SerializeField] private float gameTime = 30;
		[SerializeField] private int spawnLatencyByDecisecondMin = 4;
		[SerializeField] private int spawnLatencyByDecisecondMax = 15;

		[UdonSynced]
		[FieldChangeCallback(nameof(IsGaming))]
		private bool isGaming;

		private int nextSpawnIndex;
		private float nextSpawnTime;

		[UdonSynced]
		[FieldChangeCallback(nameof(Score))]
		private int score;

		[UdonSynced]
		[FieldChangeCallback(nameof(SpawnData))]
		private string spawnData = "";

		private string[] spawnDatas;

		private int startTime;

		private int Score
		{
			get => score;
			set
			{
				score = value;
				OnScoreChange();
			}
		}

		private bool IsGaming
		{
			get => isGaming;
			set
			{
				isGaming = value;
				OnGameStateChange();
			}
		}

		private string SpawnData
		{
			get => spawnData;
			set
			{
				spawnData = value;
				Debug.Log($"{nameof(SpawnData)} Changed to : {SpawnData}");
				spawnDatas = spawnData.Split(new[] { DATA_PACK_SEPARATOR }, StringSplitOptions.None);

				nextSpawnIndex = 0;
				startTime = int.Parse(spawnDatas[nextSpawnIndex]
					.Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None)[0]);
				nextSpawnTime = startTime;
			}
		}

		private void Start()
		{
			OnScoreChange();
			OnGameStateChange();
		}

		private void Update()
		{
			// Debug.Log(Networking.SimulationTime(Networking.LocalPlayer) + "_" + Networking.SimulationTime(gameObject));

			var time = (startTime + gameTime * 1000 - Networking.GetServerTimeInMilliseconds()) / 1000;

			if (time <= 0)
			{
				timeText.text = "-";
				timeStateText.text = "게임 종료 : ";
			}
			else if (time >= gameTime)
			{
				timeText.text = (time - gameTime).ToString();
				timeStateText.text = "시작까지 : ";
			}
			else
			{
				timeText.text = time.ToString();
				timeStateText.text = "남은 시간 : ";
			}

			if (isGaming)
			{
				if (spawnDatas == null)
					return;
				if (spawnDatas.Length - 1 <= nextSpawnIndex)
					return;

				if (Networking.GetServerTimeInMilliseconds() >= nextSpawnTime)
				{
					StandUp();
					nextSpawnIndex++;
				}
			}
		}

		private void OnScoreChange()
		{
			Debug.Log(nameof(OnScoreChange));
			scoreText.text = Score.ToString();
		}

		public void TryGetScore(int index)
		{
			Debug.Log($"{nameof(TryGetScore)} : index = {index}");

			if (syncedBools[index].Value)
			{
				SetOwner();
				syncedBools[index].SetValueFalse();
				Score++;
				RequestSerialization();
			}
		}

		public void TryGetScore0()
		{
			TryGetScore(0);
		}

		public void TryGetScore1()
		{
			TryGetScore(1);
		}

		public void TryGetScore2()
		{
			TryGetScore(2);
		}

		public void TryGetScore3()
		{
			TryGetScore(3);
		}

		public void TryGetScore4()
		{
			TryGetScore(4);
		}

		public void TryGetScore5()
		{
			TryGetScore(5);
		}

		public void TryGetScore6()
		{
			TryGetScore(6);
		}

		public void TryGetScore7()
		{
			TryGetScore(7);
		}

		public void TryGetScore8()
		{
			TryGetScore(8);
		}

		public void TryGetScore9()
		{
			TryGetScore(9);
		}

		public void TryGetScore10()
		{
			TryGetScore(10);
		}

		private void OnGameStateChange()
		{
			Debug.Log(nameof(OnGameStateChange));
			gameStateMeshRenderer.material.color = IsGaming ? Color.green : Color.red;
		}

		private string GetRandomSpawnData()
		{
			var newSpawnData = "";

			var startTimeByMilsec = Networking.GetServerTimeInMilliseconds() + waitTime * 1000;
			var nextSpawnTimeByMilsec = startTimeByMilsec;

			for (float gameTimeBySec = 0; gameTimeBySec < gameTime;)
			{
				var spawnLatencyBySec = Random.Range(spawnLatencyByDecisecondMin, spawnLatencyByDecisecondMax);
				nextSpawnTimeByMilsec += spawnLatencyBySec * 100;

				newSpawnData += $"{nextSpawnTimeByMilsec}";
				newSpawnData += $"{DATA_SEPARATOR}{Random.Range(0, syncedBools.Length)}";

				newSpawnData += $"{DATA_PACK_SEPARATOR}";
				gameTimeBySec += spawnLatencyBySec * .1f;
			}

			newSpawnData.Trim(new char[] { DATA_PACK_SEPARATOR });

			return newSpawnData;
		}

		public void GameStateSwitch()
		{
			SetOwner();
			IsGaming = !IsGaming;
			Score = 0;

			if (IsGaming)
				SpawnData = GetRandomSpawnData();

			foreach (var syncedBool in syncedBools)
				syncedBool.SetValueFalse();

			RequestSerialization();
		}

		private void StandUp()
		{
			var s = spawnDatas[nextSpawnIndex].Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None);

			nextSpawnTime = int.Parse(s[0]);
			var batIndex = int.Parse(s[1]);

			Debug.Log($"{nameof(StandUp)} : dataIndex = {nextSpawnIndex}, batIndex{int.Parse(s[1])}");

			if (IsOwner(syncedBools[batIndex].gameObject))
				syncedBools[batIndex].SetValueTrue();
		}
	}
}