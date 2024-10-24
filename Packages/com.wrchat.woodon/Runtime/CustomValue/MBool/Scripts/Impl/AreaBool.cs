using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AreaBool : MBool
	{
		[Header("_" + nameof(AreaBool))]
		[SerializeField] private BoxCollider[] areaColliders;
		[SerializeField] private float updateDelay = 0.5f;
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

			if (Networking.LocalPlayer == null)
				return;
		
			bool isPlayerIn = IsPlayerIn(Networking.LocalPlayer);

			if (isPlayerIn != Value)
				SetValue(isPlayerIn);
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