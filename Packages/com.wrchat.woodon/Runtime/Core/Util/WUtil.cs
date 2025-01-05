using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	// Util & Helper
	public static class WUtil
	{
		public static bool IsNotOnline() => Networking.LocalPlayer == null;

		public static bool IsDigit(string s)
		{
			if (string.IsNullOrEmpty(s))
				return false;

			s = s.TrimEnd('\n', ' ', (char)8203);
			if (string.IsNullOrEmpty(s))
				return false;

			foreach (char c in s)
				if (char.IsDigit(c) == false)
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

		#region Array
		public static void Resize<T>(ref T[] originArr, int size)
		{
			T[] newArr = new T[size];

			int copyLength = Math.Min(originArr.Length, size);
			Array.Copy(originArr, 0, newArr, 0, copyLength);

			for (int i = originArr.Length; i < size; i++)
				newArr[i] = default;

			originArr = newArr;
		}

		public static void Add<T>(ref T[] originArr, T element)
		{
			Resize(ref originArr, originArr.Length + 1);
			originArr[originArr.Length - 1] = element;
		}

		public static void AddRange<T>(ref T[] originArr, T[] elements)
		{
			Resize(ref originArr, originArr.Length + elements.Length);
			Array.Copy(elements, 0, originArr, originArr.Length - elements.Length, elements.Length);
		}

		public static void Remove<T>(ref T[] originArr, T element)
		{
			int index = Array.IndexOf(originArr, element);
			RemoveAt(ref originArr, index);
		}

		public static void RemoveAt<T>(ref T[] originArr, int index)
		{
			if ((index < 0) || (index >= originArr.Length))
				Debug.LogError($"{nameof(WUtil)}.{nameof(RemoveAt)} : Index out of range");

			T[] newArr = new T[originArr.Length - 1];
			if (index > 0)
				Array.Copy(originArr, 0, newArr, 0, index);

			if (index < originArr.Length - 1)
				Array.Copy(originArr, index + 1, newArr, index, originArr.Length - index - 1);
		}
		#endregion
	}
}