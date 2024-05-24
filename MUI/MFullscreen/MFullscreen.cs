using UdonSharp;
using UdonSharp.Video;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Video.Components.Base;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MFullscreen : MBase
	{
		[SerializeField] private Renderer internalAvProRenderer;
		[SerializeField] private BaseVRCVideoPlayer videoPlayer;

		[SerializeField] private RawImage[] targetImages;

		[SerializeField] private VideoScreenHandler videoScreenHandler;

		[SerializeField] private CustomBool customBool;
		[SerializeField] private bool hideVideoWhenIsNotPlaying = false;

		private CanvasGroup canvasGroup;

		private void Start()
		{
			canvasGroup = GetComponent<CanvasGroup>();

			if (!customBool)
				SetScreenFalse();
		}

		private void Update()
		{
			foreach (var targetImage in targetImages)
				if (videoPlayer.IsPlaying) //Playing
				{
					targetImage.texture = videoScreenHandler.GetVideoTexture();
					targetImage.color = Color.white;
				}
				else if (hideVideoWhenIsNotPlaying)
				{
					targetImage.texture = null;
					targetImage.color = Color.white * 0;
				}
		}

		public void ToggleScreen() => canvasGroup.alpha = canvasGroup.alpha == 0 ? 1 : 0;
		public void SetScreen(bool value) => canvasGroup.alpha = value ? 1 : 0;
		public void SetScreenTrue() => SetScreen(true);
		public void SetScreenTrue_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetScreenTrue));
		public void SetScreenFalse() => SetScreen(false);
		public void SetScreenFalse_Global() => SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetScreenFalse));

		public void UpdateValue()
		{
			if (customBool)
				SetScreen(customBool.Value);
		}
	}
}