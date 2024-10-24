
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MVideoPlayerControllerButton : MBase
	{
		[SerializeField] private TextMeshProUGUI videoNameText;
		private MVideoPlayerControllerUI videoPlayerControllerUI;
		private int index;

		public void Init(MVideoPlayerControllerUI videoPlayerControllerUI, int index, string videoName)
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