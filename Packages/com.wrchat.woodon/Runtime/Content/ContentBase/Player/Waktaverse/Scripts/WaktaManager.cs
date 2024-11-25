using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WaktaManager : WEventPublisher
	{
		[field: Header("_" + nameof(WaktaManager))]
		[field: SerializeField] public WaktaMemberData[] Datas { get; set; }

		public static WaktaManager GetInstance()
		{
			return GameObject.Find(nameof(WaktaManager)).GetComponent<WaktaManager>();
		}

		public WaktaMemberData GetData(WaktaMember waktaMember)
		{
			foreach (WaktaMemberData data in Datas)
				if (data.Member == waktaMember)
					return data;

			return null;
		}

		public WaktaMemberData GetData(int index)
		{
			return Datas[index];
		}

		public WaktaMember[] GetEnabledMembersByType(WaktaMemberType type)
		{
			WaktaMember[] members = WaktaUtil.GetMembersByType(type);
			
			int enabledMemberCount = 0;
			foreach (WaktaMember member in members)
				if (Datas[(int)member].RuntimeBool)
					enabledMemberCount++;

			WaktaMember[] enabledMembers = new WaktaMember[enabledMemberCount];

			int enabledMemberIndex = 0;
			foreach (WaktaMember member in members)
				if (Datas[(int)member].RuntimeBool)
					enabledMembers[enabledMemberIndex++] = member;

			return enabledMembers;
		}

		public int GetMemberIndex(string name)
		{
			WaktaMember member = WaktaUtil.GetWaktaMember(name);

			if (member != WaktaMember.None)
				return (int)member;


			for (int i = 0; i < Datas.Length; i++)
			{
				WaktaMemberData data = Datas[i];

				if ((name == data.Nickname) ||
					(name == data.DisplayName))
					return i;
			}

			return NONE_INT;
		}
	}
}