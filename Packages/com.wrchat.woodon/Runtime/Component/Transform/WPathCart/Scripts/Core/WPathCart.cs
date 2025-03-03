using Cinemachine;
using UnityEngine;

namespace WRC.Woodon
{
	public class WPathCart : WEventPublisher
	{
		[Header("_" + nameof(WPathCart))]
		[SerializeField] private CinemachineDollyCart cart;
		[SerializeField] private WStation station;
		[SerializeField] private float pathLength = 10f;

		[Header("_" + nameof(WPathCart) + " - Options")]
		[SerializeField] private float speed = 1f;
		[SerializeField] private float duration = NONE_INT;
		[SerializeField] private WPathCartMovementType defaultMovementType;
		[SerializeField] private bool isOneWay = true;
		[SerializeField] private Transform cartTransform;
		[SerializeField] private bool setCartRotationWhenUseStation = false;

		private WPathCartState curState = WPathCartState.Stop;
		private WPathCartMovementType curMovementType;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			// Cart 초기 위치 설정
			InitCart(defaultMovementType);
		}

		private void InitCart(WPathCartMovementType movementType)
		{
			curState = WPathCartState.Stop;
			this.curMovementType = movementType;

			cart.m_Position = GetDestinationPos(GetOppositeMovementType(movementType));

			if (setCartRotationWhenUseStation)
			{
				float rotationY = movementType == WPathCartMovementType.Backward ? 180 : 0;
				cartTransform.localRotation = Quaternion.Euler(0, rotationY, 0);
			}
		}

		private float GetDestinationPos(WPathCartMovementType movementType)
		{
			switch (movementType)
			{
				case WPathCartMovementType.Forward:
					return pathLength;
				case WPathCartMovementType.Backward:
					return 0;
				default:
					WDebugLog($"Invalid {nameof(WPathCartMovementType)}: {movementType}");
					return pathLength;
			}
		}

		[ContextMenu(nameof(TogglePath))]
		public void TogglePath()
		{
			InitCart(GetOppositeMovementType(curMovementType));
		}

		[ContextMenu(nameof(StartPath))]
		public void StartPath(WPathCartMovementType movementType)
		{
			if (curMovementType != movementType)
				InitCart(movementType);

			curState = WPathCartState.Move;

			station.UseStation();

			SendEvents(WPathCartEvent.StartPath);
			SendEvents((WPathCartEvent)((int)WPathCartEvent.StartPath + (int)movementType));
		}

		private void Update()
		{
			MoveCart();
			CheckEnd();
		}

		private void MoveCart()
		{
			if (curState != WPathCartState.Move)
				return;

			cart.m_Position += GetMoveAmount(curMovementType);
			cart.m_Position = Mathf.Clamp(cart.m_Position, 0, pathLength);
		}

		// Tick당 (Update) 이동량
		private float GetMoveAmount(WPathCartMovementType movementType)
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

			if (movementType == WPathCartMovementType.Backward)
			{
				moveAmount *= -1;
			}

			return moveAmount;
		}

		private void CheckEnd()
		{
			if (curState != WPathCartState.Move)
				return;

			if (IsPathEnd())
			{
				EndPath();
			}
		}

		private void EndPath()
		{
			curState = WPathCartState.Stop;

			station.ExitStation();

			SendEvents(WPathCartEvent.EndPath);
			SendEvents((WPathCartEvent)((int)WPathCartEvent.EndPath + (int)curMovementType));

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

		private WPathCartMovementType GetOppositeMovementType(WPathCartMovementType curMovementType)
		{
			switch (curMovementType)
			{
				case WPathCartMovementType.Forward:
					return WPathCartMovementType.Backward;
				case WPathCartMovementType.Backward:
					return WPathCartMovementType.Forward;
				default:
					WDebugLog($"Invalid {nameof(WPathCartMovementType)}: {curMovementType}");
					return WPathCartMovementType.Forward;
			}
		}

		#region HorribleEvents
		public void StartCurPath() => StartPath(curMovementType);
		public void StartPathForward() => StartPath(WPathCartMovementType.Forward);
		public void StartPathBackward() => StartPath(WPathCartMovementType.Backward);
		#endregion
	}
}