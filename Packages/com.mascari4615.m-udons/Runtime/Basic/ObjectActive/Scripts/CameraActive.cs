using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	// TODO: Camera -> Component
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class CameraActive : MBase
	{
		[Header("_" + nameof(CameraActive))]
		[SerializeField] private Camera[] cameras;
		[SerializeField] private MBool mBool;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mBool != null)
			{
				mBool.RegisterListener(this, nameof(UpdateValue));
				UpdateValue();
			}
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