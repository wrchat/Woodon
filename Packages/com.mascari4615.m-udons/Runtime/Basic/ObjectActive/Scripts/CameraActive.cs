using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CameraActive : MBase
	{
		[SerializeField] private Camera[] cameras;
		[SerializeField] private MBool mBool;

		private void Start()
		{
			Init();
			UpdateValue();
		}

		private void Init()
		{
			mBool.RegisterListener(this, nameof(UpdateValue));
		}

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			MDebugLog($"{nameof(UpdateValue)}");

			foreach (Camera camera in cameras)
				camera.enabled = mBool.Value;
		}
	}
}