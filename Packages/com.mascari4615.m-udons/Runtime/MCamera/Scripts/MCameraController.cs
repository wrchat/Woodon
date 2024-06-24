using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MCameraController : MBase
	{
		[SerializeField] private CustomBool isHolding;
		[SerializeField] private VRC_Pickup pickup;

		// private CameraManager cameraManager;

		public bool IsPlayerHolding(VRCPlayerApi playerApi)
		{
			return IsPlayerHolding(playerApi, pickup);
		}

		private int curCCPosData;

		private readonly float[] fovData = new float[108];

		private float fovValue = 40;
		private bool isLocal;
		private readonly bool isLookingAt = false;
		[SerializeField] private MCameraFovSync mCameraFovSync;
		[SerializeField] private MCameraPosSync mCameraPosSync;
		[SerializeField] private Camera targetCamera;

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

				FovValue = fovData[curCCPosData];

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

		public float FovValue
		{
			get => fovValue;
			set
			{
				MDebugLog("FovValue: " + value);

				fovValue = value;
				targetCamera.fieldOfView = fovValue;
				fovData[curCCPosData] = fovValue;
			}
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

			isLocal = (mCameraPosSync || mCameraFovSync) == false;

			for (var i = 0; i < fovData.Length; i++)
				fovData[i] = 40f;

			mCameraFovSync.Init(this);
		}

		private void Update()
		{
			if (isHolding != null)
				isHolding.SetValue(IsPlayerHolding(Networking.LocalPlayer, pickup));
		}

		private void LateUpdate()
		{
			// if (isLookingAt)
			// 	targetCamera.transform.LookAt(cameraManager.LookAt);
		}

		public void AddFov(float amount)
		{
			if (isLocal)
				FovValue += amount;
			else if (IsOwner())
				mCameraFovSync.FovValue = FovValue + amount;
		}

		public void SetFOV(float newFov)
		{
			if (isLocal)
				FovValue = newFov;
			else if (IsOwner())
				mCameraFovSync.FovValue = newFov;
		}

		public void SetCCPosData(int newCCPosData)
		{
			if (isLocal)
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

		public void TakeOwner()
		{
			if (!isLocal)
			{
				SetOwner();
				SetOwner(mCameraPosSync.gameObject);
				SetOwner(mCameraFovSync.gameObject);
			}
		}
	}
}