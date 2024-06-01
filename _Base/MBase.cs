using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Mascari4615
{
	public class MBase : UdonSharpBehaviour
	{
		public const string DEBUG_PREFIX = "DEBUG";

		public const char DATA_PACK_SEPARATOR = '_';
		public const char DATA_SEPARATOR = '@';

		public const int NONE_INT = -1;
		public const string NONE_STRING = "SANS";
		public const string TRUE_STRING = "TRUE";
		public const string FALSE_STRING = "FALSE";

		[Header("_" + nameof(MBase))]
		[SerializeField] protected bool DEBUG;

		#region HACKs
		protected void MDebugLog(string log = NONE_STRING)
		{
			// To Not Log Error When Player Left The World.
			if (NotOnline)
				return;

			if (DEBUG)
				Debug.Log($"{DEBUG_PREFIX}, {Networking.LocalPlayer.playerId}, {gameObject.name} : {log}");
		}

		protected bool NotOnline => (Networking.LocalPlayer == null);

		protected void SetOwner(GameObject targetObject = null)
		{
			if (targetObject == null)
				targetObject = gameObject;

			if (!IsOwner(targetObject))
				Networking.SetOwner(Networking.LocalPlayer, targetObject);
		}

		protected bool IsOwner(GameObject targetObject = null)
		{
			if (NotOnline)
				return false;

			if (targetObject == null)
				targetObject = gameObject;

			return Networking.LocalPlayer.IsOwner(targetObject);
		}

		protected VRCPlayerApi[] Players => VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);

		protected VRCPlayerApi GetPlayerByName(string targetName)
		{
			VRCPlayerApi[] players = Players;

			foreach (var player in players)
				if (player.displayName == targetName)
					return player;

			return null;
		}

		protected string LocalPlayerName => Networking.LocalPlayer.displayName;
		protected bool IsLocalPlayerVR => Networking.LocalPlayer.IsUserInVR();

		protected bool IsLocalPlayer(VRCPlayerApi player) => !NotOnline && (Networking.LocalPlayer == player);
		protected bool IsLocalPlayerID(int id) => !NotOnline && (Networking.LocalPlayer.playerId == id);
		protected bool IsLocalPlayerName(string targetName) => !NotOnline && (Networking.LocalPlayer.displayName == targetName);

		protected bool IsDigit(string s)
		{
			if (string.IsNullOrEmpty(s))
				return false;

			s = s.TrimEnd('\n', ' ', (char)8203);
			if (string.IsNullOrEmpty(s))
				return false;

			foreach (var c in s)
				if (!char.IsDigit(c))
					return false;

			return true;
		}

		protected bool IsLocalPlayerHolding(VRC_Pickup pickup)
		{
			if (NotOnline)
				return false;

			return Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left) == pickup ||
				   Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right) == pickup;
		}

		protected void TP(Transform tr) => Networking.LocalPlayer.TeleportTo(tr.position, tr.rotation);
		#endregion

		#region Wakgood
		protected string WakgoodName => "VRwakgood";
		protected bool IsLocalWakgood => Networking.LocalPlayer.displayName.Equals(WakgoodName);
		protected VRCPlayerApi Wakgood => GetPlayerByName(WakgoodName);
		#endregion
	}
}