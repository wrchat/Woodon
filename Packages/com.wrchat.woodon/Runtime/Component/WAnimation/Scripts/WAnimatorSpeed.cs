
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public class WAnimatorSpeed : WBase
	{
		[Header("_" + nameof(WAnimatorSpeed))]
		[SerializeField] private float speed;
		private WAnimator wAnimator;

		private void Start()
		{
			wAnimator = GetComponent<WAnimator>();
			foreach (Animator animator in wAnimator.Animators)
				animator.speed = speed;
		}
	}
}