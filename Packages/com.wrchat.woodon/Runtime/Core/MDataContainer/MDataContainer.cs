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

		[SerializeField] protected MData mData;

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
			if (mData == null)
				return;

			mData.RegisterListener(this, nameof(ParseData), MDataEvent.OnDeserialization);
		}

		public virtual void SerializeData()
		{
			if (mData == null)
				return;

			mData.SetData("RuntimeInt", RuntimeInt);
			mData.SetData("RuntimeBool", RuntimeBool);
			mData.SetData("RuntimeString", RuntimeString);

			mData.SerializeData();
		}

		public virtual void ParseData()
		{
			if (mData == null)
				return;

			RuntimeInt = (int)mData.DataDictionary["RuntimeInt"].Double;
			RuntimeBool = mData.DataDictionary["RuntimeBool"].Boolean;
			RuntimeString = mData.DataDictionary["RuntimeString"].String;

			SendEvents();
		}
	}
}