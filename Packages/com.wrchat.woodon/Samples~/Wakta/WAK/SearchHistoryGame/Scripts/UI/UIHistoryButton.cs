using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.Wak.SearchHistoryGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIHistoryButton : UdonSharpBehaviour
	{
		[SerializeField] private Image profile;
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private CanvasGroup noneCanvasGroup;
		[SerializeField] private CanvasGroup dataCanvasGroup;

		public void SetData(WaktaverseMemberData data, int index)
		{
			profile.sprite = data.Sprites[index];
			nameText.text = data.Strings[index];
		}

		public void SetEnabled(bool enabled)
		{
			noneCanvasGroup.alpha = enabled ? 0 : 1;
			noneCanvasGroup.blocksRaycasts = !enabled;
			noneCanvasGroup.interactable = !enabled;

			dataCanvasGroup.alpha = enabled ? 1 : 0;
			dataCanvasGroup.blocksRaycasts = enabled;
			dataCanvasGroup.interactable = enabled;
		}
	}
}