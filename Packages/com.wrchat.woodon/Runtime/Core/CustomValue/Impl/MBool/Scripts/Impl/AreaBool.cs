using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AreaBool : MBool
	{
		[Header("_" + nameof(AreaBool))]
		[SerializeField] private Collider[] areaColliders;
		[SerializeField] private float updateDelay = 0.1f;
		[SerializeField] private bool checkOnlyLocalPlayer = true;
		private Bounds[] boundsArray;

		protected override void Start()
		{
			boundsArray = new Bounds[areaColliders.Length];
			for (int i = 0; i < boundsArray.Length; i++)
			{
				// boundsArray[i] = new Bounds(areaColliders[i].center, areaColliders[i].size);
				boundsArray[i] = areaColliders[i].bounds;
			}
			base.Start();

			SendCustomEventDelayedSeconds(nameof(UpdateValue), updateDelay);
		}

		public void UpdateValue()
		{
			SendCustomEventDelayedSeconds(nameof(UpdateValue), updateDelay);

			if (IsNotOnline())
				return;

			if (checkOnlyLocalPlayer)
			{
				bool isPlayerIn = IsPlayerIn(Networking.LocalPlayer);

				if (isPlayerIn != Value)
					SetValue(isPlayerIn);
			}
			else
			{
				VRCPlayerApi[] players = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
				VRCPlayerApi.GetPlayers(players);

				bool isPlayerIn = false;

				foreach (VRCPlayerApi playerApi in players)
				{
					if (IsPlayerIn(playerApi))
					{
						isPlayerIn = true;
						break;
					}
				}

				if (isPlayerIn != Value)
					SetValue(isPlayerIn);
			}
		}

		public bool IsPlayerIn(VRCPlayerApi player)
		{
			if (player == null)
				return false;

			Vector3 playerPos = player.GetPosition();
			foreach (Bounds bounds in boundsArray)
			{
				if (bounds.Contains(playerPos))
				{
					return true;
				}
			}
			
			return false;
		}
	}
}