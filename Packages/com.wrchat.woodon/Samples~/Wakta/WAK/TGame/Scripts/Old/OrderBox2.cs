using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class OrderBox2 : MBase
	{
		[SerializeField] private int index;
		[SerializeField] private OrderPanel2 orderPanel2;
		[SerializeField] private GameObject dropBox;
		[SerializeField] private Image dropBoxStateImage;
		[SerializeField] private Sprite[] dropBoxStateSprites;

		[SerializeField] private Image selectedPlayerImage;
		[SerializeField] private Sprite nullPlayer;

		[SerializeField] private Image[] playerSelectBorders;
		[SerializeField] private Image[] playerFades;
		[SerializeField] private Button[] playerButtons;

		[UdonSynced, FieldChangeCallback(nameof(TargetPlayerIndex))]
		private int _targetPlayerIndex = NONE_INT;
		public int TargetPlayerIndex
		{
			get => _targetPlayerIndex;
			set
			{
				_targetPlayerIndex = value;

				orderPanel2.UpdateUI();
				// UpdateUI(dropBox.activeSelf);
			}
		}

		public void SetPlayerSprite(Sprite[] playerSprites)
		{
			for (int order = 0; order < TGameManager.PLAYER_COUNT; order++)
				playerButtons[order].image.sprite = playerSprites[order];
		}

		public void Init()
		{
			SetOwner();
			TargetPlayerIndex = NONE_INT;
			RequestSerialization();
		}

		// public void UpdateUI(bool active)
		public void UpdateUI()
		{
			// dropBox.SetActive(active);
			dropBoxStateImage.sprite = dropBoxStateSprites[dropBox.activeSelf ? 0 : 1];
			selectedPlayerImage.sprite = (_targetPlayerIndex == NONE_INT) ? nullPlayer : playerButtons[_targetPlayerIndex].image.sprite;

			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				playerSelectBorders[i].gameObject.SetActive(_targetPlayerIndex == i);

				playerFades[i].gameObject.SetActive(false);
				playerButtons[i].interactable = true;
			}

			// for (int i = 0; i < index; i++)
			for (int i = 0; i < TGameManager.PLAYER_COUNT; i++)
			{
				int curOrderBoxTargetPlayerIndex = orderPanel2.OrderBoxes[i].TargetPlayerIndex;
				bool alreadySelected = (curOrderBoxTargetPlayerIndex != NONE_INT);

				if ((i == index) || !alreadySelected)
					continue;

				playerFades[curOrderBoxTargetPlayerIndex].gameObject.SetActive(alreadySelected);
				playerButtons[curOrderBoxTargetPlayerIndex].interactable = (!alreadySelected);
			}
		}

		public void SelectPlayer(int targetPlayerIndex)
		{
			if (!orderPanel2.ThiefGameManager.IsLastPlayer)
				return;

			if (orderPanel2.ThiefGameManager.Data.CurRound != 0)
				if (targetPlayerIndex == orderPanel2.ThiefGameManager.Data.RoundDatas[orderPanel2.ThiefGameManager.Data.CurRound - 1].NumberByOrder[6])
					return;

			SetOwner();
			TargetPlayerIndex = TargetPlayerIndex == targetPlayerIndex ? NONE_INT : targetPlayerIndex;
			RequestSerialization();

			//for (int i = index + 1; i < ThiefGameManager.PLAYER_COUNT; i++)
			//	orderPanel2.OrderBoxes[i].Init();
		}

		public void SelectPlayer0() => SelectPlayer(0);
		public void SelectPlayer1() => SelectPlayer(1);
		public void SelectPlayer2() => SelectPlayer(2);
		public void SelectPlayer3() => SelectPlayer(3);
		public void SelectPlayer4() => SelectPlayer(4);
		public void SelectPlayer5() => SelectPlayer(5);
		public void SelectPlayer6() => SelectPlayer(6);
	}
}
