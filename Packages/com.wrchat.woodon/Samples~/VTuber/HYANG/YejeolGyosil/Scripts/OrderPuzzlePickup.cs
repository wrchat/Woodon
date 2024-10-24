using UnityEngine;
using VRC.SDKBase;
using UdonSharp;
using WRC.Woodon;
using static WRC.Woodon.MUtil;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class OrderPuzzlePickup : MPickup
	{
		[Header("_" + nameof(OrderPuzzlePickup))]
		private OrderPuzzleManager manager;
		public int Index { get; private set; }
		public int Order { get; private set; }

		public void Init(OrderPuzzleManager manager, int index)
		{
			this.manager = manager;
			Index = index;
			Order = index;
		}

		public void SetOrder(int order)
		{
			Order = order;
		}

		protected override void Update()
		{
			base.Update();
			
			// if (IsOwner() == false)
			// 	return;

			if (manager == null)
				return;

			if (IsPlayerHolding(Networking.LocalPlayer, Pickup))
				return;

			if (Order < 0 || Order >= manager.Positions.Length)
			{
				MDebugLog($"Order is out of range: {Order}");
				return;
			}
			
			transform.position = Vector3.Lerp(transform.position, manager.Positions[Order].position, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, manager.Positions[Order].rotation, 0.1f);
		}

		public override void OnPickup()
		{
			MDebugLog($"{Index}번째 아이템을 픽업합니다. 순서: {Order}");
			manager.StartPickup(Index, Order);
		}

		public override void OnDrop()
		{
			manager.EndPickup();
		}
	}
}