namespace WRC.Woodon
{
	public static class QuizUtil
	{
		public static string ToFriendlyString(this QuizContentState state)
		{
			switch (state)
			{
				case QuizContentState.Wait:
					return "대기";
				case QuizContentState.ShowQuiz:
					return "문제 공개";
				case QuizContentState.SelectAnswer:
					return "답 선택";
				case QuizContentState.ShowPlayerAnswer:
					return "플레이어 답 보기";
				case QuizContentState.CheckAnswer:
					return "답 확인";
				case QuizContentState.Explaining:
					return "설명";
				case QuizContentState.Scoring:
					return "점수 매기기";
				default:
					return "ERROR";
			}
		}
	}
}