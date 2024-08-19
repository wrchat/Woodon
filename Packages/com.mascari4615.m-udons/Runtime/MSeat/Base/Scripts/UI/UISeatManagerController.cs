using TMPro;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UISeatManagerController : MBase
	{
		[Header("_" + nameof(UISeatManagerController))]
		[SerializeField] private MTurnSeatManager turnSeatManager;
		[SerializeField] private TextMeshProUGUI curStateText;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			turnSeatManager.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
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
			curStateText.text = turnSeatManager.StateToString[turnSeatManager.CurGameState];
		}
	}
}