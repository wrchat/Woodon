using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UITurnBaseManagerController : MBase
	{
		[Header("_" + nameof(UITurnBaseManagerController))]
		[SerializeField] private MTurnBaseManager turnBaseManager;
		[SerializeField] private TextMeshProUGUI curStateText;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			turnBaseManager.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void PrevState()
		{
			turnBaseManager.SetGameState(turnBaseManager.CurGameState - 1);
		}

		public void NextState()
		{
			turnBaseManager.SetGameState(turnBaseManager.CurGameState + 1);
		}

		public void UpdateUI()
		{
			curStateText.text = turnBaseManager.StateToString[turnBaseManager.CurGameState];
		}
	}
}