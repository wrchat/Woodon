using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMTargetPlayerSelectButton : MBase
	{
		[Header("_" + nameof(UIMTargetPlayerSelectButton))]
		[SerializeField] private TextMeshProUGUI playerNameText;
		private MTargetUI mTargetUI;
		private int index;

		public void Init(MTargetUI mTargetUI, int index)
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