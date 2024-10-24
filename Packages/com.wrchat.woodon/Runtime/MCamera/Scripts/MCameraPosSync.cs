using UdonSharp;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MCameraPosSync : UdonSharpBehaviour
	{
		/*[UdonSynced]
		[FieldChangeCallback(nameof(CurCCPosData))]
		private int curCCPosData;

		private MCameraController mCameraController;

		public int CurCCPosData
		{
			get => curCCPosData;
			set
			{
				curCCPosData = value;
				mCameraController.CurCCPosData = curCCPosData;
			}
		}

		private void Start()
		{
			mCameraController = transform.GetComponent<MCameraController>();
		}

		public void SetCCPosData(int newCCPosData)
		{
			CurCCPosData = newCCPosData;
			RequestSerialization();
		}*/
	}
}