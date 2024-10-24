using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.Wak.SearchHistoryGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class SHG_Manager : MBase
	{
		public static string GetWaktaMemberInitSyncData() => $"{FALSE_STRING}{DATA_SEPARATOR}{FALSE_STRING}{DATA_SEPARATOR}{FALSE_STRING}";
		public static string GetWaktaMemberClearSyncData() => $"{TRUE_STRING}{DATA_SEPARATOR}{TRUE_STRING}{DATA_SEPARATOR}{TRUE_STRING}";

		[UdonSynced, FieldChangeCallback(nameof(SelectedMember))] private WaktaMember _selectedMember = WaktaMember.None;
		public WaktaMember SelectedMember
		{
			get => _selectedMember;
			set
			{
				_selectedMember = value;
				OnSelectedDataChanged();
			}
		}

		[SerializeField] private UIMemberListPanel gomemListPanel;
		[SerializeField] private UIMemberListPanel gomemAcademyListPanel;
		[SerializeField] private UIMemberPanel memberPanel;

		private bool _isInit = false;

		private void Init()
		{
			if (_isInit)
				return;
			_isInit = true;

			gomemListPanel.Init(this, WaktaMemberType.Gomem);
			gomemAcademyListPanel.Init(this, WaktaMemberType.GomemAcademy);
			memberPanel.Init(this);

			WaktaverseData waktaverseData = WaktaverseData.GetInstance();
			if (Networking.IsMaster)
			{
				foreach (WaktaverseMemberData data in waktaverseData.Datas)
					data.SetSyncData(GetWaktaMemberInitSyncData());
			}

			waktaverseData.RegisterListener(this, nameof(UpdateUI));
		}

		public void UpdateUI()
		{
			gomemListPanel.UpdateUI();
			gomemAcademyListPanel.UpdateUI();
			memberPanel.UpdateUI();
		}

		private void OnSelectedDataChanged()
		{
			MDebugLog(nameof(OnSelectedDataChanged));

			if (_isInit == false)
				Init();

			UpdateUI();
		}

		private void Start()
		{
			Init();
			UpdateUI();
		}

		public void SelectMember(WaktaMember member)
		{
			SetOwner();
			SelectedMember = member;
			RequestSerialization();
		}

		public void CloseMemberPanel()
		{
			SetOwner();
			SelectedMember = WaktaMember.None;
			RequestSerialization();
		}

		public void ResetAll()
		{
			SetOwner();
		
			WaktaverseData waktaverseData = WaktaverseData.GetInstance();
			foreach (WaktaverseMemberData data in waktaverseData.Datas)
				data.SetSyncData(GetWaktaMemberInitSyncData());

			SelectedMember = WaktaMember.None;
			
			RequestSerialization();
		}
	}
}