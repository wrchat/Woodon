using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WVideoScreen : WBase
	{
		[SerializeField] private WVideoPlayer wVideoPlayer;
		[SerializeField] private RawImage[] rawImages;
		[SerializeField] private bool hideVideoWhenIsNotPlaying = false;

		private void Update()
		{
			UpdateScreen();
		}

		public void UpdateScreen()
		{
			foreach (RawImage rawImage in rawImages)
			{
				if (wVideoPlayer.IsPlaying)
				{
					rawImage.texture = wVideoPlayer.GetVideoTexture();
					rawImage.color = Color.white;
				}
				else if (hideVideoWhenIsNotPlaying)
				{
					rawImage.texture = null;
					rawImage.color = Color.clear;
				}
				else
				{
					rawImage.texture = null;
					rawImage.color = Color.black;
				}
			}
		}
	}
}
