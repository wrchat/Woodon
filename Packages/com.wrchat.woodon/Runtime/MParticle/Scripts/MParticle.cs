using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MParticle : MBase
	{
		[Header("_" + nameof(MParticle))]
		[SerializeField] private ParticleSystem[] particleSystems;

		[Header("_" + nameof(MColor) + " - Option")]
		[SerializeField] private MColor mColor;
		[SerializeField] private float delay = 0f;
		[SerializeField] private ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmittingAndClear;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (mColor != null)
			{
				mColor.RegisterListener(this, nameof(UpdateColorByMColor));
				UpdateColorByMColor();
			}
		}

		public void UpdateColorByMColor()
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				ParticleSystem.MainModule mainModule = particleSystem.main;
				mainModule.startColor = mColor.Value;
			}
		}

		public void Play()
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				if (particleSystem.isPlaying)
					particleSystem.Stop(true, stopBehavior);
				particleSystem.Play();
			}
		}

		public void PlayNTimes(int n)
		{
			for (int i = 0; i < n; i++)
				SendCustomEventDelayedSeconds(nameof(Play), i * delay);
		}
		
		public void Stop()
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				particleSystem.Stop(true, stopBehavior);
			}
		}


		#region HorribleEvents
		public void Play2Times_L() => PlayNTimes(2);
		public void Play3Times_L() => PlayNTimes(3);
		public void Play4Times_L() => PlayNTimes(4);
		public void Play5Times_L() => PlayNTimes(5);
		public void Play6Times_L() => PlayNTimes(6);
		public void Play7Times_L() => PlayNTimes(7);
		public void Play8Times_L() => PlayNTimes(8);
		public void Play9Times_L() => PlayNTimes(9);

		public void Play_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play));
		public void Stop_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Stop));

		public void Play2Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play2Times_L));
		public void Play3Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play3Times_L));
		public void Play4Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play4Times_L));
		public void Play5Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play5Times_L));
		public void Play6Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play6Times_L));
		public void Play7Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play7Times_L));
		public void Play8Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play8Times_L));
		public void Play9Times_G() => SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Play9Times_L));
		#endregion
	}
}