using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MDataContainer : WEventPublisher
	{
		[field: Header("_" + nameof(MDataContainer))]
		[field: SerializeField] public string Name { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string Value { get; set; } = NONE_STRING;
		[field: SerializeField] public Sprite Sprite { get; set; }
		[field: SerializeField, TextArea(3, 10)] public string[] StringData { get; set; }
		[field: SerializeField] public Sprite[] Sprites { get; set; }

		[SerializeField] protected WJson wJson;

		public int RuntimeInt { get; set; } = NONE_INT;
		public bool RuntimeBool { get; set; } = false;
		public string RuntimeString { get; set; } = NONE_STRING;

		public int Index { get; set; } = NONE_INT;

		private void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			if (wJson == null)
				return;

			wJson.RegisterListener(this, nameof(ParseData), WJsonEvent.OnDeserialization);
		}

		public virtual void SerializeData()
		{
			if (wJson == null)
				return;

			wJson.SetData("RuntimeInt", RuntimeInt);
			wJson.SetData("RuntimeBool", RuntimeBool);
			wJson.SetData("RuntimeString", RuntimeString);

			wJson.SerializeData();
		}

		public virtual void ParseData()
		{
			if (wJson == null)
				return;

			RuntimeInt = (int)wJson.GetData("RuntimeInt").Double;
			RuntimeBool = wJson.GetData("RuntimeBool").Boolean;
			RuntimeString = wJson.GetData("RuntimeString").String;

			SendEvents();
		}

		public void Clear()
		{
			Name = string.Empty;
			Value = string.Empty;
			Sprite = null;
			StringData = null;
			Sprites = null;
		}
	}
}