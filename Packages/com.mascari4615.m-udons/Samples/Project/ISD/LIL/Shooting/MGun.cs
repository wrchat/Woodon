using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MGun : MBase
	{
		[Header("_" + nameof(MGun))]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip shootSFX;
		[SerializeField] private AudioClip noneSFX;
		[SerializeField] private AudioClip reloadSFX;

		[SerializeField] private Animator[] animators;
		[SerializeField] private ParticleSystem[] particles;
		[SerializeField] private VRC_Pickup gunObject;
		[SerializeField] private VRCObjectSync objectSync;
		[SerializeField] private GameObject reloadButton;

		[SerializeField] private float hapticDuration = .1f;
		[SerializeField] private float hapticAmplitude = .5f;
		[SerializeField] private float hapticFrequency = .5f;

		[SerializeField] private bool sendEventWhenLocalPlayerIsGunOwner;
		[SerializeField] private bool pcCanReloadByKeyR;

		[SerializeField] private GameObject ui;
		[SerializeField] private TextMeshProUGUI maxAmmoText;
		[SerializeField] private TextMeshProUGUI curAmmoText;

		[SerializeField] private int maxAmmo;
		[SerializeField] private bool autoReload;

		// TimeEvent가 동작 중일 때는 아무런 조작도 할 수 없도록
		[SerializeField] private TimeEvent reloadCooltime;

		[UdonSynced(), FieldChangeCallback(nameof(CurAmmo))]
		private int curAmmo;

		private ShootingManager shootingManager;

		public int CurAmmo
		{
			get => curAmmo;
			set
			{
				curAmmo = value;
				OnAmmoChanged();
			}
		}

		private void Start()
		{
			shootingManager = GameObject.Find(nameof(ShootingManager)).GetComponent<ShootingManager>();

			if (gunObject == null)
				gunObject = transform.parent.gameObject.GetComponent<VRC_Pickup>();

			if (Networking.IsMaster)
			{
				CurAmmo = maxAmmo;
				RequestSerialization();
			}

			reloadButton.SetActive((autoReload == false) && (maxAmmo != 0));
			ui.SetActive(maxAmmo != 0);
			maxAmmoText.text = maxAmmo.ToString();
			OnAmmoChanged();
		}

		private bool _canInteract;
		public bool CanInteract
		{
			get => _canInteract;
			set
			{
				_canInteract = value;

				if ((_canInteract == false) && IsLocalPlayerHolding(gunObject))
					gunObject.Drop();

				gunObject.pickupable = _canInteract;
			}
		}

		private void Update()
		{
			if (reloadCooltime.IsExpired == false)
				return;

			if (pcCanReloadByKeyR)
				if (Input.GetKeyDown(KeyCode.E))
					if (IsLocalPlayerHolding(gunObject))
						Reload();

			if (IsLocalPlayerHolding(gunObject))
			{
				if (ui.activeSelf == false)
					ui.SetActive(true);

				if (autoReload && (CurAmmo == 0))
				{
					Reload();
				}
			}
			else
			{
				if (ui.activeSelf)
					ui.SetActive(false);
			}
		}

		private void OnParticleCollision(GameObject other)
		{
			if (sendEventWhenLocalPlayerIsGunOwner && !IsLocalPlayerHolding(gunObject))
				return;

			shootingManager.Ahh(other);
		}

		private void OnAmmoChanged()
		{
			curAmmoText.text = CurAmmo.ToString();
		}

		public VRC_Pickup.PickupHand GetLocalPlayerGripHand()
		{
			return Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right) == gunObject
				? VRC_Pickup.PickupHand.Right
				: VRC_Pickup.PickupHand.Left;
		}

		public void TryBBang()
		{
			if (IsLocalPlayerHolding(gunObject))
			{
				if (reloadCooltime.IsExpired == false)
					return;

				if (maxAmmo != 0)
				{
					if (CurAmmo <= 0)
					{
						SendCustomNetworkEvent(NetworkEventTarget.All, nameof(BBangFailed));
						return;
					}

					SetOwner();
					CurAmmo--;
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

			if (reloadCooltime.IsExpired == false)
				return;

			// if (LocalPlayerHolding())
			{
				if (CurAmmo == maxAmmo)
					return;

				SetOwner();
				CurAmmo = maxAmmo;
				RequestSerialization();

				SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ReloadSFX));

				reloadCooltime.SetTime();
			}
		}

		public void BBang()
		{
			audioSource.PlayOneShot(shootSFX);

			foreach (var particle in particles) particle.Play();

			foreach (var animator in animators) animator.SetTrigger("BBANG");
		}

		public void BBangFailed()
		{
			audioSource.PlayOneShot(noneSFX);
		}

		public void ReloadSFX()
		{
			audioSource.PlayOneShot(reloadSFX);
		}

		public void Respawn()
		{
			if (IsOwner(gunObject.gameObject))
			{
				if (IsLocalPlayerHolding(gunObject))
					gunObject.Drop(Networking.LocalPlayer);
				objectSync.Respawn();
			}
		}

		public void MoveTo(Vector3 position)
		{
			if (IsOwner(gunObject.gameObject))
			{
				if (IsLocalPlayerHolding(gunObject))
					gunObject.Drop(Networking.LocalPlayer);
				gunObject.transform.position = position;
			}
		}
	}
}