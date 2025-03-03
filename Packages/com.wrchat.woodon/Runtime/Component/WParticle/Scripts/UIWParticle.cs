using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWParticle : WBase
	{
		[Header("_" + nameof(UIWParticle))]
		[SerializeField] private WParticle wParticle;

		#region HorribleEvents
		public void Play() => wParticle.Play();
		public void Stop() => wParticle.Stop();

		public void Play2Times_L() => wParticle.Play2Times_L();
		public void Play3Times_L() => wParticle.Play3Times_L();
		public void Play4Times_L() => wParticle.Play4Times_L();
		public void Play5Times_L() => wParticle.Play5Times_L();
		public void Play6Times_L() => wParticle.Play6Times_L();
		public void Play7Times_L() => wParticle.Play7Times_L();
		public void Play8Times_L() => wParticle.Play8Times_L();
		public void Play9Times_L() => wParticle.Play9Times_L();
		
		public void Play_G() => wParticle.Play_G();
		public void Stop_G() => wParticle.Stop_G();

		public void Play2Times_G() => wParticle.Play2Times_G();
		public void Play3Times_G() => wParticle.Play3Times_G();
		public void Play4Times_G() => wParticle.Play4Times_G();
		public void Play5Times_G() => wParticle.Play5Times_G();
		public void Play6Times_G() => wParticle.Play6Times_G();
		public void Play7Times_G() => wParticle.Play7Times_G();
		public void Play8Times_G() => wParticle.Play8Times_G();
		public void Play9Times_G() => wParticle.Play9Times_G();
		#endregion
	}
}
