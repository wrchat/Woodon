using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CameraActive : ActiveToggle
	{
		[Header("_" + nameof(CameraActive))]
		[SerializeField] private Camera[] cameras;

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}");

			foreach (Camera camera in cameras)
				camera.enabled = Active;
		} 
	}
}