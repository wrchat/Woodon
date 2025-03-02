using UdonSharp;
using UnityEngine;
using static WRC.Woodon.WUtil;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-10000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class QuizManager : ContentManager
	{
		[Header("_" + nameof(QuizManager))]
		[SerializeField] protected int playerCount = 10;
		[SerializeField] protected WInt curQuizIndex;
		[SerializeField] private Transform wrongPos;
		[SerializeField] private Transform[] quizDataParents;
		[SerializeField] protected WInt quizDataParentsIndex;
		[SerializeField] private MString seatIndexInputField;

		[field: Header("_" + nameof(QuizManager) + "_GameRule")]
		[field: SerializeField] public bool AddScoreWhenCorrectAnswer { get; private set; } = false;
		[field: SerializeField] public bool SubScoreWhenWrongAnswer { get; private set; } = false;
		[field: SerializeField] public bool DropPlayerWhenWrongAnswer { get; private set; } = false;
		[field: SerializeField] public bool DropPlayerWhenZeroScore { get; private set; } = false;
		[field: SerializeField] public bool HACK_CanSelectAnswerAnyTime { get; private set; } = false;

		protected int[] answerCount = new int[(int)QuizAnswerType.None + 1];

		public QuizData[] QuizDatas { get; private set; }
		public int CurQuizIndex => curQuizIndex.Value;
		public QuizData CurQuizData => QuizDatas[CurQuizIndex];

		public bool CanSelectAnswer { get; protected set; } = true;

		protected override void Init()
		{
			contentStateMax = (int)QuizContentState.Scoring;

			QuizDatas = quizDataParents[0].GetComponentsInChildren<QuizData>();

			base.Init();

			curQuizIndex.RegisterListener(this, nameof(OnQuizIndexChange));
			curQuizIndex.SetMinMaxValue(0, QuizDatas.Length - 1);
			OnQuizIndexChange();

			if (quizDataParentsIndex != null)
			{
				quizDataParentsIndex.RegisterListener(this, nameof(OnQuizDataParentChange));
				quizDataParentsIndex.SetMinMaxValue(0, quizDataParents.Length - 1);
				OnQuizDataParentChange();
			}
		}

		public override void UpdateContent()
		{
			CalcAnswerCount();

			base.UpdateContent();
		}

		private void CalcAnswerCount()
		{
			answerCount = new int[(int)QuizAnswerType.None + 1];
			foreach (QuizSeat seat in Seats)
			{
				if ((int)seat.ExpectedAnswer < 0 || (int)seat.ExpectedAnswer >= answerCount.Length)
					continue;

				answerCount[(int)seat.ExpectedAnswer]++;
			}
		}

		protected override void OnContentStateChange(DataChangeState changeState)
		{
			// if (changeState != DataChangeState.Less)
			{
				if (ContentState == (int)QuizContentState.Wait) OnWait();
				else if (ContentState == (int)QuizContentState.ShowQuiz) OnQuizTime();
				else if (ContentState == (int)QuizContentState.SelectAnswer) OnSelectAnswer();
				else if (ContentState == (int)QuizContentState.ShowPlayerAnswer) OnShowPlayerAnswer();
				else if (ContentState == (int)QuizContentState.CheckAnswer) OnCheckAnswer();
				else if (ContentState == (int)QuizContentState.Explaining) OnExplaining();
				else if (ContentState == (int)QuizContentState.Scoring) OnScoring();
			}

			base.OnContentStateChange(changeState);
		}

		public virtual void OnQuizIndexChange()
		{
			UpdateContent();
			SendEvents();
		}

		public virtual void OnQuizDataParentChange()
		{
			if (quizDataParentsIndex == null)
			{
				WDebugLog($"{nameof(OnQuizDataParentChange)} : {nameof(quizDataParentsIndex)}가 null입니다.", LogType.Error);
				return;
			}

			QuizDatas = quizDataParents[quizDataParentsIndex.Value].GetComponentsInChildren<QuizData>();
			curQuizIndex.SetMinMaxValue(0, QuizDatas.Length - 1);
			curQuizIndex.SetValue(0);
		}

		public void TeleportToSeat()
		{
			if (!IsDigit(seatIndexInputField.Value))
				return;

			int seatIndex = int.Parse(seatIndexInputField.Value);

			if (0 < seatIndex && seatIndex <= playerCount)
				TP(Seats[seatIndex - 1].transform);
		}

		public void TP_WrongPos()
		{
			if (wrongPos)
				TP(wrongPos);
		}

		public virtual void OnWait()
		{
			WDebugLog($"{nameof(OnWait)}");

			if (IsOwner() == false)
				return;

			foreach (MSeat seat in Seats)
				seat.ResetTurnData();
		}

		public virtual void OnQuizTime() => WDebugLog($"{nameof(OnQuizTime)}");
		public virtual void OnSelectAnswer() => WDebugLog($"{nameof(OnSelectAnswer)}");
		public virtual void OnShowPlayerAnswer() => WDebugLog($"{nameof(OnShowPlayerAnswer)}");
		public virtual void OnCheckAnswer() => WDebugLog($"{nameof(OnCheckAnswer)}");
		public virtual void OnExplaining() => WDebugLog($"{nameof(OnExplaining)}");
		public virtual void OnScoring() => WDebugLog($"{nameof(OnScoring)}");

		public override string GetContentStateString()
		{
			return ((QuizContentState)ContentState).ToFriendlyString();
		}

		public void SetContentState_Wait() => SetContentState(QuizContentState.Wait);
		public void SetContentState_ShowQuiz() => SetContentState(QuizContentState.ShowQuiz);
		public void SetContentState_SelectAnswer() => SetContentState(QuizContentState.SelectAnswer);
		public void SetContentState_ShowPlayerAnswer() => SetContentState(QuizContentState.ShowPlayerAnswer);
		public void SetContentState_CheckAnswer() => SetContentState(QuizContentState.CheckAnswer);
		public void SetContentState_Explaining() => SetContentState(QuizContentState.Explaining);
		public void SetContentState_Scoring() => SetContentState(QuizContentState.Scoring);
	}
}