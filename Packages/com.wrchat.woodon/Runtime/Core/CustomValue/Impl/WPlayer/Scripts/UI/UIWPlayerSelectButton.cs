using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWPlayerSelectButton : WBase
	{
		[Header("_" + nameof(UIWPlayerSelectButton))]
		[SerializeField] private TextMeshProUGUI playerNameText;
		private UIWPlayer wPlayerUI;
		private int index;

		public void Init(UIWPlayer wPlayerUI, int index)
		{
			this.wPlayerUI = wPlayerUI;
			this.index = index;
		}

		public void UpdateUI(string playerName)
		{
			playerNameText.text = playerName;
		}

		public void Click()
		{
			wPlayerUI.SelectPlayer(index);
		}
	}
}