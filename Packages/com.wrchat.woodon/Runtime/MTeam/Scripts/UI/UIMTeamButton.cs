using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIMTeamButton : MBase
	{
		[field: SerializeField] public MTarget MTarget { get; private set; }
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private string noneString = "-";

		private MTeam mTeam;

		public void Init(MTeam mTeam)
		{
			MTarget.RegisterListener(this, nameof(OnPlayerChanged));
			OnPlayerChanged();

			this.mTeam = mTeam;
		}

		public void OnPlayerChanged()
		{
			VRCPlayerApi targetPlayerAPI = MTarget.GetTargetPlayerAPI();
			nameText.text = targetPlayerAPI != null ? targetPlayerAPI.displayName : noneString;

			mTeam.PlayerChanged(this);
		}

		public void Click()
		{
			ToggleLocalPlayer();
		}

		private void ToggleLocalPlayer()
		{
			MTarget.ToggleLocalPlayer();
		}

		public bool IsPlayer(VRCPlayerApi targetPlayer = null)
		{
			return MTarget.IsTargetPlayer(targetPlayer);
		}
	}
}