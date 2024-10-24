using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	public enum TGamePlayerUIState
	{
		None,
		Waiting,
		Safe,
		TakedCoin,
		Shooted,
		MinCoin,
		GreatThief,
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class TGameUIPlayerBlock : MBase
	{
		private readonly Color[] colorsByState = new Color[]
		{
			Color.gray, // None
			new Color(236f / 255f, 167f / 255f, 148f / 255f), // Waiting
			new Color(235f / 255f, 124f / 255f, 143f / 255f), // Safe
			new Color(0, 0, 0, 1 / 255f), // TakedCoin
			new Color(140/255f, 112/255f, 164/255f), // Shooted
			new Color(140/255f, 112/255f, 164/255f), // MinCoin
			new Color(237/255f, 213/255f, 58/255f), // GreatThief
		};

		private readonly Color diedCoinColor = new Color(0, 0, 0, 200f / 255f);

		[field: Header("_" + nameof(TGameUIPlayerBlock))]
		[SerializeField] private TextMeshProUGUI playerNameText;
		[SerializeField] private Image playerImage;
		[SerializeField] private Image diedImage;
		[SerializeField] private Image safeIcon;
		[SerializeField] private Image waitingIcon;
		[SerializeField] private Image coinTextBackground;
		[SerializeField] private TextMeshProUGUI coinText;
		[SerializeField] private Sprite nullPlayerSprite;

		private TGamePlayerUIState state;
		private bool showCoinAlltime = false;

		public void Init()
		{
			coinTextBackground.color = colorsByState[0];
			playerNameText.text = string.Empty;
			playerImage.sprite = nullPlayerSprite;
			diedImage.enabled = false;
			safeIcon.enabled = false;
			waitingIcon.enabled = false;
			coinText.text = "-";
			coinText.color = Color.black;
		}

		public void SetState(TGamePlayerUIState state)
		{
			this.state = state;
			UpdateUI();
		}

		public void SetPlayer(string playerName, Sprite playerSprite)
		{
			playerNameText.text = playerName;
			playerImage.sprite = playerSprite;
		}

		public void SetCoin(string coin)
		{
			coinText.text = coin;
		}

		public void SetShowCoinAlltime(bool showCoinAlltime)
		{
			this.showCoinAlltime = showCoinAlltime;
		}

		public void UpdateUI()
		{
			if ((showCoinAlltime == false) &&
				(state == TGamePlayerUIState.None || state == TGamePlayerUIState.TakedCoin))
				coinText.text = "-";

			coinText.enabled =
				state == TGamePlayerUIState.None ||
				state == TGamePlayerUIState.TakedCoin ||
				((int)state >= (int)TGamePlayerUIState.Shooted);

			if (state == TGamePlayerUIState.Shooted ||
				state == TGamePlayerUIState.MinCoin)
				coinText.color = diedCoinColor;
			else
				coinText.color = Color.black;

			coinTextBackground.color = colorsByState[(int)state];
			safeIcon.enabled = state == TGamePlayerUIState.Safe;
			waitingIcon.enabled = state == TGamePlayerUIState.Waiting;
			diedImage.enabled = state == TGamePlayerUIState.Shooted;
		}
	}
}