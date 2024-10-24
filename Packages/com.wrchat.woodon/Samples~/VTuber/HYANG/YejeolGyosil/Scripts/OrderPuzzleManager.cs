using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class OrderPuzzleManager : MBase
	{
		[field: Header("_" + nameof(OrderPuzzleManager))]
		[field: SerializeField] public Transform[] Positions { get; private set; }
		[SerializeField] private OrderPuzzlePickup[] pickups;
	
		private int holdingIndex = NONE_INT;
		private int[] lastOrders = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		
		[UdonSynced, FieldChangeCallback(nameof(OrderSync))] private string _orderSync;
		public string OrderSync
		{
			get => _orderSync;
			private set
			{
				MDebugLog($"OrderSync: {OrderSync} -> {value}");
				_orderSync = value;

				if (string.IsNullOrEmpty(value))
					return;

				string[] split = value.Split(DATA_SEPARATOR);
				for (int i = 0; i < pickups.Length; i++)
					pickups[i].SetOrder(int.Parse(split[i]));
			}
		}

		private void SyncOrder()
		{
			// lastOrderString = string.Join(DATA_SEPARATOR.ToString(), lastOrders);

			string str = string.Empty;
			for (int i = 0; i < pickups.Length; i++)
			{
				str += pickups[i].Order;
				if (i < pickups.Length - 1)
					str += DATA_SEPARATOR;
			}

			if (string.IsNullOrEmpty(str))
			{
				// MDebugLog("Empty OrderSync");
				return;
			}
			
			if (OrderSync == str)
			{
				// MDebugLog("Same OrderSync");
				return;
			}

			SetOwner();
			OrderSync = str;
			// RequestSerialization();
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			// pickups = GetComponentsInChildren<OrderPuzzlePickup>(true);
			for (int i = 0; i < pickups.Length; i++)
				pickups[i].Init(this, i);
		}

		public void StartPickup(int index, int order)
		{
			SetOwner();

			holdingIndex = index;
			lastOrders = new int[pickups.Length - 1];

			int curOrder = 0;
			if (curOrder == order)
				curOrder++;

			// 순서
			for (int i = 0; i < lastOrders.Length; i++)
			{
				for (int j = 0; j < pickups.Length; j++)
				{
					if (pickups[j].Order == curOrder)
					{
						lastOrders[i] = pickups[j].Index;

						curOrder++;
						if (curOrder == order)
							curOrder++;
						break;
					}
				}
			}

			SyncOrder();

			// RequestSerialization();
		}

		public void EndPickup()
		{
			SetOwner();
			holdingIndex = NONE_INT;
			// RequestSerialization();
		}

		private void Update()
		{
			if (holdingIndex == NONE_INT)
				return;

			if (IsOwner() == false)
				return;

			pickups[holdingIndex].SetOrder(NearOrder(pickups[holdingIndex].transform.position));
			int ban = pickups[holdingIndex].Order;

			int curOrder = 0;
			if (curOrder == ban)
				curOrder++;

			// 나머지 순서대로
			for (int i = 0; i < lastOrders.Length; i++)
			{
				pickups[lastOrders[i]].SetOrder(curOrder);

				curOrder++;
				if (curOrder == ban)
					curOrder++;
			}

			SyncOrder();
		}

		private int NearOrder(Vector3 position)
		{
			float minDistance = float.MaxValue;
			int index = NONE_INT;

			for (int i = 0; i < Positions.Length; i++)
			{
				float distance = Vector3.Distance(position, Positions[i].position);

				if (distance <= minDistance)
				{
					minDistance = distance;
					index = i;
				}
			}

			return index;
		}

		// OrderSync를 순서 바꿔서 섞기
		public void ShuffleOrder()
		{
			SetOwner();

			for (int i = 0; i < pickups.Length; i++)
				pickups[i].SetOrder(i);

			for (int i = 0; i < 100; i++)
			{
				int a = Random.Range(0, pickups.Length);
				int b = Random.Range(0, pickups.Length);

				int temp = pickups[a].Order;
				pickups[a].SetOrder(pickups[b].Order);
				pickups[b].SetOrder(temp);
			}

			SyncOrder();
		}

		public void ResetOrder()
		{
			SetOwner();

			for (int i = 0; i < pickups.Length; i++)
				pickups[i].SetOrder(i);

			SyncOrder();
		}
	}
}