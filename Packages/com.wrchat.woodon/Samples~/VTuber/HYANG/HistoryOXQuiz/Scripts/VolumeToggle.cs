using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.HistoryOXQuiz
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VolumeToggle : MBase
	{
		[Header("_" + nameof(VolumeToggle))]
		[SerializeField] private WBool wBool;
		[SerializeField] private AudioSource[] audioSources;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			wBool.RegisterListener(this, nameof(UpdateVolume));
		}
		
		public void UpdateVolume()
		{
			foreach (AudioSource audioSource in audioSources)
				audioSource.mute = wBool.Value;
		}
	}
}