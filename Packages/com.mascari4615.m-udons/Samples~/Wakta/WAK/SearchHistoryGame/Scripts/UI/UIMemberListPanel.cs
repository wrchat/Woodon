using UdonSharp;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMemberListPanel : MBase
	{
		private SHG_Manager manager;
		private UIMemberButton[] memberButtons;

		private WaktaMember[] members;
		private WaktaverseData waktaverseData;

		public void Init(SHG_Manager manager, WaktaMemberType memberType)
		{
			this.manager = manager;

			memberButtons = GetComponentsInChildren<UIMemberButton>(true);

			members = Waktaverse.GetMembersByType(memberType);
			waktaverseData = WaktaverseData.GetInstance();

			for (int i = 0; i < memberButtons.Length; i++)
			{
				if (i < members.Length)
				{
					WaktaverseMemberData memberData = waktaverseData.GetData(members[i]);
					memberButtons[i].Init(this, memberData);
				}
				else
				{
					memberButtons[i].gameObject.SetActive(false);
				}
			}
		}

		public void UpdateUI()
		{
			for (int i = 0; i < memberButtons.Length; i++)
				if (i < members.Length)
					memberButtons[i].UpdateUI();
		}

		public void SelectMember(WaktaMember member)
		{
			manager.SelectMember(member);
		}
	}
}