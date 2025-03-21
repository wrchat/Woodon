﻿using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class QuizData : WDataContainer
	{
		[field: Header("_" + nameof(QuizData))]
		[field: SerializeField] public QuizAnswerType QuizAnswer { get; set; } = QuizAnswerType.None;
		[field: TextArea(3, 10), SerializeField] public string QuizAnswerString { get; set; } = NONE_STRING;
		[field: TextArea(3, 10), SerializeField] public string NoteData { get; set; } = NONE_STRING;

		public string Quiz => Value;
		public Sprite[] AnswerSprites => Sprites;

		public bool Used { get; set; }

		public override void SerializeData()
		{
			wJson.SetData("Used", Used);
			base.SerializeData();
		}

		public override void ParseData()
		{
			base.ParseData();
			Used = wJson.DataDictionary["Used"].Boolean;
		}
	}
}