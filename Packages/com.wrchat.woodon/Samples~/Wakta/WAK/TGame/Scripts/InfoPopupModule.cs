using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class InfoPopupModule : UdonSharpBehaviour
	{
		[SerializeField] private Animator infoAnimator;
		[SerializeField] private TextMeshProUGUI[] infoTmps;
		[SerializeField] private Sprite[] sprites;
		[SerializeField] private Image[] images;

		[SerializeField] private string[] infoTexts;

		public void Popup(int infoIndex)
		{
			foreach (Image image in images)
				image.sprite = sprites[infoIndex];

			foreach (TextMeshProUGUI text in infoTmps)
				text.text = infoTexts[infoIndex];

			infoAnimator.SetTrigger("POP");
		}
	}
}