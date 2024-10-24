using TMPro;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.VII.QGame
{
	public class QGameSeat : QuizSeat
	{
		[Header("_" + nameof(QGameSeat))]
		[SerializeField] private MeshRenderer tile;

		[SerializeField] private Material tileSelectAnswerMaterial;
		[SerializeField] private Material tileOMaterial;
		[SerializeField] private Material tileXMaterial;
		[SerializeField] private Material tileAliveMaterial;
		[SerializeField] private Material tileDefaultMaterial;

		[SerializeField] private TextMeshProUGUI timeText;
		private float t = 0;

		public override void UpdateStuff()
		{
			base.UpdateStuff();

			// 초기화
			tile.material = tileDefaultMaterial;

			// 빈자리라면 그대로
			if (TargetPlayerID == NONE_INT)
				return;

			// 상황에 따라 타일 Material 바꾸기
			switch (QuizManager.CurGameState)
			{
				case (int)QuizGameState.Wait:
				case (int)QuizGameState.ShowQuiz:
				case (int)QuizGameState.SelectAnswer:
					tile.material =
						HasSelectedAnswer ?
						tileSelectAnswerMaterial :
						tileAliveMaterial;
					break;
				case (int)QuizGameState.ShowPlayerAnswer:
				case (int)QuizGameState.CheckAnswer:
				case (int)QuizGameState.Scoring:
					if (HasSelectedAnswer)
					{
						tile.material =
							(ExpectedAnswer == QuizAnswerType.O) ?
							tileOMaterial :
							tileXMaterial;
					}
					break;
			}
		}

		private void Update()
		{
			if (t >= 0)
				t -= Time.deltaTime;

			// timeText.text = t <= 0 ? "-" : Mathf.CeilToInt(t + 1).ToString();
			timeText.text = t <= 0 ? "-" : t.ToString("F1");
		}

		public override void OnSelectAnswer()
		{
			t = 15;
		}
	}
}