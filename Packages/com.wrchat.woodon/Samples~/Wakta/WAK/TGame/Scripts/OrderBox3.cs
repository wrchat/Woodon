using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class OrderBox3 : MBase
	{
		[Header("_" + nameof(OrderPanel3))]
		private int index;
		private OrderPanel3 orderPanel3;
		private TGameManager gameManager;
		private MSFXManager mSFXManager;
		private Animator animator;
		[SerializeField] private Image[] playerImages;
		[SerializeField] private Sprite nullPlayerSprite;

		[UdonSynced, FieldChangeCallback(nameof(IsShowing))] private bool isShowing;
		public bool IsShowing
		{
			get => isShowing;
			set
			{
				if (isShowing == false && value == true)
				{
					mSFXManager.PlaySFX_L(0);
					mSFXManager.PlaySFX_L(1);
				}

				isShowing = value;
				animator.SetBool("OPEN", value);
			}
		}

		public void Init(OrderPanel3 orderPanel3, TGameManager gameManager, MSFXManager mSFXManager, int index)
		{
			this.orderPanel3 = orderPanel3;
			this.gameManager = gameManager;
			this.mSFXManager = mSFXManager;
			this.index = index;
			animator = GetComponent<Animator>();
		}

		public void UpdateUI()
		{
			int nextRound = gameManager.Data.CurRound + 1;
			if (nextRound > 5)
				return;
			int targetPlayerIndex = gameManager.Data.RoundDatas[nextRound].NumberByOrder[index];
			if (targetPlayerIndex == NONE_INT)
				return;

			// 플레이어 이미지 열의 Sprite를 밑에서부터 하나씩 설정
			// 마지막에서 두 번째 이미지에 이 박스 순서의 플레이어 Sprite가 들어가야 함
			targetPlayerIndex++;
			for (int i = playerImages.Length - 1; i >= 0; i--)
				playerImages[i].sprite = gameManager.
					GetPlayerRouletteImage(Mathf.Abs(targetPlayerIndex - ((playerImages.Length - 1) - i)) % TGameManager.PLAYER_COUNT);

			playerImages[0].sprite = nullPlayerSprite;
		}

		public void Show()
		{
			SetOwner();
			IsShowing = true;
			RequestSerialization();
		}

		public void Hide()
		{
			SetOwner();
			IsShowing = false;
			RequestSerialization();
		}
	}
}
