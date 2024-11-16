using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MTarget : WEventPublisher
	{
		[Header("_" + nameof(MTarget))]
		[SerializeField] private string autoTargetName = "-";

		[UdonSynced, FieldChangeCallback(nameof(TargetPlayerID))] private int _targetPlayerID = NONE_INT;
		public int TargetPlayerID
		{
			get => _targetPlayerID;
			set
			{
				int origin = _targetPlayerID;
				_targetPlayerID = value;
				OnTargetChanged(DataChangeStateUtil.GetChangeState(origin, value));
			}
		}

		protected virtual void OnTargetChanged(DataChangeState changeState)
		{
			if (DataChangeStateUtil.IsDataChanged(changeState))
				SendEvents();
		}

		[field: Header("_" + nameof(MTarget) + " - Options")]
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
			MDebugLog($"{nameof(SetTarget)} : {id}");

			SetOwner();
			TargetPlayerID = id;
			RequestSerialization();
		}

		public void SetTargetLocalPlayer() => SetTarget(Networking.LocalPlayer.playerId);
		public void SetTargetNone() => SetTarget(NONE_INT);

		public void ToggleLocalPlayer()
		{
			if (IsTargetPlayer(Networking.LocalPlayer))
				SetTargetNone();
			else
				SetTargetLocalPlayer();
		}

		public virtual void ResetPlayer() => SetTarget(UseNone ? NONE_INT : Networking.LocalPlayer.playerId);

		// ---- ---- ---- ----

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (Networking.IsMaster)
				ResetPlayer();
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
				ResetPlayer();
			}
		}
	}
}