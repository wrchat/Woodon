using UdonSharp;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class AnimatedButton : UdonSharpBehaviour
	{
		[Header("_" + nameof(AnimatedButton))]
		[SerializeField] private Animator[] animators;
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip audioClip;

		[SerializeField] private new ParticleSystem particleSystem;
		[SerializeField] private float lerpAddSpeed = 1;
		[SerializeField] private float lerpStaySpeed = 1;
		[SerializeField] private float lerpSubSpeed = 1;
		[SerializeField] private float minSpeed = .1f;
		[SerializeField] private float maxSpeed = 1f;
		[SerializeField] private float maxScale = 2;
		private float defaultScale = 1;

		private bool nowAdd;
		private bool nowStay;
		private bool nowSub;

		private float t;
		private float tt;

		private void Start()
		{
			defaultScale = transform.localScale.x;
		}

		private void Update()
		{
			if (nowAdd)
			{
				t += Time.deltaTime * lerpAddSpeed;

				if (t > 1)
				{
					nowAdd = false;
					nowStay = true;
					t = 1;
					tt = 0;
				}
			}
			else if (nowStay)
			{
				tt += Time.deltaTime * lerpStaySpeed;

				if (tt > 1)
				{
					nowStay = false;
					nowSub = true;
				}
			}
			else if (nowSub)
			{
				t -= Time.deltaTime * lerpSubSpeed;

				if (t < 0)
				{
					nowSub = true;
					t = 0;
				}
			}

			foreach (Animator animator in animators)
				animator.speed = Mathf.Lerp(minSpeed, maxSpeed, t);

			transform.localScale = Vector3.one * Mathf.Lerp(defaultScale, maxScale, t);
		}

		public override void Interact()
		{
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Click));
		}

		public void Click()
		{
			if (particleSystem != null)
				particleSystem.Play();

			audioSource.PlayOneShot(audioClip);

			nowAdd = true;
			t = 0;
		}
	}
}