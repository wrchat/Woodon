using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.HistoryOXQuiz
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class HistoryOXQuizManager : QuizManager
	{
		[Header("_" + nameof(HistoryOXQuizManager))]

		[SerializeField] private BoxCollider[] walls;
		[SerializeField] private BoxCollider[] oAreaColliders;
		[SerializeField] private BoxCollider[] middleColliders;
		[SerializeField] private BoxCollider[] xAreaColliders;
		private Bounds[] oBoundsArray;
		private Bounds[] middleBoundsArray;
		private Bounds[] xBoundsArray;

		protected override void Init()
		{
			oBoundsArray = new Bounds[oAreaColliders.Length];
			for (int i = 0; i < oBoundsArray.Length; i++)
				oBoundsArray[i] = oAreaColliders[i].bounds;

			middleBoundsArray = new Bounds[middleColliders.Length];
			for (int i = 0; i < middleBoundsArray.Length; i++)
				middleBoundsArray[i] = middleColliders[i].bounds;

			xBoundsArray = new Bounds[xAreaColliders.Length];
			for (int i = 0; i < xBoundsArray.Length; i++)
				xBoundsArray[i] = xAreaColliders[i].bounds;
			
			base.Init();
		}

		public bool IsPlayerInBounds(VRCPlayerApi player, Bounds[] targetBounds)
		{
			Vector3 playerPos = player.GetPosition();
			foreach (Bounds bounds in targetBounds)
			{
				if (bounds.Contains(playerPos))
					return true;
			}

			return false;
		}

		public override void OnWait()
		{
			base.OnWait();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(false);
		}

		public override void OnQuizTime()
		{
			base.OnQuizTime();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(false);
		}

		public override void OnSelectAnswer()
		{
			base.OnSelectAnswer();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(false);
		}

		public override void OnShowPlayerAnswer()
		{
			base.OnShowPlayerAnswer();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(true);
		}

		public override void OnCheckAnswer()
		{
			base.OnCheckAnswer();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(true);
		}

		public override void OnExplaining()
		{
			base.OnExplaining();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(true);
		}

		public override void OnScoring()
		{
			base.OnScoring();

			foreach (BoxCollider wall in walls)
				wall.gameObject.SetActive(false);

			QuizAnswerType answerType = CurQuizData.QuizAnswer;
			Bounds[] wrongBoundsArray = answerType == QuizAnswerType.O ? xBoundsArray : oBoundsArray;
			bool isLocalPlayerAtWrongPlace = IsPlayerInBounds(Networking.LocalPlayer, wrongBoundsArray);
			bool isLocalPlayerAtMiddle = IsPlayerInBounds(Networking.LocalPlayer, middleBoundsArray);

			bool isLocalPlayerStaff = RoleUtil.IsPlayerRole(RoleTag.Staff);

			if (isLocalPlayerStaff)
			{
			
			}
			else if (isLocalPlayerAtWrongPlace || isLocalPlayerAtMiddle)
			{
				// HACK:
				// TP_WrongPos();
				Networking.LocalPlayer.Respawn();
			}
			else
			{
			}
		}
	}
}