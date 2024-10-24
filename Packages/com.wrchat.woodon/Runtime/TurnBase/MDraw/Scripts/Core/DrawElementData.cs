using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DrawElementData : MData
	{
		[field: SerializeField] public TeamType InitTeamType { get; private set; } = TeamType.None;
		[field: SerializeField] public DrawRole InitRole { get; private set; } = DrawRole.None;

		public TeamType TeamType { get; set; } = TeamType.None;
		public DrawRole Role { get; set; } = DrawRole.None;
		public bool IsShowing { get; set; } = false;

		public override string Save()
		{
			string data = string.Empty;

			data += $"{(int)TeamType}{DATA_SEPARATOR}";
			data += $"{(int)Role}{DATA_SEPARATOR}";
			data += $"{IsShowing}{DATA_SEPARATOR}";
			data += $"{RuntimeData}{DATA_SEPARATOR}";

			return data;
		}

		public override void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);

			TeamType = (TeamType)int.Parse(datas[0]);
			Role = (DrawRole)int.Parse(datas[1]);
			IsShowing = bool.Parse(datas[2]);
			RuntimeData = datas[3];

			// MDebugLog($"{nameof(ParseDataPack)}, Index : {Index}, TeamType : {TeamType}, Role : {Role}");
		}
	}
}