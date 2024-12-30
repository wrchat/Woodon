using UnityEngine;

namespace WRC.Woodon
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActive : ActiveToggle
	{
		[Header("_" + nameof(CanvasGroupActive))]
		[SerializeField] private CanvasGroup[] activeCanvasGroups;
		[SerializeField] private CanvasGroup[] disableCanvasGroups;

		private Collider[] activeColliders = new Collider[0];
		private Collider[] disableColliders = new Collider[0];

		[Header("_" + nameof(CanvasGroupActive) + " - Options")]
		[SerializeField] private bool toggleOnlyInteractable = false;
		[SerializeField] private bool toggleColliders = false;

		protected override void Init()
		{
			if (toggleColliders)
			{
				for (int i = 0; i < activeCanvasGroups.Length; i++)
				{
					if (activeCanvasGroups[i] == null)
						continue;

					Collider[] colliders = activeCanvasGroups[i].GetComponentsInChildren<Collider>(true);
					activeColliders.AddRange(colliders);
				}

				for (int i = 0; i < disableCanvasGroups.Length; i++)
				{
					if (disableCanvasGroups[i] == null)
						continue;

					Collider[] colliders = disableCanvasGroups[i].GetComponentsInChildren<Collider>(true);
					disableColliders.AddRange(colliders);
				}
			}

			base.Init();
		}

		protected override void UpdateActive()
		{
			MDebugLog($"{nameof(UpdateActive)}");

			if (toggleOnlyInteractable)
			{
				foreach (CanvasGroup c in activeCanvasGroups)
					c.interactable = Active;

				foreach (CanvasGroup c in disableCanvasGroups)
					c.interactable = !Active;
			}
			else
			{
				foreach (CanvasGroup c in activeCanvasGroups)
					WUtil.SetCanvasGroupActive(c, Active);

				foreach (CanvasGroup c in disableCanvasGroups)
					WUtil.SetCanvasGroupActive(c, !Active);
			}

			if (toggleColliders)
			{
				foreach (Collider c in activeColliders)
					c.enabled = Active;

				foreach (Collider c in disableColliders)
					c.enabled = !Active;
			}
		}

		public void RegisterActiveCanvasGroup(CanvasGroup canvasGroup)
		{
			activeCanvasGroups.Add(canvasGroup);
		}

		public void RegisterDisableCanvasGroup(CanvasGroup canvasGroup)
		{
			disableCanvasGroups.Add(canvasGroup);
		}
	}
}

// 밥