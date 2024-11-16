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
			mData.SetData("TeamType", (int)TeamType);
			mData.SetData("Role", (int)Role);
			mData.SetData("IsShowing", IsShowing);
			mData.SetData("TeamType", (int)TeamType);
			base.SerializeData();
		}

		public override void ParseData()
		{
			base.ParseData();
			TeamType = (TeamType)(int)mData.DataDictionary["TeamType"].Double;
			Role = (DrawRole)(int)mData.DataDictionary["Role"].Double;
			IsShowing = mData.DataDictionary["IsShowing"].Boolean;

			// MDebugLog($"{nameof(ParseDataPack)}, Index : {Index}, TeamType : {TeamType}, Role : {Role}");
		}
	}
}