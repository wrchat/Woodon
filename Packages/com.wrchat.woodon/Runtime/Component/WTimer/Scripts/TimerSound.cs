using System;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TimerSound : MBase
	{
		[Header("_" + nameof(Timer))]
		[SerializeField] private Timer timer;
		[SerializeField] private UITimer timerUI;

		[SerializeField] private AudioSource loopAudioSource;
		[SerializeField] private AudioClip defaultSFX;

		[SerializeField] private float[] timeFlags;
		[SerializeField] private AudioClip[] timeSFXs;
		[SerializeField] private bool[] useLoops;

		[SerializeField] private AudioSource sfxSource;
		[SerializeField] private AudioClip startSFX;
		[SerializeField] private AudioClip endSFX;

		[SerializeField] private AudioSource lerpSFXSource;
		[SerializeField] private AudioClip lerpSFX;


		private void Start()
		{
			Init();
		}

		private void Init()
		{
			timer.RegisterListener(this, nameof(PlayStartSFX), TimerEvent.TimerStarted);
			timer.RegisterListener(this, nameof(PlayEndSFX), TimerEvent.TimeExpired);

			lerpSFXSource.clip = lerpSFX;
			lerpSFXSource.mute = true;
			lerpSFXSource.loop = true;
			lerpSFXSource.Play();

			loopAudioSource.clip = defaultSFX;
			loopAudioSource.mute = true;
			loopAudioSource.loop = true;
			loopAudioSource.Play();
		}

		public void PlayStartSFX()
		{
			sfxSource.PlayOneShot(startSFX);
		}

		public void PlayEndSFX()
		{
			sfxSource.PlayOneShot(endSFX);
		}

		private void Update()
		{
			UpdateSound();
		}

		private void UpdateSound()
		{
			if (timer == null)
				return;

			lerpSFXSource.volume = timerUI.IsLerping ? 1 : 0;
		
			int expireTime = timer.ExpireTime;

			if (expireTime == NONE_INT)
			{
				if (loopAudioSource.loop == true)
					loopAudioSource.mute = true;
				return;
			}
			loopAudioSource.mute = false;

			int diff = expireTime - timer.CalcedCurTime;
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(diff);

			bool isInFlag = false;
			for (int i = timeFlags.Length - 1; i >= 0; i--)
			{
				if (timeSpan.TotalSeconds <= timeFlags[i])
				{
					isInFlag = true;
					loopAudioSource.loop = useLoops[i];
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
				loopAudioSource.loop = true;
				if (loopAudioSource.clip != defaultSFX)
				{
					loopAudioSource.clip = defaultSFX;
					loopAudioSource.Play();
				}
			}
		}
	}
}