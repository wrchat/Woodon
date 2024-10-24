using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using Random = UnityEngine.Random;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.LIL.ButkoWorld
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class LilpaShootingManager : MBase
	{
		[Header("_" + nameof(LilpaShootingManager))]
		[SerializeField] private MBool[] isShooted;
		[SerializeField] private MValue score;

		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private TextMeshProUGUI timeStateText;

		// =====================================================================

		[SerializeField] private MeshRenderer gameStateMeshRenderer;
		[SerializeField] private int waitTime = 10;
		[SerializeField] private float gameTime = 30;
		[SerializeField] private int spawnLatencyByDecisecondMin = 4;
		[SerializeField] private int spawnLatencyByDecisecondMax = 15;

		private int nextSpawnIndex;
		private float nextSpawnTime;

		private string[] spawnDatas;

		private int startTime;


		public bool IsGaming
		{
			get => isGaming;
			private set
			{
				isGaming = value;
				OnGameStateChange();
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(IsGaming))] private bool isGaming;

		private string SpawnData
		{
			get => spawnData;
			set
			{
				spawnData = value;
				MDebugLog($"{nameof(SpawnData)} Changed to : {SpawnData}");
				spawnDatas = spawnData.Split(new[] { DATA_PACK_SEPARATOR }, StringSplitOptions.None);

				nextSpawnIndex = 0;
				startTime = int.Parse(spawnDatas[nextSpawnIndex]
					.Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None)[0]);
				nextSpawnTime = startTime;
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(SpawnData))] private string spawnData = "";

		private void Start()
		{
			OnScoreChange();
			OnGameStateChange();
		}

		private void Update()
		{
			// Debug.Log(Networking.SimulationTime(Networking.LocalPlayer) + "_" + Networking.SimulationTime(gameObject));

			float time = (startTime + gameTime * 1000 - Networking.GetServerTimeInMilliseconds()) / 1000;

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
			MDebugLog(nameof(OnScoreChange));
			scoreText.text = score.Value.ToString();
		}

		public void TryGetScore(int index)
		{
			MDebugLog($"{nameof(TryGetScore)} : index = {index}");

			if (isShooted[index].Value)
			{
				isShooted[index].SetValueFalse();
				score.AddValue(1);
			}
		}

		private void OnGameStateChange()
		{
			MDebugLog(nameof(OnGameStateChange));
			gameStateMeshRenderer.material.color = IsGaming ? Color.green : Color.red;
		}

		private string GetRandomSpawnData()
		{
			string newSpawnData = "";

			int startTimeByMilsec = Networking.GetServerTimeInMilliseconds() + waitTime * 1000;
			int nextSpawnTimeByMilsec = startTimeByMilsec;

			for (float gameTimeBySec = 0; gameTimeBySec < gameTime;)
			{
				int spawnLatencyBySec = Random.Range(spawnLatencyByDecisecondMin, spawnLatencyByDecisecondMax);
				nextSpawnTimeByMilsec += spawnLatencyBySec * 100;

				newSpawnData += $"{nextSpawnTimeByMilsec}";
				newSpawnData += $"{DATA_SEPARATOR}{Random.Range(0, isShooted.Length)}";

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
			score.SetValue(0);

			if (IsGaming)
				SpawnData = GetRandomSpawnData();

			foreach (MBool isShooted_ in isShooted)
				isShooted_.SetValueFalse();

			RequestSerialization();
		}

		private void StandUp()
		{
			string[] s = spawnDatas[nextSpawnIndex].Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None);

			nextSpawnTime = int.Parse(s[0]);
			int batIndex = int.Parse(s[1]);

			MDebugLog($"{nameof(StandUp)} : dataIndex = {nextSpawnIndex}, batIndex{int.Parse(s[1])}");

			if (IsOwner(isShooted[batIndex].gameObject))
				isShooted[batIndex].SetValueTrue();
		}

		#region HorribleEvents
		public void TryGetScore0() => TryGetScore(0);
		public void TryGetScore1() => TryGetScore(1);
		public void TryGetScore2() => TryGetScore(2);
		public void TryGetScore3() => TryGetScore(3);
		public void TryGetScore4() => TryGetScore(4);
		public void TryGetScore5() => TryGetScore(5);
		public void TryGetScore6() => TryGetScore(6);
		public void TryGetScore7() => TryGetScore(7);
		public void TryGetScore8() => TryGetScore(8);
		public void TryGetScore9() => TryGetScore(9);
		public void TryGetScore10() => TryGetScore(10);
		#endregion
	}
}