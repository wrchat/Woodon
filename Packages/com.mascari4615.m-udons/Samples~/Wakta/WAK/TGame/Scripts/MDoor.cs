using UdonSharp;
using UnityEngine;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MDoor : MEventSender
	{
		[field: Header("_" + nameof(MDoor))]
		[field: SerializeField] public Transform TpPos { get; private set;}
		[SerializeField] private MDoor linkedDoor;
		[SerializeField] private MSFXManager sfxManager;
		[SerializeField] private Animator lockAnimator;

		[SerializeField] private CustomBool IsLock;

		public void EnterDoor()
		{
			if (linkedDoor == null)
				return;
			
			if (IsLock.Value)
			{
				PlayLockSFX();
				linkedDoor.PlayLockSFX();
				return;
			}

			TP(linkedDoor.TpPos);
			PlayOutSFX();
			linkedDoor.PlayInSFX();

			SendEvents();
		}

		public void PlayInSFX()
		{
			sfxManager.PlaySFX_G(0);
		}

		public void PlayOutSFX()
		{
			sfxManager.PlaySFX_G(0);
		}

		public void PlayLockSFX()
		{
			sfxManager.PlaySFX_G(1);
		}

		public void UpdateLock()
		{
			lockAnimator.SetBool("LOCK", IsLock.Value);
		}
	}
}