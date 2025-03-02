using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WCameraManager : WBase
	{
		[Header("_" + nameof(WCameraManager))]
		[SerializeField] private WCameraData[] cameraDatas;
		[SerializeField] private Camera cameraBrain;
		[SerializeField] private MValue cameraIndex;

		[Header("_" + nameof(WCameraManager) + " - Options")]
		[SerializeField] private bool canTurnOffCamera = true;
		private int lastCameraIndex = NONE_INT;

		[SerializeField] private KeyCode camOffKeyCode = KeyCode.Backspace;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (cameraDatas.Length == 0)
				cameraDatas = transform.GetComponentsInChildren<WCameraData>();

			// cameraIndex.SetMinMaxValue(NONE_INT, cameraDatas.Length - 1);
			cameraIndex.RegisterListener(this, nameof(UpdateCameraIndexByMValue));

			TurnOffCamera();
		}

		public void UpdateCameraIndexByMValue()
		{
			WDebugLog($"{nameof(UpdateCameraIndexByMValue)} : {cameraIndex.Value}");
			if ((cameraIndex.Value < 0) || (cameraIndex.Value >= cameraDatas.Length))
				TurnOffCamera();
			else
				SetCamera(cameraIndex.Value, isReciever: true);
		}

		private void Update()
		{
			if (canTurnOffCamera)
			{
				if (Input.GetKeyDown(camOffKeyCode) ||
					Input.GetKeyDown(KeyCode.Backspace) ||
					Input.GetKeyDown(KeyCode.Escape))
				{
					cameraIndex.SetValue(NONE_INT); // UpdateCameraIndexByMValue -> TurnOffCamera
				}
			}

			for (int i = 0; i < cameraDatas.Length; i++)
			{
				if (cameraDatas[i].KeyCode == KeyCode.None)
					continue;

				if (Input.GetKeyDown(cameraDatas[i].KeyCode))
				{
					if (lastCameraIndex == i)
						cameraIndex.SetValue(NONE_INT); // UpdateCameraIndexByMValue -> TurnOffCamera
					else
						SetCamera(i);
				
					break;
				}
			}
		}

		public void SetCamera(int newCameraIndex, bool alwaysOn = false, bool isReciever = false)
		{
			WDebugLog($"{nameof(SetCamera)}({newCameraIndex}) : {alwaysOn}, {isReciever}");

			// None | Invalid index
			if ((newCameraIndex < 0) || (newCameraIndex >= cameraDatas.Length))
			{
				WDebugLog($"{nameof(SetCamera)} : Invalid index");
				TurnOffCamera();
				return;
			}

			// Toggle if same index
			if ((newCameraIndex == lastCameraIndex) && (alwaysOn == false))
			{
				WDebugLog($"{nameof(SetCamera)} : Same index");
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
			lastCameraIndex = cameraIndex.Value;
		
			for (int i = 0; i < cameraDatas.Length; i++)
				cameraDatas[i].Camera.Priority = (newCameraIndex == i) ? 4444 : NONE_INT;
		}

		public void TurnOffCamera()
		{
			WDebugLog($"{nameof(TurnOffCamera)}");

			cameraBrain.enabled = false;
			lastCameraIndex = NONE_INT;
		}
	}
}