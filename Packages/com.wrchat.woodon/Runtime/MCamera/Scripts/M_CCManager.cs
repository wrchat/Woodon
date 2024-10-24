using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class M_CCManager : MBase
	{
		[Header("_" + nameof(M_CCManager))]
		[SerializeField] private M_CCData[] cameraDatas;
		[SerializeField] private Camera cameraBrain;
		[SerializeField] private MValue cameraIndex;

		[Header("_" + nameof(M_CCManager) + " - Options")]
		[SerializeField] private bool canTurnOffCamera = true;
		private int lastCameraIndex = NONE_INT;

		// private bool useOmakaseCam = false;

		/*public int OmakaseCamIndex
		{
			get => _omakaseCamIndex;
			set
			{
				_omakaseCamIndex = value;

				if (useOmakaseCam)
					SetCamera(OmakaseCamIndex);
			}
		}
		[UdonSynced, FieldChangeCallback(nameof(OmakaseCamIndex))] private int _omakaseCamIndex = 0;

		[SerializeField] private TextMeshProUGUI curOwnerText;*/
		[SerializeField] private KeyCode camOffKeyCode = KeyCode.Backspace;
		// [SerializeField] private KeyCode omakaseCamKeyCode = KeyCode.F12;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (cameraDatas.Length == 0)
				cameraDatas = transform.GetComponentsInChildren<M_CCData>();

			// cameraIndex_MValue.SetMinMaxValue(NONE_INT, cameraDatas.Length - 1);
			cameraIndex.RegisterListener(this, nameof(UpdateCameraIndexByMValue));

			TurnOffCamera();
		}

		public void UpdateCameraIndexByMValue()
		{
			MDebugLog($"{nameof(UpdateCameraIndexByMValue)} : {cameraIndex.Value}");
			if (cameraIndex.Value < 0 || cameraIndex.Value >= cameraDatas.Length)
				TurnOffCamera();
			else
				SetCamera(cameraIndex.Value, isReciever: true);
		}

		private void Update()
		{
			if (canTurnOffCamera)
			{
				// useOmakaseCam = false;
				if (Input.GetKeyDown(camOffKeyCode) ||
					Input.GetKeyDown(KeyCode.Backspace) ||
					Input.GetKeyDown(KeyCode.Escape))
				{
					cameraIndex.SetValue(NONE_INT);
					TurnOffCamera();
				}
			}

			/*else if (Input.GetKeyDown(omakaseCamKeyCode) && !IsOwner())
			{
				useOmakaseCam = !useOmakaseCam;

				if (useOmakaseCam)
				{
					// SetCamera(useOmakaseCam ? OmakaseCamIndex : lastCameraIndex);
					SetCamera(OmakaseCamIndex);
				}
				else TurnOffCamera();
			}*/
			// else
			{
				// if (!useOmakaseCam)
				{
					for (int i = 0; i < cameraDatas.Length; i++)
					{
						if (cameraDatas[i].KeyCode == KeyCode.None)
							continue;

						if (Input.GetKeyDown(cameraDatas[i].KeyCode))
						{
							SetCamera(i);
							break;
						}
					}
				}
			}

			// curOwnerText.text = Networking.GetOwner(gameObject).displayName;
		}

		public void SetCamera(int newCameraIndex, bool alwaysOn = false, bool isReciever = false)
		{
			MDebugLog($"{nameof(SetCamera)}({newCameraIndex}) : {alwaysOn}, {isReciever}");

			// None | Invalid index
			if (newCameraIndex < 0 || newCameraIndex >= cameraDatas.Length)
			{
				MDebugLog($"{nameof(SetCamera)} : Invalid index");
				TurnOffCamera();
				return;
			}

			// Toggle if same index
			if (newCameraIndex == lastCameraIndex && alwaysOn == false)
			{
				MDebugLog($"{nameof(SetCamera)} : Same index");
				TurnOffCamera();
				return;
			}

			if (isReciever == false)
				if (cameraIndex != null)
				{
					lastCameraIndex = cameraIndex.Value;
					cameraIndex.SetValue(newCameraIndex);
				}

			cameraBrain.enabled = true;
			for (int i = 0; i < cameraDatas.Length; i++)
				cameraDatas[i].Camera.Priority = (newCameraIndex == i) ? 4444 : NONE_INT;

			// if (!useOmakaseCam)
			// cameraIndex.SetValue(newCameraIndex);

			/*if (IsOwner())
			{
				OmakaseCamIndex = cameraIndex;
				RequestSerialization();
			}*/
		}

		public void TurnOffCamera()
		{
			MDebugLog($"{nameof(TurnOffCamera)}");

			// useOmakaseCam = false;
			cameraBrain.enabled = false;
			lastCameraIndex = NONE_INT;
		}

		/*public void SetOmakaseOwner()
		{
			SetOwner();
			useOmakaseCam = false;
			TurnOffCamera();
		}*/

		/*[SerializeField] private Transform lookAt;

		[SerializeField] private RawImage fullScreen;

		[SerializeField] private MRail[] rails;

		private CCPosData[] ccPosDatas;
		private int curCameraControllerIndex;
		private int curCCPosIndex;

		private MRail curRail;
		private float fovMoveSpeed = 10f;

		private int fovMoveSpeedState = 3;
		private Image[] mCameraControllerButtonImages;

		private MCameraController[] mCameraControllers;
		public Transform LookAt => lookAt;

		private void Start()
		{
			var CCPosDatas = transform.Find("CCPosDatas");
			ccPosDatas = new CCPosData[CCPosDatas.childCount];
			ccPosDatas = CCPosDatas.GetComponentsInChildren<CCPosData>();

			var MCameraControllers = transform.Find("MCameraControllers");
			mCameraControllers = new MCameraController[MCameraControllers.childCount];
			mCameraControllers = MCameraControllers.GetComponentsInChildren<MCameraController>();

			var CameraUICanvas = transform.Find("CameraUICanvas");
			mCameraControllerButtonImages = new Image[CameraUICanvas.childCount];
			mCameraControllerButtonImages = CameraUICanvas.GetComponentsInChildren<Image>();

			TakeCameraOwner(0);
			SetCCPosIndex(0);
			fullScreen.transform.parent.gameObject.SetActive(false);
		}

		private void Update()
		{
			// var isStaff = IsCam(Networking.LocalPlayer);
			// if (isStaff == false)
			//	return;

			if (Input.GetKeyDown(KeyCode.F1)) SetCCPosIndex(0);
			else if (Input.GetKeyDown(KeyCode.F2)) SetCCPosIndex(1);
			else if (Input.GetKeyDown(KeyCode.F3)) SetCCPosIndex(2);
			else if (Input.GetKeyDown(KeyCode.F4)) SetCCPosIndex(3);
			else if (Input.GetKeyDown(KeyCode.F5)) SetCCPosIndex(4);
			else if (Input.GetKeyDown(KeyCode.F6)) SetCCPosIndex(5);
			else if (Input.GetKeyDown(KeyCode.F7)) SetCCPosIndex(6);
			else if (Input.GetKeyDown(KeyCode.F8)) SetCCPosIndex(7);
			else if (Input.GetKeyDown(KeyCode.F9)) SetCCPosIndex(8);
			else if (Input.GetKeyDown(KeyCode.F10)) SetCCPosIndex(9);
			else if (Input.GetKeyDown(KeyCode.F11)) SetCCPosIndex(10);

			if (Input.GetKeyDown(KeyCode.Alpha1)) SetAndUpdateCCPos(0);
			else if (Input.GetKeyDown(KeyCode.Alpha2)) SetAndUpdateCCPos(1);
			else if (Input.GetKeyDown(KeyCode.Alpha3)) SetAndUpdateCCPos(2);
			else if (Input.GetKeyDown(KeyCode.Alpha4)) SetAndUpdateCCPos(3);
			else if (Input.GetKeyDown(KeyCode.Alpha5)) SetAndUpdateCCPos(4);
			else if (Input.GetKeyDown(KeyCode.Alpha6)) SetAndUpdateCCPos(5);
			else if (Input.GetKeyDown(KeyCode.Alpha7)) SetAndUpdateCCPos(6);
			else if (Input.GetKeyDown(KeyCode.Alpha8)) SetAndUpdateCCPos(7);
			else if (Input.GetKeyDown(KeyCode.Alpha9)) SetAndUpdateCCPos(8);

			if (Input.GetKeyDown(KeyCode.Backspace)) fullScreen.transform.parent.gameObject.SetActive(false);

			if (Input.GetKey(KeyCode.UpArrow))
				mCameraControllers[curCameraControllerIndex].AddFov(Time.deltaTime * -fovMoveSpeed);
			else if (Input.GetKey(KeyCode.DownArrow))
				mCameraControllers[curCameraControllerIndex].AddFov(Time.deltaTime * fovMoveSpeed);

			if (Input.GetKeyDown(KeyCode.Alpha0))
				SwitchFovMoveSpeed();

			// if curCamera is railCamera
			if (curRail != null)
			{
				if (Input.GetKeyDown(KeyCode.Minus))
					curRail.SwitchSpeed();
				else if (Input.GetKeyDown(KeyCode.Plus))
					curRail.ToggleAutoMoving();

				if (curRail.AutoMoving)
				{
					if (Input.GetKeyDown(KeyCode.LeftArrow))
						curRail.DirectionLeft();
					else if (Input.GetKeyDown(KeyCode.RightArrow))
						curRail.DirectionRight();
				}
				else
				{
					if (Input.GetKey(KeyCode.LeftArrow))
						curRail.AddRail(false);
					else if (Input.GetKey(KeyCode.RightArrow))
						curRail.AddRail(true);
				}
			}
		}

		public Transform GetCCPos(int ccPosIndex, int ccPosNum)
		{
			return ccPosDatas[ccPosIndex].IndexOf(ccPosNum);
		}

		private void SwitchFovMoveSpeed()
		{
			if (fovMoveSpeedState == 1)
			{
				fovMoveSpeedState = 2;
				fovMoveSpeed = 1f;
			}
			else if (fovMoveSpeedState == 2)
			{
				fovMoveSpeedState = 3;
				fovMoveSpeed = 10f;
			}
			else
			{
				fovMoveSpeedState = 1;
				fovMoveSpeed = .5f;
			}
		}

		public void TakeCameraOwner0()
		{
			TakeCameraOwner(0);
		}

		public void TakeCameraOwner1()
		{
			TakeCameraOwner(1);
		}

		public void TakeCameraOwner2()
		{
			TakeCameraOwner(2);
		}

		public void TakeCameraOwner3()
		{
			TakeCameraOwner(3);
		}

		private void TakeCameraOwner(int index)
		{
			curCameraControllerIndex = index;
			fullScreen.texture = mCameraControllers[curCameraControllerIndex].GetCameraTargetTexture();

			for (var i = 0; i < mCameraControllerButtonImages.Length; i++)
				mCameraControllerButtonImages[i].color =
					mCameraControllers[i] == mCameraControllers[curCameraControllerIndex]
						? Color.green
						: Color.red;

			if (!Networking.LocalPlayer.IsOwner(mCameraControllers[curCameraControllerIndex].gameObject))
			{
				Networking.SetOwner(Networking.LocalPlayer,
					mCameraControllers[curCameraControllerIndex].gameObject);
				mCameraControllers[curCameraControllerIndex].TakeOwner();
			}
		}

		private void SetCCPosIndex(int num)
		{
			if (num > ccPosDatas.Length - 1)
				return;

			curCCPosIndex = num;
		}

		private void SetAndUpdateCCPos(int ccPosNum)
		{
			// i.e.

			// CC 1-3
			// ccPosData == (0 * 10) + 2 == 2
			// ccPosIndex == 2 / 10 == 0
			// ccPosNum == 2 % 10 == 2

			// CC 11-2
			// ccPosData == (10 * 10) + 1 == 101
			// ccPosIndex == 101 / 10 == 10
			// ccPosNum == 101 % 10 == 1

			if (ccPosDatas[curCCPosIndex].Length() - 1 < ccPosNum)
				return;

			fullScreen.transform.parent.gameObject.SetActive(true);
			curRail = curCCPosIndex >= 6 && curCCPosIndex <= 8 ? rails[curCCPosIndex - 6] : null;

			mCameraControllers[curCameraControllerIndex].SetCCPosData(curCCPosIndex * 10 + ccPosNum);
		}*/
	}
}