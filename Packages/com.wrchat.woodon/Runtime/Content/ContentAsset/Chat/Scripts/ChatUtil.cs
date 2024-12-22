
using VRC.SDK3.Data;

namespace WRC.Woodon.Chat
{
	public static class ChatUtil
	{
		public static void UpdateChatData(this DataDictionary chatData, int time, string name, string message, int udonIndex, TeamType chatRoom, string additionalData = "")
		{
			chatData.Clear();

			chatData.Add("Time", time);
			chatData.Add("Name", name);
			chatData.Add("Message", message);
			chatData.Add("UdonIndex", udonIndex);
			int chatRoomInt = (int)chatRoom;
			chatData.Add("ChatRoom", chatRoomInt);
			chatData.Add("AdditionalData", additionalData);
		}

		public static int GetChatTime(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("Time", out DataToken dataToken))
				return (int)dataToken.Double;
		
			return 0;
		}

		public static string GetChatName(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("Name", out DataToken dataToken))
				return dataToken.String;
		
			return string.Empty;
		}

		public static string GetChatMessage(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("Message", out DataToken dataToken))
				return dataToken.String;
		
			return string.Empty;
		}

		public static int GetChatUdonIndex(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("UdonIndex", out DataToken dataToken))
				return (int)dataToken.Double;

			return MBase.NONE_INT;
		}

		public static TeamType GetChatRoom(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("ChatRoom", out DataToken dataToken))
			{
				int chatRoomInt = (int)dataToken.Double;
				return (TeamType)chatRoomInt;
			}

			return TeamType.None;
		}

		public static string GetChatAdditionalData(this DataDictionary chatData)
		{
			if (chatData.TryGetValue("AdditionalData", out DataToken dataToken))
				return dataToken.String;

			return string.Empty;
		}
	}
}