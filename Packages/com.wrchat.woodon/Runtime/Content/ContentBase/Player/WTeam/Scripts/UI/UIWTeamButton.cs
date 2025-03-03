using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIWTeamButton : WBase
	{
		[field: SerializeField] public WPlayer WPlayer { get; private set; }
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private string noneString = "-";

		private WTeam mTeam;

		public void Init(WTeam mTeam)
		{
			WPlayer.RegisterListener(this, nameof(OnPlayerChanged));
			OnPlayerChanged();

			this.mTeam = mTeam;
		}

		public void OnPlayerChanged()
		{
			VRCPlayerApi targetPlayerAPI = WPlayer.GetTargetPlayerAPI();
			nameText.text = targetPlayerAPI != null ? targetPlayerAPI.displayName : noneString;

			mTeam.PlayerChanged(this);
		}

		public void Click()
		{
			ToggleLocalPlayer();
		}

		private void ToggleLocalPlayer()
		{
			WPlayer.ToggleLocalPlayer();
		}

		public bool IsPlayer(VRCPlayerApi targetPlayer = null)
		{
			return WPlayer.IsTargetPlayer(targetPlayer);
		}
	}
}