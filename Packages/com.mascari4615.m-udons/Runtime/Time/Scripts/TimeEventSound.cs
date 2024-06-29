using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TimeEventSound : MBase
	{
		[SerializeField] private TimeEvent timeEvent;

		[SerializeField] private AudioSource loopAudioSource;
		[SerializeField] private AudioSource endingSFXSource;
		[SerializeField] private AudioSource lerpSFXSource;

		[SerializeField] private float[] timeFlags;
		[SerializeField] private AudioClip[] timeSFXs;

		[SerializeField] private AudioClip defaultSFX;
		[SerializeField] private AudioClip endingSFX;

		// HACK:
		[SerializeField] private AudioClip lerpSFX;
		[SerializeField] private UITimeEvent timeEventUI;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			timeEvent.RegisterListener(this, nameof(OnEnding));

			lerpSFXSource.clip = lerpSFX;
			lerpSFXSource.volume = 0;
			lerpSFXSource.loop = true;
			lerpSFXSource.Play();

			loopAudioSource.clip = defaultSFX;
			loopAudioSource.volume = 0;
			loopAudioSource.loop = true;
			loopAudioSource.Play();
		}

		public void OnEnding()
		{
			endingSFXSource.clip = endingSFX;
			endingSFXSource.PlayOneShot(endingSFX);
		}

		private void Update()
		{
			UpdateSound();
		}

		private void UpdateSound()
		{
			if (timeEvent == null)
				return;

			lerpSFXSource.volume = timeEventUI.IsLerping ? 1 : 0;
		
			int expireTime = timeEvent.ExpireTime;

			if (expireTime == NONE_INT)
			{
				loopAudioSource.volume = 0;
				return;
			}
			loopAudioSource.volume = 1;

			int diff = expireTime - Networking.GetServerTimeInMilliseconds();
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(diff);

			bool isInFlag = false;
			for (int i = timeFlags.Length - 1; i >= 0; i--)
			{
				if (timeSpan.TotalSeconds <= timeFlags[i])
				{
					isInFlag = true;
					if (loopAudioSource.clip != timeSFXs[i])
					{
						loopAudioSource.clip = timeSFXs[i];
						loopAudioSource.Play();
					}
					break;
				}
			}

			if (isInFlag == false)
			{
				if (loopAudioSource.clip != defaultSFX)
				{
					loopAudioSource.clip = defaultSFX;
					loopAudioSource.Play();
				}
			}
		}
	}
}