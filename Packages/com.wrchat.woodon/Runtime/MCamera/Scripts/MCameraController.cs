using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using static WRC.Woodon.MUtil;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MCameraController : MPickup
	{
		[Header("_" + nameof(MCameraController))]
		[SerializeField] private MCameraFovSync mCameraFovSync;
		[SerializeField] private MCameraPosSync mCameraPosSync;
		[SerializeField] private Camera targetCamera;

		// private CameraManager cameraManager;

		private int curCCPosData;

		private readonly float[] fovData = new float[108];

		// private readonly bool isLookingAt = false;

		public int CurCCPosData
		{
			get => curCCPosData;
			set
			{
				curCCPosData = value;

				// i.e. CC 3-5
				// ccPosData : (2 * 10) + 4 = 24
				// ccPosIndex : 24 / 10 = 2
				// ccPosNum : 24 % 10 = 4

				var ccPosIndex = curCCPosData / 10;
				var ccPosNum = curCCPosData % 10;

				// targetCamera.transform.parent = cameraManager.GetCCPos(ccPosIndex, ccPosNum);
				// targetCamera.transform.localPosition = Vector3.zero;
				// targetCamera.transform.localRotation = Quaternion.identity;

				// isLookingAt = !(
				//     (ccPosIndex == 0 && ccPosNum == 1) ||
				//     (ccPosIndex == 1 && ccPosNum == 1) ||
				//     (ccPosIndex == 1 && ccPosNum == 2) ||
				//     (ccPosIndex == 2 && ccPosNum == 0) ||
				//     (ccPosIndex == 3) ||
				//     (ccPosIndex == 4) ||
				//     (ccPosIndex == 5 && ccPosNum == 1) ||
				//     (ccPosIndex == 9));
			}
		}

		public void UpdateFov()
		{
			targetCamera.fieldOfView = mCameraFovSync.SyncedValue;
			fovData[curCCPosData] = mCameraFovSync.SyncedValue;
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			// cameraManager = GameObject.Find(nameof(CameraManager)).GetComponent<CameraManager>();

			// targetCamera = transform.GetChild(0).GetComponent<Camera>();
			// targetCamera.transform.localPosition = Vector3.zero;
			// targetCamera.transform.localRotation = Quaternion.identity;

			// mCameraPosSync = transform.GetComponent<MCameraPosSync>();
			// mCameraFovSync = transform.GetComponent<MCameraFovSync>();

			for (var i = 0; i < fovData.Length; i++)
				fovData[i] = 40f;

			mCameraFovSync.Init(this);
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

		// public RenderTexture GetCameraTargetTexture()
		// {
		// 	if (targetCamera == null)
		// 		targetCamera = transform.GetChild(0).GetComponent<Camera>();

		// 	return targetCamera.targetTexture;
		// }

		public override void OnPickup()
		{
			base.OnPickup();

			SetOwner();
			SetOwner(mCameraFovSync.gameObject);
			// SetOwner(mCameraPosSync.gameObject);
		}
	}
}