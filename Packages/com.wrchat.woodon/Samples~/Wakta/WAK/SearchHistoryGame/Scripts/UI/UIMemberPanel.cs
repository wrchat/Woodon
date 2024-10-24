using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.Wak.SearchHistoryGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMemberPanel : MBase
	{
		private SHG_Manager manager;
		private CanvasGroup canvasGroup;
		private WaktaverseData waktaverseData;

		[SerializeField] private Image memberProfile;
		[SerializeField] private TextMeshProUGUI memberName;
		[SerializeField] private UIHistoryButton[] historyButtons;

		public void Init(SHG_Manager manager)
		{
			this.manager = manager;
			canvasGroup = GetComponent<CanvasGroup>();
			waktaverseData = WaktaverseData.GetInstance();
		}

		public void UpdateUI()
		{
			canvasGroup.alpha = manager.SelectedMember == WaktaMember.None ? 0 : 1;
			canvasGroup.blocksRaycasts = manager.SelectedMember != WaktaMember.None;
			canvasGroup.interactable = manager.SelectedMember != WaktaMember.None;

			if (manager.SelectedMember == WaktaMember.None)
				return;

			WaktaverseMemberData memberData = waktaverseData.GetData(manager.SelectedMember);
			string[] data = memberData.SyncData.Split(DATA_SEPARATOR);

			memberProfile.sprite = memberData.Profile;
			memberName.text = Waktaverse.GetNickname(memberData.Member);

			historyButtons[0].SetData(memberData, 0);
			historyButtons[1].SetData(memberData, 1);
			historyButtons[2].SetData(memberData, 2);

			if (data.Length < 3)
			{
				historyButtons[0].SetEnabled(false);
				historyButtons[1].SetEnabled(false);
				historyButtons[2].SetEnabled(false);

				MDebugLog($"Data length is less than 3: {data.Length}");
				return;
			}

			historyButtons[0].SetEnabled(data[0] == TRUE_STRING);
			historyButtons[1].SetEnabled(data[1] == TRUE_STRING);
			historyButtons[2].SetEnabled(data[2] == TRUE_STRING);
		}

		[ContextMenu(nameof(ShowFirst))]
		public void ShowFirst() => ShowHistory(0);

		[ContextMenu(nameof(ShowSecond))]
		public void ShowSecond() => ShowHistory(1);

		[ContextMenu(nameof(ShowThird))]
		public void ShowThird() => ShowHistory(2);

		public void ShowHistory(int index)
		{
			if (manager.SelectedMember == WaktaMember.None)
				return;

			WaktaverseMemberData memberData = waktaverseData.GetData(manager.SelectedMember);
			string[] data = memberData.SyncData.Split(DATA_SEPARATOR);

			if (data[index] == TRUE_STRING)
				return;

			data[index] = TRUE_STRING;
			memberData.SetSyncData(string.Join(DATA_SEPARATOR.ToString(), data));
		}
	}
}