using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MVideoScreen : MBase
	{
		[SerializeField] private MVideoPlayer mVideoPlayer;
		[SerializeField] private RawImage[] rawImages;
		[SerializeField] private bool hideVideoWhenIsNotPlaying = false;

		private void Update()
		{
			UpdateScreen();
		}

		public void UpdateScreen()
		{
			foreach (RawImage rawImage in rawImages)
				if (mVideoPlayer.IsPlaying)
				{
					rawImage.texture = mVideoPlayer.GetVideoTexture();
					rawImage.color = Color.white;
				}
				else if (hideVideoWhenIsNotPlaying)
				{
					rawImage.texture = null;
					rawImage.color = Color.white * 0;
				}
		}
	}
}
