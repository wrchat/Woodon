using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.Wak.SearchHistoryGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMemberButton : UdonSharpBehaviour
	{
		private UIMemberListPanel panel;
		private WaktaverseMemberData data;

		private Button button;

		// HACK:
		[SerializeField] private Image[] profileImages;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Image disableImage;
		[SerializeField] private bool onlyShowOnce = false;


		public void Init(UIMemberListPanel panel, WaktaverseMemberData data)
		{
			this.panel = panel;
			this.data = data;

			button = GetComponent<Button>();

			foreach (Image profileImage in profileImages)
				profileImage.sprite = data.Profile;
			nameText.text = Waktaverse.GetNickname(data.Member);
		}

		public void UpdateUI()
		{
			disableImage.gameObject.SetActive(data.SyncData == SHG_Manager.GetWaktaMemberClearSyncData());

			if (onlyShowOnce)
				button.interactable = data.SyncData != SHG_Manager.GetWaktaMemberClearSyncData();
		}

		public void Click()
		{
			panel.SelectMember(data.Member);
		}
	}
}