using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MGun : MPickup
	{
		[Header("_" + nameof(MGun))]
		[SerializeField] private MValue curAmmo;

		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSFX;
		[SerializeField] private AudioClip noneSFX;
		[SerializeField] private AudioClip reloadSFX;

		[SerializeField] private Animator[] animators;
		[SerializeField] private ParticleSystem[] particles;
		[SerializeField] private GameObject reloadButton;

		[Header("_" + nameof(MGun) + "Options")]
		[SerializeField] private int maxAmmo;
		[SerializeField] private bool autoReload;
		[SerializeField] private float hapticDuration = .1f;
		[SerializeField] private float hapticAmplitude = .5f;
		[SerializeField] private float hapticFrequency = .5f;

		[SerializeField] private bool sendEventWhenLocalPlayerIsGunOwner;
		[SerializeField] private bool pcCanReloadByKeyR;
		// Timer가 동작 중일 때는 아무런 조작도 할 수 없도록
		[SerializeField] private Timer reloadCooltime;

		private ShootingManager shootingManager;

		private void Start()
		{
			shootingManager = GameObject.Find(nameof(ShootingManager)).GetComponent<ShootingManager>();

			curAmmo.SetMinMaxValue(0, maxAmmo, recalcValue: false);

			if (Networking.IsMaster)
				curAmmo.SetValue(maxAmmo);

			reloadButton.SetActive((autoReload == false) && (maxAmmo != 0));
		}

		private bool _canInteract;
		public bool CanInteract
		{
			get => _canInteract;
			set
			{
				_canInteract = value;

				if ((_canInteract == false) && IsHolding())
					Drop();

				// gunObject.pickupable = _canInteract;
			}
		}

		protected override void Update()
		{
			base.Update();
		
			if (reloadCooltime.IsExpiredOrStoped == false)
				return;

			if (pcCanReloadByKeyR)
				if (Input.GetKeyDown(KeyCode.E))
					if (IsHolding())
						Reload();

			if (IsHolding())
				if (autoReload && (curAmmo.Value == 0))
					Reload();
		}

		private void OnParticleCollision(GameObject other)
		{
			if (sendEventWhenLocalPlayerIsGunOwner && (IsHolding() == false))
				return;

			shootingManager.Ahh(other);
		}

		public void TryBBang()
		{
			if (IsHolding())
			{
				if (reloadCooltime.IsExpiredOrStoped == false)
					return;

				if (maxAmmo != 0)
				{
					if (curAmmo.Value <= 0)
					{
						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BBangFailed));
						return;
					}

					SetOwner();
					curAmmo.DecreaseValue();
					RequestSerialization();
				}

				Networking.LocalPlayer.PlayHapticEventInHand(GetLocalPlayerGripHand(), hapticDuration, hapticAmplitude,
					hapticFrequency);
				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BBang));
			}
		}

		public void Reload()
		{
			if (maxAmmo == 0)
				return;

			if (reloadCooltime.IsExpiredOrStoped == false)
				return;

			// if (LocalPlayerHolding())
			{
				if (curAmmo.Value == maxAmmo)
					return;

				curAmmo.SetValue(maxAmmo);

				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ReloadSFX));

				reloadCooltime.StartTimer();
			}
		}

		public void BBang()
		{
			audioSource.PlayOneShot(shootSFX);

			foreach (ParticleSystem particle in particles)
				particle.Play();

			foreach (Animator animator in animators)
				animator.SetTrigger("BBANG");
		}

		public void BBangFailed()
		{
			audioSource.PlayOneShot(noneSFX);
		}

		public void ReloadSFX()
		{
			audioSource.PlayOneShot(reloadSFX);
		}

		public void MoveTo(Vector3 position)
		{
			if (IsHolding())
			{
				Drop();
				transform.position = position;
			}
		}
	}
}