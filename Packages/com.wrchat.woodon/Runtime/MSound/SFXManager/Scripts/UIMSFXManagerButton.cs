using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMSFXManagerButton : MBase
	{
		[SerializeField] private TextMeshProUGUI sfxNameText;
		private UIMSFXManager sfxManagerUI;
		private int index;

		public void Init(UIMSFXManager sfxManagerUI, int index, string sfxName)
		{
			this.sfxManagerUI = sfxManagerUI;
			this.index = index;

			sfxNameText.text = sfxName;
		}
		
		public void Click()
		{
			SelectSFX();
		}

		public void SelectSFX()
		{
			sfxManagerUI.PlaySFX(index);
		}
	}
}