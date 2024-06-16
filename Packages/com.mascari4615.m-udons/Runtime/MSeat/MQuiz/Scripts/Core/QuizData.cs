
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public enum QuizAnswerType
	{
		O,
		X,
		One,
		Two,
		Three,
		Four,
		Five,
		String,
		ManyAnswer,
		None
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class QuizData : MBase
	{
		[field: Header("_" + nameof(QuizData))]
		[field: TextArea(3, 10), SerializeField] public string Quiz { get; set; } = NONE_STRING;
		[field: SerializeField] public Sprite QuizSprite { get; set; }
		[field: SerializeField] public Sprite[] AnswerSprites { get; set; }
		[field: SerializeField] public QuizAnswerType QuizAnswer { get; set; } = QuizAnswerType.None;
		[field: TextArea(3, 10), SerializeField] public string QuizAnswerString { get; set; } = NONE_STRING;
		[field: TextArea(3, 10), SerializeField] public string NoteData { get; set; } = NONE_STRING;
	}
}