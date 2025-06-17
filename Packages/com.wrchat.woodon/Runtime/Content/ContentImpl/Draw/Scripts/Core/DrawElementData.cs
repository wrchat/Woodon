using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DrawElementData : WDataContainer
	{
		[field: SerializeField] public TeamType InitTeamType { get; set; } = TeamType.None;
		[field: SerializeField] public DrawRole InitRole { get; set; } = DrawRole.None;

		public TeamType TeamType { get; set; } = TeamType.None;
		public DrawRole Role { get; set; } = DrawRole.None;
		public bool IsShowing { get; set; } = false;

		public override void SerializeData()
		{
			wJson.SetData("TeamType", (int)TeamType);
			wJson.SetData("Role", (int)Role);
			wJson.SetData("IsShowing", IsShowing);
			wJson.SetData("TeamType", (int)TeamType);
			base.SerializeData();
		}

		public override void ParseData()
		{
			base.ParseData();
			TeamType = (TeamType)wJson.GetData("TeamType", (int)TeamType);
			Role = (DrawRole)wJson.GetData("Role", (int)Role);
			IsShowing = wJson.GetData("IsShowing", IsShowing);

			// WDebugLog($"{nameof(ParseDataPack)}, Index : {Index}, TeamType : {TeamType}, Role : {Role}");
		}
	}
}