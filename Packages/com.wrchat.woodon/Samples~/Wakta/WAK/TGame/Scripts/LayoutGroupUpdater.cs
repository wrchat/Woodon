using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.WAK.TGame
{
	public class LayoutGroupUpdater : UdonSharpBehaviour
	{
		private LayoutGroup[] layoutGroups;

		private void OnEnable()
		{
			layoutGroups = GetComponentsInChildren<LayoutGroup>(true);

			if (layoutGroups != null)
			{
				foreach (LayoutGroup layoutGroup in layoutGroups)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
				}
			}
		}
	}
}