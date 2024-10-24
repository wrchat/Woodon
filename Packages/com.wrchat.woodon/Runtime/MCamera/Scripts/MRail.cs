using Cinemachine;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	public enum RailCameraSpeed
	{
		Slow,
		Default,
		Fast
	}

	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MRail : UdonSharpBehaviour
	{
		[SerializeField] private CinemachineDollyCart cart;
		// [SerializeField] private float speed = 1;
		[SerializeField] private bool autoMoving;
		private float _railValue = .5f;
		private bool isAdding = true;
		private RailCameraSpeed railCameraSpeed = RailCameraSpeed.Default;

		private float RailValue
		{
			get => _railValue;
			set
			{
				_railValue = Mathf.Clamp01(value);
				cart.m_Position = _railValue;
			}
		}

		public bool AutoMoving => autoMoving;

		private void Start()
		{
			cart.m_Position = _railValue;
		}

		private void Update()
		{
			if (autoMoving)
			{
				if (isAdding)
				{
					if (RailValue >= 1)
						isAdding = false;
					else
						RailValue += DeltaDistance();
				}
				else
				{
					if (RailValue <= 0)
						isAdding = true;
					else
						RailValue -= DeltaDistance();
				}

				cart.m_Position = RailValue;
			}
		}

		public void AddRail(bool add)
		{
			// 0 ~ 1
			if (!Networking.LocalPlayer.IsOwner(gameObject))
				Networking.SetOwner(Networking.LocalPlayer, gameObject);
			if (!Networking.LocalPlayer.IsOwner(cart.gameObject))
				Networking.SetOwner(Networking.LocalPlayer, cart.gameObject);
			if (!autoMoving)
			{
				if (add)
					RailValue += DeltaDistance();
				else
					RailValue -= DeltaDistance();
			}
		}

		public void ToggleAutoMoving()
		{
			autoMoving = !autoMoving;
		}

		public void DirectionRight()
		{
			isAdding = true;
		}

		public void DirectionLeft()
		{
			isAdding = false;
		}

		public float DeltaDistance()
		{
			return Time.deltaTime * (railCameraSpeed == RailCameraSpeed.Slow ? .02f :
				railCameraSpeed == RailCameraSpeed.Default ? .04f : .1f);
		}

		public void SwitchSpeed()
		{
			railCameraSpeed = railCameraSpeed == RailCameraSpeed.Slow
				? RailCameraSpeed.Default
				: railCameraSpeed == RailCameraSpeed.Default
					? RailCameraSpeed.Fast
					: RailCameraSpeed.Slow;
		}
	}
}