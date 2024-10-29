using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	// Util & Helper
	public static class MUtil
	{
		public static bool IsNotOnline() => Networking.LocalPlayer == null;

		public static bool IsDigit(string s)
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

		public static VRCPlayerApi[] GetPlayers() => VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);

		public static bool IsPlayerHolding(VRCPlayerApi targetPlayer, VRC_Pickup pickup)
		{
			if (IsNotOnline())
				return false;

			return pickup.currentPlayer == targetPlayer;

			// return targetPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left) == pickup ||
			// 	   targetPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right) == pickup;
		}

		public static VRCPlayerApi GetPlayerByName(string targetName)
		{
			VRCPlayerApi[] players = GetPlayers();

			foreach (VRCPlayerApi player in players)
				if (player.displayName == targetName)
					return player;

			return null;
		}

		public static bool IsPlayerVR(VRCPlayerApi playerApi) => playerApi.IsUserInVR();

		public static bool IsLocalPlayer(VRCPlayerApi player) => !IsNotOnline() && (Networking.LocalPlayer == player);
		public static bool IsLocalPlayerID(int id) => !IsNotOnline() && (Networking.LocalPlayer.playerId == id);
		public static bool IsLocalPlayerName(string targetName) => !IsNotOnline() && (Networking.LocalPlayer.displayName == targetName);

		public static void TP(Transform tr) => Networking.LocalPlayer.TeleportTo(tr.position, tr.rotation);
		public static void TP(Vector3 pos, Quaternion rot = default) => Networking.LocalPlayer.TeleportTo(pos, rot);

		public static void SetCanvasGroupActive(CanvasGroup canvasGroup, bool active)
		{
			canvasGroup.alpha = active ? 1 : 0;
			canvasGroup.blocksRaycasts = active;
			canvasGroup.interactable = active;
		}
	}
}