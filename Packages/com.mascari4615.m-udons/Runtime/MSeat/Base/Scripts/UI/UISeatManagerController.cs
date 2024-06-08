using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatManagerController : MBase
	{
		[SerializeField] private TextMeshProUGUI curStateText;
		private MTurnSeatManager turnSeatManager;
		private bool isInit;

		public void Init(MTurnSeatManager turnSeatManager)
		{
			if (isInit)
				return;
			isInit = true;

			this.turnSeatManager = turnSeatManager;
		}

		public void PrevState()
		{
			turnSeatManager.SetGameState(turnSeatManager.CurGameState - 1);
		}

		public void NextState()
		{
			turnSeatManager.SetGameState(turnSeatManager.CurGameState + 1);
		}

		public void UpdateUI()
		{
			if (isInit == false)
				return;

			curStateText.text = turnSeatManager.StateToString[turnSeatManager.CurGameState];
		}
	}
}