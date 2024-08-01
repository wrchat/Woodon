using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MData : MBase
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public Sprite Sprite { get; private set; }
		[field: SerializeField, TextArea(1, 3)] public string[] StringData { get; private set; }
		[field: SerializeField] public Sprite[] Sprites { get; private set; }

		public int Index { get; set; } = NONE_INT;
		public string SyncData { get; set; } = string.Empty;

		public virtual string Save()
		{
			string data = string.Empty;

			data += $"{SyncData}{DATA_SEPARATOR}";

			return data;
		}

		public virtual void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);

			SyncData = datas[3];
		}
	}
}