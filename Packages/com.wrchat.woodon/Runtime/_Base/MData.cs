using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MData : MBase
	{
		[field: Header("_" + nameof(MData))]
		[field: SerializeField] public string Name { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string Value { get; set; } = NONE_STRING;
		[field: SerializeField] public Sprite Sprite { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string[] StringData { get; set; }
		[field: SerializeField] public Sprite[] Sprites { get; set; }

		public int Index { get; set; } = NONE_INT;
		public string RuntimeData { get; set; } = string.Empty;

		public virtual string Save()
		{
			string data = string.Empty;

			data += $"{RuntimeData}{DATA_SEPARATOR}";

			return data;
		}

		public virtual void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);

			RuntimeData = datas[0];
		}
	}
}