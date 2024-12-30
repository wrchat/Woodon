using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceArea : VoiceTagger
	{
		[Header("_" + nameof(VoiceArea))]
		private Bounds[] boundsArray;
		[SerializeField] private BoxCollider[] areaColliders;

		protected override void Start()
		{
			boundsArray = new Bounds[areaColliders.Length];
			for (int i = 0; i < boundsArray.Length; i++)
			{
				// boundsArray[i] = new Bounds(areaColliders[i].center, areaColliders[i].size);
				boundsArray[i] = areaColliders[i].bounds;
			}
			base.Start();
		}

		public override bool IsCondition(VRCPlayerApi player)
		{
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