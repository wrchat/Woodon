using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.WGame
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WGameHiddenManager : MBase
	{
		[UdonSynced, FieldChangeCallback(nameof(DoorOpen))] private bool _doorOpen = false;
		public bool DoorOpen
		{
			get => _doorOpen;
			set
			{
				_doorOpen = value;
				OnDoorOpenChange();
			}
		}

		private void OnDoorOpenChange()
		{
			doorAnimator.SetBool("OPEN", _doorOpen);
			hider.SetActive(!_doorOpen);
		}

		[SerializeField] private GameObject hider;
		[SerializeField] private WGameHiddenObject[] wGameHiddenObjects;
		[SerializeField] private Animator doorAnimator;
		[SerializeField] private MSFXManager sfx;

		private void Start()
		{
			OnDoorOpenChange();
		}

		public void TryOpenDoor()
		{
			if (IsOwner())
			{
				for (int i = 0; i < wGameHiddenObjects.Length; i++)
					if (wGameHiddenObjects[i].OwnerWaktaIndex == WGameHiddenObject.NO_ONE)
						return;

				DoorOpen = true;
				RequestSerialization();
				sfx.PlaySFX_G(0);
				sfx.PlaySFX_G(1);
			}
		}
	}
}