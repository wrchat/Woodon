using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMemberButton : UdonSharpBehaviour
	{
		private UIMemberListPanel panel;
		private WaktaverseMemberData data;

		private Button button;

		[SerializeField] private Image profile;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private Image disableImage;

		public void Init(UIMemberListPanel panel, WaktaverseMemberData data)
		{
			this.panel = panel;
			this.data = data;

			button = GetComponent<Button>();

			profile.sprite = data.Profile;
			nameText.text = Waktaverse.GetNickname(data.Member);
		}

		public void UpdateUI()
		{
			disableImage.gameObject.SetActive(data.SyncData == SHG_Manager.GetWaktaMemberClearSyncData());
			button.interactable = data.SyncData != SHG_Manager.GetWaktaMemberClearSyncData();
		}

		public void Click()
		{
			panel.SelectMember(data.Member);
		}
	}
}