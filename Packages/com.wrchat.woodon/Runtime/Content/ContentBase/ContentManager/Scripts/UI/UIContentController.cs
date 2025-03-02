using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIContentController : UIContent
	{
		[Header("_" + nameof(UIContentController))]
		[SerializeField] private TextMeshProUGUI curStateText;
		[SerializeField] private string[] stateToStringOverride;

		public void PrevState()
		{
			contentManager.SetContentState(contentManager.ContentState - 1);
		}

		public void NextState()
		{
			contentManager.SetContentState(contentManager.ContentState + 1);
		}

		public override void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)}");

			// Override가 불가능하다면
			if ((contentManager.ContentState < 0) || (contentManager.ContentState >= stateToStringOverride.Length))
			{
				curStateText.text = contentManager.GetContentStateString();
			}
			else
			{
				curStateText.text = stateToStringOverride[contentManager.ContentState];
			}
		}
	}
}