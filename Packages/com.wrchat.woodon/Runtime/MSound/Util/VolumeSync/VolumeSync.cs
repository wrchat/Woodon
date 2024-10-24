using UnityEngine;

namespace WRC.Woodon
{
	public class VolumeSync : MBase
	{
		[SerializeField] private MValue volumeValue;
		[SerializeField] private AudioSource[] audioSources;
		// [SerializeField] private float volumeCorrection = 1;

		private void Update()
		{
			if (volumeValue == null)
				return;

			foreach (AudioSource audioSource in audioSources)
				audioSource.volume = volumeValue.Value;
		}
	}
}