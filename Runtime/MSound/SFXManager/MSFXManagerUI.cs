
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MSFXManagerUI : MBase
	{
		private TextMeshProUGUI[] sfxNameTexts;
		[SerializeField] private Transform textsParent;
		[SerializeField] private MSFXManager sfxManager;
		[SerializeField] private bool global = false;

		private void Start()
		{
			sfxNameTexts = textsParent.GetComponentsInChildren<TextMeshProUGUI>();

			if (sfxNameTexts != null)
				for (var i = 0; i < sfxNameTexts.Length; i++)
				{
					if (i >= sfxManager.AudioClips.Length)
					{
						sfxNameTexts[i].transform.parent.gameObject.SetActive(false);
						continue;
					}

					sfxNameTexts[i].text = sfxManager.AudioClips[i].name;
				}
		}

		private void PlaySFX(int index)
		{
			if (global)
				sfxManager.PlaySFX_G(index);
			else
				sfxManager.PlaySFX_L(index);
		}

		public void PlaySFX0() => PlaySFX(0);
		public void PlaySFX1() => PlaySFX(1);
		public void PlaySFX2() => PlaySFX(2);
		public void PlaySFX3() => PlaySFX(3);
		public void PlaySFX4() => PlaySFX(4);
		public void PlaySFX5() => PlaySFX(5);
		public void PlaySFX6() => PlaySFX(6);
		public void PlaySFX7() => PlaySFX(7);
		public void PlaySFX8() => PlaySFX(8);
		public void PlaySFX9() => PlaySFX(9);
		public void PlaySFX10() => PlaySFX(10);
		public void PlaySFX11() => PlaySFX(11);
		public void PlaySFX12() => PlaySFX(12);
		public void PlaySFX13() => PlaySFX(13);
		public void PlaySFX14() => PlaySFX(14);
		public void PlaySFX15() => PlaySFX(15);
		public void PlaySFX16() => PlaySFX(16);
		public void PlaySFX17() => PlaySFX(17);
		public void PlaySFX18() => PlaySFX(18);
		public void PlaySFX19() => PlaySFX(19);

		public void StopSFX_Global() => sfxManager.StopSFX_Global();
		public void StopSFX() => sfxManager.StopSFX();
	}
}