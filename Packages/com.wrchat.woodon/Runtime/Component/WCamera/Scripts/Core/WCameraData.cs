using Cinemachine;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WCameraData : WBase
	{
		[field: Header("_" + nameof(WCameraData))]
		[field: SerializeField] public KeyCode KeyCode { get; set; } = KeyCode.None;
		[field: SerializeField] public bool IsUseKeyCode { get; set; } = true;

		public CinemachineVirtualCamera Camera
		{
			get
			{
				if (_camera == null)
					_camera = GetComponent<CinemachineVirtualCamera>();

				return _camera;
			}
		}
		private CinemachineVirtualCamera _camera;
	}
}