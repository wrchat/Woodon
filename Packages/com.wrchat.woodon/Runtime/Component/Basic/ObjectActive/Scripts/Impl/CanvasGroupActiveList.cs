using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActiveList : ActiveList
	{
		[Header("_" + nameof(CanvasGroupActiveList))]
		[SerializeField] private CanvasGroup[] canvasGroups;

		private Collider[][] colliders = new Collider[0][];

		[Header("_" + nameof(CanvasGroupActiveList) + " - Options")]
		[SerializeField] private bool toggleOnlyInteractable = false;
		[SerializeField] private bool toggleColliders = false;

		protected override void Init()
		{
			if (toggleColliders)
			{
				colliders = new Collider[canvasGroups.Length][];

				for (int i = 0; i < canvasGroups.Length; i++)
				{
					if (canvasGroups[i] == null)
						continue;

					Collider[] cs = canvasGroups[i].GetComponentsInChildren<Collider>(true);
					colliders[i] = cs;
				}
			}

			base.Init();
		}

		protected override void InitWIntMinMax()
		{
			// wInt.SetMinMaxValue(0, canvasGroups.Length - 1);
		}

		protected override void UpdateActive()
		{
			WDebugLog($"{nameof(UpdateActive)}({Value})");

			switch (option)
			{
				case ActiveListOption.UseValueAsListIndex:
					for (int i = 0; i < canvasGroups.Length; i++)
					{
						if (canvasGroups[i] == null)
							continue;

						if (toggleOnlyInteractable)
						{
							canvasGroups[i].interactable = i == Value;
						}
						else
						{
							WUtil.SetCanvasGroupActive(canvasGroups[i], i == Value);
						}

						if (toggleColliders)
						{
							foreach (Collider c in colliders[i])
								c.enabled = i == Value;
						}
					}

					break;
				case ActiveListOption.UseValueAsTargetIndex:
					bool isTargetIndex = Value == targetIndex;
					for (int i = 0; i < canvasGroups.Length; i++)
					{
						if (canvasGroups[i] == null)
							continue;

						if (toggleOnlyInteractable)
						{
							canvasGroups[i].interactable = isTargetIndex;
						}
						else
						{
							WUtil.SetCanvasGroupActive(canvasGroups[i], isTargetIndex);
						}
						
						if (toggleColliders)
						{
							foreach (Collider c in colliders[i])
								c.enabled = isTargetIndex;
						}
					}
					break;
				default:
					WDebugLog($"{nameof(UpdateActive)}({Value}) - {option}, Invalid Option");
					break;
			}
		}
	}
}