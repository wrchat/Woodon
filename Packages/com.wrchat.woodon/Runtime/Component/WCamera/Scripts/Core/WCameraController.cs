using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WCameraController : MPickup
	{
		[Header("_" + nameof(WCameraController))]
		[SerializeField] private MCameraFovSync mCameraFovSync;
		[SerializeField] private Camera targetCamera;

		private int curCCPosData;

		public int CurCCPosData
		{
			get => curCCPosData;
			set
			{
				curCCPosData = value;
			}
		}

		public void UpdateFov()
		{
			targetCamera.fieldOfView = mCameraFovSync.SyncedValue;
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			mCameraFovSync.Init(this);
		}

		protected override void Update()
		{
			Move();
		}

		private void Move()
		{
			// 유니티 에디터랑 똑같은 움직임
			// WASD로 움직이고, QE로 상하로 움직임, Shift로 빠르게 움직임, 마우스로 회전함
			
			if (IsOwner())
			{
				Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				float speed = Input.GetKey(KeyCode.LeftShift) ? 10f : 5f;

				transform.Translate(speed * Time.deltaTime * move);

				Vector3 rot = transform.rotation.eulerAngles;
				rot.y += Input.GetAxis("Mouse X") * 5f;
				rot.x -= Input.GetAxis("Mouse Y") * 5f;
				transform.rotation = Quaternion.Euler(rot);
			}
		}

		private void LateUpdate()
		{
			// if (isLookingAt)
			// 	targetCamera.transform.LookAt(cameraManager.LookAt);

			UpdateFov();
		}

		public void SetCCPosData(int newCCPosData)
		{
			// if (isLocal)
				CurCCPosData = newCCPosData;
			// else if (IsOwner())
			// 	mCameraPosSync.SetCCPosData(newCCPosData);
		}

		public RenderTexture GetCameraTargetTexture() => targetCamera.targetTexture;

		public override void OnPickup()
		{
			base.OnPickup();

			SetOwner();
			SetOwner(mCameraFovSync.gameObject);
		}
	}
}