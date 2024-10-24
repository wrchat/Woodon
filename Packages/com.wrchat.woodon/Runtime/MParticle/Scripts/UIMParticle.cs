using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMParticle : MBase
	{
		[Header("_" + nameof(UIMParticle))]
		[SerializeField] private MParticle mParticle;

		#region HorribleEvents
		public void Play() => mParticle.Play();
		public void Stop() => mParticle.Stop();

		public void Play2Times_L() => mParticle.Play2Times_L();
		public void Play3Times_L() => mParticle.Play3Times_L();
		public void Play4Times_L() => mParticle.Play4Times_L();
		public void Play5Times_L() => mParticle.Play5Times_L();
		public void Play6Times_L() => mParticle.Play6Times_L();
		public void Play7Times_L() => mParticle.Play7Times_L();
		public void Play8Times_L() => mParticle.Play8Times_L();
		public void Play9Times_L() => mParticle.Play9Times_L();
		
		public void Play_G() => mParticle.Play_G();
		public void Stop_G() => mParticle.Stop_G();

		public void Play2Times_G() => mParticle.Play2Times_G();
		public void Play3Times_G() => mParticle.Play3Times_G();
		public void Play4Times_G() => mParticle.Play4Times_G();
		public void Play5Times_G() => mParticle.Play5Times_G();
		public void Play6Times_G() => mParticle.Play6Times_G();
		public void Play7Times_G() => mParticle.Play7Times_G();
		public void Play8Times_G() => mParticle.Play8Times_G();
		public void Play9Times_G() => mParticle.Play9Times_G();
		#endregion
	}
}
