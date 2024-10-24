using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMTargetPlayerSelectButton : MBase
	{
		[Header("_" + nameof(UIMTargetPlayerSelectButton))]
		[SerializeField] private TextMeshProUGUI playerNameText;
		private UIMTarget mTargetUI;
		private int index;

		public void Init(UIMTarget mTargetUI, int index)
		{
			this.mTargetUI = mTargetUI;
			this.index = index;
		}

		public void UpdateUI(string playerName)
		{
			playerNameText.text = playerName;
		}

		public void Click()
		{
			mTargetUI.SelectPlayer(index);
		}
	}
}