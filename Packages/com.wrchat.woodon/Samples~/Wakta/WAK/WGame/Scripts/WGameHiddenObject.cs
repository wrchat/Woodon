using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.WGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WGameHiddenObject : MBase
	{
		[SerializeField] private Sprite[] playerIcons;
		[SerializeField] private Sprite nullIcon;

		[SerializeField] private GameObject button;
		[SerializeField] private GameObject button2;
		[SerializeField] private GameObject effect;
		[SerializeField] private Image image;
		[SerializeField] private TextMeshProUGUI nameText;

		[SerializeField] private WGameHiddenManager gameHiddenManager;

		public const int NO_ONE = -2;

		[UdonSynced, FieldChangeCallback(nameof(OwnerWaktaIndex))] private int ownerWaktaIndex = NO_ONE;
		public int OwnerWaktaIndex
		{
			get => ownerWaktaIndex;
			set
			{
				ownerWaktaIndex = value;
				OnOwnerChange();
			}
		}

		private void OnOwnerChange()
		{
			button.SetActive(ownerWaktaIndex == NO_ONE);
			button2.SetActive(ownerWaktaIndex != NO_ONE);
			effect.SetActive(ownerWaktaIndex != NO_ONE);

			if (ownerWaktaIndex != NO_ONE)
			{
				string nickname = Waktaverse.GetNickname(OwnerWaktaIndex);

				Sprite sprite = ownerWaktaIndex != NONE_INT ? playerIcons[ownerWaktaIndex] : nullIcon;
				string _name = ownerWaktaIndex != NONE_INT ? $"{nickname} 발견 !" : "이파리가 발견 !";

				// _name += "\n<size=15>이세돌 데뷔 2주년을 축하합니다 !";

				image.sprite = sprite;
				nameText.text = _name;
			}

			gameHiddenManager.TryOpenDoor();
		}

		private void Start()
		{
			OnOwnerChange();
		}

		public void Find()
		{
			SetOwner();
			OwnerWaktaIndex = Waktaverse.GetIndex(Networking.LocalPlayer.displayName);
			RequestSerialization();
		}
	}
}