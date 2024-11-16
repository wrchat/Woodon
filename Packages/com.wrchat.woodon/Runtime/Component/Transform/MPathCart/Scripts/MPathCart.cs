using Cinemachine;
using UnityEngine;

namespace WRC.Woodon
{
	public class MPathCart : WEventPublisher
	{
		[Header("_" + nameof(MPathCart))]
		[SerializeField] private CinemachineDollyCart cart;
		[SerializeField] private MStation station;
		[SerializeField] private float pathLength = 10f;

		[Header("_" + nameof(MPathCart) + " - Options")]
		[SerializeField] private float speed = 1f;
		[SerializeField] private float duration = NONE_INT;
		[SerializeField] private MPathCartMovementType defaultMovementType;
		[SerializeField] private bool isOneWay = true;
		[SerializeField] private Transform cartTransform;
		[SerializeField] private bool setCartRotationWhenUseStation = false;

		private MPathCartState curState = MPathCartState.Stop;
		private MPathCartMovementType curMovementType;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			// Cart 초기 위치 설정
			InitCart(defaultMovementType);
		}

		private void InitCart(MPathCartMovementType movementType)
		{
			curState = MPathCartState.Stop;
			this.curMovementType = movementType;

			cart.m_Position = GetDestinationPos(GetOppositeMovementType(movementType));

			if (setCartRotationWhenUseStation)
			{
				float rotationY = movementType == MPathCartMovementType.Backward ? 180 : 0;
				cartTransform.localRotation = Quaternion.Euler(0, rotationY, 0);
			}
		}

		private float GetDestinationPos(MPathCartMovementType movementType)
		{
			switch (movementType)
			{
				case MPathCartMovementType.Forward:
					return pathLength;
				case MPathCartMovementType.Backward:
					return 0;
				default:
					MDebugLog($"Invalid {nameof(MPathCartMovementType)}: {movementType}");
					return pathLength;
			}
		}

		[ContextMenu(nameof(TogglePath))]
		public void TogglePath()
		{
			InitCart(GetOppositeMovementType(curMovementType));
		}

		[ContextMenu(nameof(StartPath))]
		public void StartPath(MPathCartMovementType movementType)
		{
			if (curMovementType != movementType)
				InitCart(movementType);

			curState = MPathCartState.Move;

			station.UseStation();

			SendEvents(MPathCartEvent.StartPath);
			SendEvents((MPathCartEvent)((int)MPathCartEvent.StartPath + (int)movementType));
		}

		private void Update()
		{
			MoveCart();
			CheckEnd();
		}

		private void MoveCart()
		{
			if (curState != MPathCartState.Move)
				return;

			cart.m_Position += GetMoveAmount(curMovementType);
			cart.m_Position = Mathf.Clamp(cart.m_Position, 0, pathLength);
		}

		// Tick당 (Update) 이동량
		private float GetMoveAmount(MPathCartMovementType movementType)
		{
			float moveAmount;

			if (duration != NONE_INT)
			{
				// pathLength를 duration만큼 이동하도록 설정
				moveAmount = (pathLength / duration) * speed * Time.deltaTime;
			}
			else
			{
				moveAmount = speed * Time.deltaTime;
			}

			if (movementType == MPathCartMovementType.Backward)
			{
				moveAmount *= -1;
			}

			return moveAmount;
		}

		private void CheckEnd()
		{
			if (curState != MPathCartState.Move)
				return;

			if (IsPathEnd())
			{
				EndPath();
			}
		}

		private void EndPath()
		{
			curState = MPathCartState.Stop;

			station.ExitStation();

			SendEvents(MPathCartEvent.EndPath);
			SendEvents((MPathCartEvent)((int)MPathCartEvent.EndPath + (int)curMovementType));

			if (isOneWay)
			{
				InitCart(curMovementType);
			}
			else
			{
				TogglePath();
			}
		}

		private bool IsPathEnd()
		{
			// return cart.m_Position == pathLength;
			float destinationPos = GetDestinationPos(curMovementType);
			return Mathf.Approximately(cart.m_Position, destinationPos);
		}

		private MPathCartMovementType GetOppositeMovementType(MPathCartMovementType curMovementType)
		{
			switch (curMovementType)
			{
				case MPathCartMovementType.Forward:
					return MPathCartMovementType.Backward;
				case MPathCartMovementType.Backward:
					return MPathCartMovementType.Forward;
				default:
					MDebugLog($"Invalid {nameof(MPathCartMovementType)}: {curMovementType}");
					return MPathCartMovementType.Forward;
			}
		}

		#region HorribleEvents
		public void StartCurPath() => StartPath(curMovementType);
		public void StartPathForward() => StartPath(MPathCartMovementType.Forward);
		public void StartPathBackward() => StartPath(MPathCartMovementType.Backward);
		#endregion
	}
}