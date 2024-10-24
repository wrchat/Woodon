using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.ReVerse
{
	public class TrainCart : UdonSharpBehaviour
	{
		[SerializeField] private Animator animator;
		private bool isMoving;

		private float speed = 1;
		private float t;
		private Vector3 targetPos = Vector3.zero;

		private void Update()
		{
			if (isMoving)
			{
				t += Time.deltaTime * speed;
				transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, t);

				if (t >= 1)
				{
					isMoving = false;
					t = 0;
					transform.localPosition = targetPos;
				}
			}
		}

		public void MoveTo(Vector3 targetPos, float speed)
		{
			isMoving = true;
			this.targetPos = targetPos;
			this.speed = speed;
		}

		public void Die()
		{
			animator.SetBool("DIE", true);
		}

		public void Revival()
		{
			animator.SetBool("DIE", false);
		}
	}
}