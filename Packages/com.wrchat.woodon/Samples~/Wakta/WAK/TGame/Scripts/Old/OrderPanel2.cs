using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class OrderPanel2 : MBase
	{
		[SerializeField] private TGameManager thiefGameManager;
		public TGameManager ThiefGameManager => thiefGameManager;
		[SerializeField] private OrderBox2[] orderBoxes;
		public OrderBox2[] OrderBoxes => orderBoxes;

		[SerializeField] private GameObject nextRoundButton;
		private Vector3 originPos;

		private void Start()
		{
			originPos = transform.position;
		}

		public void SetActive(bool active)
		{
			transform.position = originPos + (active ? Vector3.zero : Vector3.down * 100);
			UpdateUI();
		}

		public void Init()
		{
			SetOwner();
			Lock = false;
			RequestSerialization();

			foreach (OrderBox2 orderBox in orderBoxes)
				orderBox.Init();
		}

		public void UpdateUI()
		{
			bool everythingSelected = true;

			// orderBoxes[0].UpdateUI(true);
			// for (int i = 1; i < orderBoxes.Length; i++)
			//	orderBoxes[i].UpdateUI(orderBoxes[i - 1].TargetPlayerIndex != NONE_INT);

			foreach (OrderBox2 orderBox in orderBoxes)
			{
				orderBox.UpdateUI();

				if (orderBox.TargetPlayerIndex == NONE_INT)
					everythingSelected = false;
			}

			nextRoundButton.SetActive(everythingSelected);
			// nextRoundButton.SetActive(orderBoxes[ThiefGameManager.PLAYER_COUNT - 1].TargetPlayerIndex != NONE_INT);
		}

		public string GetOrderByString()
		{
			string order = string.Empty;
			Transform parent = transform.GetChild(0);

			for (int i = 0; i < orderBoxes.Length; i++)
			{
				string childName = parent.GetChild(i).gameObject.name;
				foreach (OrderBox2 orderBox in orderBoxes)
				{
					if (orderBox.name == childName)
					{
						order += orderBox.TargetPlayerIndex.ToString();
						break;
					}
				}
			}

			return order;
		}

		[UdonSynced, FieldChangeCallback(nameof(Lock))] private bool _lock;
		public bool Lock
		{
			get => _lock;
			set
			{
				_lock = value;
				lockObject.SetActive(_lock);
			}
		}

		[SerializeField] private GameObject lockObject;

		public void SetLock()
		{
			SetOwner();
			Lock = true;
			RequestSerialization();
		}
	}
}