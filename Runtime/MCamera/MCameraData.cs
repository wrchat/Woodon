
using Cinemachine;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MCameraData : MBase
	{
		[field: Header("_" + nameof(MCameraData))]
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