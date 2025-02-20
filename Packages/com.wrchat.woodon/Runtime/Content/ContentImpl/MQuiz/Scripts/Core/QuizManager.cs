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
		[SerializeField] protected MValue curQuizIndex;
		[SerializeField] private Transform wrongPos;
		[SerializeField] private Transform[] quizDataParents;
		[SerializeField] protected MValue quizDataParentsIndex;
		[SerializeField] private MString seatIndexInputField;

		[field: Header("_" + nameof(QuizManager) + "_GameRule")]
		[field: SerializeField] public bool AddScoreWhenCorrectAnswer { get; private set; } = false;
		[field: SerializeField] public bool SubScoreWhenWrongAnswer { get; private set; } = false;
		[field: SerializeField] public bool DropPlayerWhenWrongAnswer { get; private set; } = false;
		[field: SerializeField] public bool DropPlayerWhenZeroScore { get; private set; } = false;

		protected int[] answerCount = new int[10];

		public QuizData[] QuizDatas { get; private set; }

		public int CurQuizIndex => curQuizIndex.Value;

		public QuizData CurQuizData => QuizDatas[CurQuizIndex];
		public bool CanSelectAnswer { get; protected set; } = true;

		protected override void Start()
		{
			base.Start();
			UpdateContent();
		}

		protected override void Init()
		{
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
				MDebugLog($"{nameof(OnQuizDataParentChange)} : {nameof(quizDataParentsIndex)}가 null입니다.", LogType.Error);
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
			MDebugLog($"{nameof(OnWait)}");

			if (IsOwner() == false)
				return;

			foreach (MSeat turnSeat in Seats)
				turnSeat.ResetTurnData();
		}

		public virtual void OnQuizTime()
		{
			MDebugLog($"{nameof(OnQuizTime)}");
		}

		public virtual void OnSelectAnswer()
		{
			MDebugLog($"{nameof(OnSelectAnswer)}");
		}

		public virtual void OnShowPlayerAnswer()
		{
			MDebugLog($"{nameof(OnShowPlayerAnswer)}");
		}

		public virtual void OnCheckAnswer()
		{
			MDebugLog($"{nameof(OnCheckAnswer)}");
		}

		public virtual void OnExplaining()
		{
			MDebugLog($"{nameof(OnExplaining)}");
		}

		public virtual void OnScoring()
		{
			MDebugLog($"{nameof(OnScoring)}");
		}
	}
}