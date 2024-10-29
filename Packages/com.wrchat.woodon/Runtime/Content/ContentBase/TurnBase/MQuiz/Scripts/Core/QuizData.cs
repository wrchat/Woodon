using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class QuizData : MData
	{
		public string Quiz => Value;
		public Sprite[] AnswerSprites => Sprites;

		[field: Header("_" + nameof(QuizData))]
		[field: SerializeField] public QuizAnswerType QuizAnswer { get; set; } = QuizAnswerType.None;
		[field: TextArea(3, 10), SerializeField] public string QuizAnswerString { get; set; } = NONE_STRING;
		[field: TextArea(3, 10), SerializeField] public string NoteData { get; set; } = NONE_STRING;

		public bool Used { get; set; }

		public override string Save()
		{
			string data = string.Empty;
			data += $"{Used}{DATA_SEPARATOR}";
			return data;
		}

		public override void Load(string data)
		{
			string[] datas = data.Split(DATA_SEPARATOR);
			Used = bool.Parse(datas[0]);
		}
	}
}