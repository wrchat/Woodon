using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ThiefSeat : UdonSharpBehaviour
	{
		[SerializeField] private VRCStation seat;
		[SerializeField] private ShootingTarget shootingTarget;
		[field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
		[SerializeField] private GameObject memo;

		private void Start()
		{
			// A를 제외한 다른 플레이어들은 자리에 고정되어 움직일 수 없다.
			seat.disableStationExit = true;
			seat.canUseStationFromStation = false;
		}

		public void SetMemoActive(bool active)
		{
			memo.SetActive(active);
		}

		public void UseStation()
		{
			seat.UseStation(Networking.LocalPlayer);
		}

		public void ExitStation()
		{
			seat.ExitStation(Networking.LocalPlayer);
		}

		public void SetShootingTargetActive(bool active)
		{
			shootingTarget.gameObject.SetActive(active);
		}
	}
}