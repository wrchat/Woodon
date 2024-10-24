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
			for (int i = 0; i < activeCanvasGroups.Length; i++)
			{
				if (activeCanvasGroups[i] == null)
					continue;

				Collider[] colliders = activeCanvasGroups[i].GetComponentsInChildren<Collider>(true);
				MDataUtil.AddRange(ref activeColliders, colliders);
			}

			for (int i = 0; i < disableCanvasGroups.Length; i++)
			{
				if (disableCanvasGroups[i] == null)
					continue;

				Collider[] colliders = disableCanvasGroups[i].GetComponentsInChildren<Collider>(true);
				MDataUtil.AddRange(ref disableColliders, colliders);
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
					MUtil.SetCanvasGroupActive(c, Active);

				foreach (CanvasGroup c in disableCanvasGroups)
					MUtil.SetCanvasGroupActive(c, !Active);
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
			MDataUtil.Add(ref activeCanvasGroups, canvasGroup);
		}

		public void RegisterDisableCanvasGroup(CanvasGroup canvasGroup)
		{
			MDataUtil.Add(ref disableCanvasGroups, canvasGroup);
		}
	}
}

// 밥