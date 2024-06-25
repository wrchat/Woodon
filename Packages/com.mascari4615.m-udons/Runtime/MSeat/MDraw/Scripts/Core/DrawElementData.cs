using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DrawElementData : MBase
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public Sprite Sprite { get; private set; }

		[field: SerializeField] public TeamType InitTeamType { get; private set; } = TeamType.None;
		[field: SerializeField] public DrawRole InitRole { get; private set; } = DrawRole.None;
		[field: SerializeField, TextArea(1, 3)] public string[] StringData { get; private set; }

		public int Index { get; set; } = NONE_INT;
		public TeamType TeamType { get; set; } = TeamType.None;
		public DrawRole Role { get; set; } = DrawRole.None;
		public string SyncData { get; set; } = string.Empty;

		public bool IsShowing { get; set; } = false;

		public string Save()
		{
			string data = string.Empty;

			data += $"{(int)TeamType}{DATA_SEPARATOR}";
			data += $"{(int)Role}{DATA_SEPARATOR}";
			data += $"{IsShowing}{DATA_SEPARATOR}";
			data += $"{SyncData}{DATA_SEPARATOR}";

			return data;
		}

		public void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);

			TeamType = (TeamType)int.Parse(datas[0]);
			Role = (DrawRole)int.Parse(datas[1]);
			IsShowing = bool.Parse(datas[2]);
			SyncData = datas[3];

			// MDebugLog($"{nameof(ParseDataPack)}, Index : {Index}, TeamType : {TeamType}, Role : {Role}");
		}
	}
}