using System.Runtime.Remoting.Contexts;
using UdonSharp;
using UnityEngine;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class MMaterial : MBase
	{
		[Header("_" + nameof(MMaterial))]
		[SerializeField] protected MeshRenderer[] meshRenderers;
		[SerializeField] private int[] rendererMaterialIndexes;
		[SerializeField] protected Material[] materials;

		[Header("_Options : 아래 중 하나를 채워 넣으세요.")]
		[SerializeField] private CustomBool switcher;
		[SerializeField] private MScore materialIndex;

		[ContextMenu(nameof(UpdateMaterial))]
		public virtual void UpdateMaterial()
		{
			MDebugLog(nameof(UpdateMaterial));

			if (meshRenderers == null || meshRenderers.Length == 0)
				return;

			if (materials == null || materials.Length == 0)
				return;

			if (materials == null || materials.Length == 0)
				return;

			Material newMaterial = materials[0];

			if (switcher)
				newMaterial = materials[switcher.Value ? 1 : 0];

			if (materialIndex)
				newMaterial = materials[materialIndex.Score];

			for (int i = 0; i < meshRenderers.Length; i++)
			{
				Material[] materials = meshRenderers[i].materials;
				materials[rendererMaterialIndexes[i]] = newMaterial;
				meshRenderers[i].materials = materials;
			}
		}
	}
}