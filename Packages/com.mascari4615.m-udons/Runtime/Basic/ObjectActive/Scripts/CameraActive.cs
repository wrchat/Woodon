using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CameraActive : MBase
	{
		[SerializeField] private Camera[] cameras;
		[SerializeField] private MBool customBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			customBool.RegisterListener(this, nameof(UpdateValue));
		}

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			MDebugLog($"{nameof(UpdateValue)}");

			foreach (Camera camera in cameras)
				camera.enabled = customBool.Value;
		}
	}
}