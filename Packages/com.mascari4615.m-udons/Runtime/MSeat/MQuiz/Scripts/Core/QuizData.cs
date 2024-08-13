using UdonSharp;
using UnityEngine;

namespace Mascari4615
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
	}
}