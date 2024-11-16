using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIContentManager : MBase
	{
		[Header("_" + nameof(UIContentManager))]
		[SerializeField] private ContentManager contentManager;
		[SerializeField] private TextMeshProUGUI curStateText;
		[SerializeField] private string[] stateToString;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			contentManager.RegisterListener(this, nameof(UpdateUI));
			UpdateUI();
		}

		public void PrevState()
		{
			contentManager.SetGameState(contentManager.CurGameState - 1);
		}

		public void NextState()
		{
			contentManager.SetGameState(contentManager.CurGameState + 1);
		}

		public void UpdateUI()
		{
			if (contentManager.CurGameState < 0 || contentManager.CurGameState >= stateToString.Length)
			{
				curStateText.text = contentManager.CurGameState.ToString();
			}
			else
			{
				curStateText.text = stateToString[contentManager.CurGameState];
			}
		}
	}
}