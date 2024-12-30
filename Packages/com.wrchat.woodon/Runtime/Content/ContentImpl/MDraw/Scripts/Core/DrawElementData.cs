using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class DrawElementData : MDataContainer
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
			TeamType = (TeamType)(int)wJson.GetData("TeamType").Double;
			Role = (DrawRole)(int)wJson.GetData("Role").Double;
			IsShowing = wJson.GetData("IsShowing").Boolean;

			// MDebugLog($"{nameof(ParseDataPack)}, Index : {Index}, TeamType : {TeamType}, Role : {Role}");
		}
	}
}