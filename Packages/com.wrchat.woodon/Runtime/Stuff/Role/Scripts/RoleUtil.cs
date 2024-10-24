using VRC.SDKBase;

namespace WRC.Woodon
{
	// Util & Helper
	public static class RoleUtil
	{
		public const string ROLE_TAG_KEY = "ROLE";
	
		public static RoleTag GetPlayerRole(VRCPlayerApi player = null)
		{
			if (player == null)
				player = Networking.LocalPlayer;

			string tag = player.GetPlayerTag(ROLE_TAG_KEY);

			if (string.IsNullOrEmpty(tag))
				return RoleTag.None;

			return (RoleTag)int.Parse(tag);
		}

		public static void SetPlayerTag(RoleTag tag, VRCPlayerApi player = null)
		{
			if (player == null)
				player = Networking.LocalPlayer;

			player.SetPlayerTag(ROLE_TAG_KEY, $"{tag}");
		}

		public static bool IsPlayerRole(RoleTag tag, VRCPlayerApi player = null)
		{
			if (player == null)
				player = Networking.LocalPlayer;

			return player.GetPlayerTag(ROLE_TAG_KEY) == $"{tag}";
		}
	}
}