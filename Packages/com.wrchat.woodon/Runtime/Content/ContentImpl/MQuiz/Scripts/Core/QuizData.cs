using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class QuizData : MDataContainer
	{
		public string Quiz => Value;
		public Sprite[] AnswerSprites => Sprites;

		[field: Header("_" + nameof(QuizData))]
		[field: SerializeField] public QuizAnswerType QuizAnswer { get; set; } = QuizAnswerType.None;
		[field: TextArea(3, 10), SerializeField] public string QuizAnswerString { get; set; } = NONE_STRING;
		[field: TextArea(3, 10), SerializeField] public string NoteData { get; set; } = NONE_STRING;

		public bool Used { get; set; }

		public override void SerializeData()
		{
			mData.SetData("Used", Used);
			base.SerializeData();
		}

		public override void ParseData()
		{
			base.ParseData();
			Used = mData.DataDictionary["Used"].Boolean;
		}
	}
}