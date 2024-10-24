
using Cinemachine;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class M_CCData : MBase
	{
		[field: Header("_" + nameof(M_CCData))]
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