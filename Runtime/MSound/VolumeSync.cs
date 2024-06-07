using UnityEngine;

namespace Mascari4615
{
	public class VolumeSync : MBase
	{
		[SerializeField] private SyncedSlider syncedSlider;
		[SerializeField] private AudioSource[] audioSources;
		// [SerializeField] private float volumeCorrection = 1;

		private void Update()
		{
			if (syncedSlider == null)
				return;

			foreach (AudioSource audioSource in audioSources)
				audioSource.volume = syncedSlider.CurValue;
		}
	}
}