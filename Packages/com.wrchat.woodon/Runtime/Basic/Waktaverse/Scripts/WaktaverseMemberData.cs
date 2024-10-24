using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WaktaverseMemberData : MBase
	{
		[UdonSynced, FieldChangeCallback(nameof(SyncData))] private string _syncData;
		public string SyncData
		{
			get => _syncData;
			set
			{
				_syncData = value;
				waktaverseData.UpdateData();
			}
		}

		[field: Header("Setting")]
		[field: SerializeField] public bool Enable { get; private set; } = true;

		[field: Header("Member Data")]
		[field: SerializeField] public WaktaMember Member { get; private set; }
		[field: SerializeField] public Sprite Profile { get; private set; }

		[field: Header("Custom Data")]
		[field: SerializeField] public Sprite[] Sprites { get; private set; }
		[field: SerializeField] public string[] Strings { get; private set; }

		private WaktaverseData waktaverseData;

		public void Init(WaktaverseData waktaverseData)
		{
			this.waktaverseData = waktaverseData;
		}
		
		public void SetSyncData(string newSyncData)
		{
			SetOwner();
			SyncData = newSyncData;
			RequestSerialization();
		}
	}
}