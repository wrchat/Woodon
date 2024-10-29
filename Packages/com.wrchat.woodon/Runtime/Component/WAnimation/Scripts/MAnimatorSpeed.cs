
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WRC.Woodon
{
	public class MAnimatorSpeed : MBase
	{
		[Header("_" + nameof(MAnimatorSpeed))]
		[SerializeField] private float speed;
		private MAnimator mAnimator;

		private void Start()
		{
			mAnimator = GetComponent<MAnimator>();
			foreach (Animator animator in mAnimator.Animators)
				animator.speed = speed;
		}
	}
}