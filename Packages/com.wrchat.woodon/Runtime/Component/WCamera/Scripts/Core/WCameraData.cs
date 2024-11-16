
using Cinemachine;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WCameraData : MBase
	{
		[field: Header("_" + nameof(WCameraData))]
		[field: SerializeField] public KeyCode KeyCode { get; set; } = KeyCode.None;

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