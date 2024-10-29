using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WaktaverseData : MEventSender
	{
		private WaktaverseMemberData[] _datas;
		public WaktaverseMemberData[] Datas
		{
			get
			{
				if (_isInit == false)
					Init();
				return _datas;
			}
		}

		public static WaktaverseData GetInstance()
		{
			return GameObject.Find(nameof(WaktaverseData)).GetComponent<WaktaverseData>();
		}

		public WaktaverseMemberData GetData(WaktaMember waktaMember)
		{
			foreach (WaktaverseMemberData data in Datas)
				if (data.Member == waktaMember)
					return data;

			return null;
		}

		private bool _isInit = false;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (_isInit)
				return;

			_isInit = true;

			_datas = GetComponentsInChildren<WaktaverseMemberData>(true);
			foreach (WaktaverseMemberData data in Datas)
				data.Init(this);
		}

		public void UpdateData()
		{
			SendEvents();
		}
	}
}