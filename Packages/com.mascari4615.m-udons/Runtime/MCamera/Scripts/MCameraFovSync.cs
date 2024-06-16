using UdonSharp;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MCameraFovSync : UdonSharpBehaviour
	{
		/*[UdonSynced(UdonSyncMode.Smooth)]
		[FieldChangeCallback(nameof(FovValue))]
		private float fovValue;

		private MCameraController mCameraController;

		public float FovValue
		{
			get => fovValue;
			set
			{
				fovValue = value;
				mCameraController.FovValue = fovValue;
			}
		}

		private void Start()
		{
			mCameraController = transform.GetComponent<MCameraController>();
		}*/
	}
}