using TMPro;
using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615
{
	public class MTextManager : UdonSharpBehaviour
	{
		private const string MONEY_CHANGE_POPUP_TRIGGER = "POPUP";
		private const string MONEY_SHOW_TRIGGER = "SHOW";

		[Header("OverlayUI")][SerializeField] private TextMeshProUGUI addText;

		[SerializeField] private TextMeshProUGUI moneyText;
		[SerializeField] private Animator overlayAnimator;
		[SerializeField] private float duration;
		private float curValue;
		private bool flag;
		private float offset;
		private float targetValue;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.M)) overlayAnimator.SetTrigger(MONEY_SHOW_TRIGGER);

			if (flag)
			{
				curValue += offset * Time.deltaTime;

				if (offset > 0)
				{
					if (curValue >= targetValue)
					{
						curValue = targetValue;
						flag = false;
					}
				}
				else
				{
					if (curValue <= targetValue)
					{
						curValue = targetValue;
						flag = false;
					}
				}

				moneyText.text = ((int)curValue).ToString();
			}
		}

		public void SetText(int origin, int target)
		{
			flag = false;

			curValue = origin;
			moneyText.text = origin.ToString();
			targetValue = target;
			addText.text = string.Concat(target - origin > 0 ? "+" : "", (target - origin).ToString());
			addText.color = target - origin > 0 ? Color.green : Color.red;
			offset = (target - origin) / duration;
			overlayAnimator.SetTrigger(MONEY_CHANGE_POPUP_TRIGGER);
		}

		public void StartCountingAnimation()
		{
			flag = true;
		}
	}
}