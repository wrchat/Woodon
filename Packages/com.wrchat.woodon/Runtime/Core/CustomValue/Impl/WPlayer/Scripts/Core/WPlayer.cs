using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WPlayer : WEventPublisher
	{
		[Header("_" + nameof(WPlayer))]
		[SerializeField] private string autoTargetName = "-";

		[UdonSynced, FieldChangeCallback(nameof(TargetPlayerID))] private int _targetPlayerID = NONE_INT;
		public int TargetPlayerID
		{
			get => _targetPlayerID;
			set
			{
				int origin = _targetPlayerID;
				_targetPlayerID = value;
				OnTargetPlayerChanged(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}

		protected virtual void OnTargetPlayerChanged(DataChangeState changeState)
		{
			if (DataChangeStateUtil.IsDataChanged(changeState))
				SendEvents();
		}

		[field: Header("_" + nameof(WPlayer) + " - Options")]
		[field: SerializeField] public bool UseNone { get; private set; } = true;

		// ---- ---- ---- ----
	
		public bool IsTargetPlayer(VRCPlayerApi targetPlayer = null)
		{
			if (targetPlayer == null)
				targetPlayer = Networking.LocalPlayer;
			
			return targetPlayer.playerId == TargetPlayerID;
		}

		public VRCPlayerApi GetTargetPlayerAPI() => VRCPlayerApi.GetPlayerById(TargetPlayerID);

		public void SetTarget(int id)
		{
			WDebugLog($"{nameof(SetTarget)} : {id}");

			SetOwner();
			TargetPlayerID = id;
			RequestSerialization();
		}

		public void SetTargetPlayerLocalPlayer() => SetTarget(Networking.LocalPlayer.playerId);
		public void SetTargetPlayerNone() => SetTarget(NONE_INT);

		public void ToggleLocalPlayer()
		{
			if (IsTargetPlayer(Networking.LocalPlayer))
				SetTargetPlayerNone();
			else
				SetTargetPlayerLocalPlayer();
		}

		public virtual void ResetTargetPlayer() => SetTarget(UseNone ? NONE_INT : Networking.LocalPlayer.playerId);

		// ---- ---- ---- ----

		protected virtual void Start()
		{
			Init();
		}

		private void Init()
		{
			if (Networking.IsMaster)
			{
				ResetTargetPlayer();
			}
		}

		public override void OnPlayerJoined(VRCPlayerApi player)
		{
			if (IsOwner() && (Networking.LocalPlayer.displayName == autoTargetName))
			{
				SetTarget(Networking.LocalPlayer.playerId);
			}
		}

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if (IsOwner() && (player.playerId == TargetPlayerID))
			{
				ResetTargetPlayer();
			}
		}
	}
}