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
	public class CoinBox : UdonSharpBehaviour
	{
		private readonly Color originColor = new Color(137 / 255f, 137 / 255f, 137 / 255f);
		private readonly Color negativeColor = new Color(171 / 255f, 61 / 255f, 61 / 255f);
		private readonly Color selectedColor = new Color(210 / 255f, 180 / 255f, 110 / 255f);
		private readonly Color lockColor = new Color(77 / 255f, 77 / 255f, 77 / 255f);

		[Header("_" + nameof(CoinBox))]
		[SerializeField] private bool isRemainCoinBox;
		[SerializeField] private Image background;
		[SerializeField] private int index;
		[SerializeField] private CoinMemoPanel coinMemoPanel;
		[SerializeField] private TextMeshProUGUI coinText;

		public int Coin { get; private set; }
		public bool IsLock { get; set; }

		public void UpdateCoin(int newCoin)
		{
			Coin = newCoin;
			coinText.text = Coin.ToString();
		}

		public void Select()
		{
			if (IsLock)
				return;

			coinMemoPanel.SetCurCoinBox(isRemainCoinBox, index);
		}

		public void UpdateColor(bool select)
		{
			if (isRemainCoinBox)
			{
				if (IsLock)
				{
					background.color = lockColor;
				}
				else
				{
					background.color = Coin >= 0 ? originColor : negativeColor;
				}
			}
			else
			{
				if (IsLock)
				{
					background.color = lockColor;
				}
				else if (select)
				{
					background.color = selectedColor;
				}
				else
				{
					background.color = Coin >= 0 ? originColor : negativeColor;
				}
			}
		}
	}
}