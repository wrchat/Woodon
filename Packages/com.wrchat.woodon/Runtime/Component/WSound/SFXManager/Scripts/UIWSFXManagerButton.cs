using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWSFXManagerButton : WBase
	{
		[SerializeField] private TextMeshProUGUI sfxNameText;
		private UIWSFXManager sfxManagerUI;
		private int index;

		public void Init(UIWSFXManager sfxManagerUI, int index, string sfxName)
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