using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using Random = UnityEngine.Random;
using WRC.Woodon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class ShootingGameManager : MBase
	{
		[SerializeField] private ShootingGameTarget[] shootingTargets;
		[SerializeField] private Sprite[] fandomSprites;
		[SerializeField] private TeamData[] teamDatas;
		[SerializeField] private MeshRenderer gameStateMeshRenderer;
		[SerializeField] private TextMeshPro timeText;

		[UdonSynced]
		[FieldChangeCallback(nameof(IsGaming))]
		private bool isGaming;

		private int nextSpawnIndex;
		private int nextSpawnTime;

		[UdonSynced]
		[FieldChangeCallback(nameof(SpawnData))]
		private string spawnData = "";

		private string[] spawnDatas;
		private int startTime;

		private bool IsGaming
		{
			get => isGaming;
			set
			{
				isGaming = value;
				gameStateMeshRenderer.material.color = IsGaming ? Color.green : Color.red;

				foreach (TeamData teamData in teamDatas)
					if (Networking.IsOwner(teamData.gameObject))
						teamData.ResetScore();
			}
		}

		private string SpawnData
		{
			get => spawnData;
			set
			{
				spawnData = value;
				spawnDatas = spawnData.Split(new[] { DATA_PACK_SEPARATOR }, StringSplitOptions.None);

				nextSpawnIndex = 0;
				nextSpawnTime =
					int.Parse(spawnDatas[nextSpawnIndex].Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None)[0]);
				startTime = nextSpawnTime;
			}
		}

		private void Update()
		{
			if (isGaming)
			{
				if (spawnDatas == null)
					return;
				if (spawnDatas.Length - 1 <= nextSpawnIndex)
					return;

				timeText.text = Mathf.Clamp(startTime / 1000 + 30 - Networking.GetServerTimeInMilliseconds() / 1000, 0,
					int.MaxValue).ToString();

				if (Networking.GetServerTimeInMilliseconds() >= nextSpawnTime)
				{
					SpawnFandom();
					nextSpawnIndex++;
				}
			}
		}

		public Sprite GetFandomSprite(int fandomIndex)
		{
			return fandomSprites[fandomIndex];
		}

		public ShootingGameTarget GetShootingTarget(int num)
		{
			return shootingTargets[num];
		}

		public void Ahoy(GameObject shootingTargetObject)
		{
			foreach (TeamData teamData in teamDatas)
				if (Networking.IsOwner(teamData.gameObject))
					teamData.Ahoy(shootingTargetObject);
		}

		private string GetRandomSpawnData()
		{
			string newSpawnData = "";

			int startTimeByMilsec = Networking.GetServerTimeInMilliseconds() + 10 * 1000;
			int nextSpawnTimeByMilsec = startTimeByMilsec;

			for (float gameTimeBySec = 0; gameTimeBySec < 30;)
			{
				int spawnLatencyBySec = Random.Range(4, 15);
				nextSpawnTimeByMilsec += spawnLatencyBySec * 100;

				newSpawnData += $"{nextSpawnTimeByMilsec}";
				newSpawnData += $"{DATA_SEPARATOR}{Random.Range(0, 6 + 1)}";
				newSpawnData += $"{DATA_SEPARATOR}{Random.Range(0, 7 + 1)}";
				newSpawnData += $"{DATA_SEPARATOR}{(Random.Range(0, 1 + 1) == 0 ? "L" : "R")}";
				newSpawnData += $"{DATA_SEPARATOR}{Random.Range(.5f, 1.5f)}";
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

			if (IsGaming)
				SpawnData = GetRandomSpawnData();

			RequestSerialization();
		}

		private void SpawnFandom()
		{
			string[] s = spawnDatas[nextSpawnIndex].Split(new[] { DATA_SEPARATOR }, StringSplitOptions.None);

			nextSpawnTime = int.Parse(s[0]);

			int nextFandomIndex = int.Parse(s[1]);
			int nextLine = int.Parse(s[2]);
			Vector3 nextDirection = s[3] == "L" ? Vector3.left : Vector3.right;
			float nextSpeed = float.Parse(s[4]);

			shootingTargets[nextSpawnIndex].Init(nextFandomIndex, nextLine, nextDirection, nextSpeed);
		}
	}
}