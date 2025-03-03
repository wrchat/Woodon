
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WVideoPlayerControllerButton : WBase
	{
		[SerializeField] private TextMeshProUGUI videoNameText;
		private WVideoPlayerControllerUI videoPlayerControllerUI;
		private int index;

		public void Init(WVideoPlayerControllerUI videoPlayerControllerUI, int index, string videoName)
		{
			this.videoPlayerControllerUI = videoPlayerControllerUI;
			this.index = index;
			videoNameText.text = videoName;
		}

		public void Click()
		{
			videoPlayerControllerUI.PlayVideo(index);
		}
	}
}