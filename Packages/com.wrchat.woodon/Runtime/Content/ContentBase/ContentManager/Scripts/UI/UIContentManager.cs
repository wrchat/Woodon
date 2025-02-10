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
			contentManager.SetContentState(contentManager.ContentState - 1);
		}

		public void NextState()
		{
			contentManager.SetContentState(contentManager.ContentState + 1);
		}

		public void UpdateUI()
		{
			if (contentManager.ContentState < 0 || contentManager.ContentState >= stateToString.Length)
			{
				curStateText.text = contentManager.ContentState.ToString();
			}
			else
			{
				curStateText.text = stateToString[contentManager.ContentState];
			}
		}
	}
}