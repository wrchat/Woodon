using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class WMaterial : WBase
	{
		[Header("_" + nameof(WMaterial))]
		[SerializeField] protected MeshRenderer[] meshRenderers;
		[SerializeField] private int[] rendererMaterialIndexes;
		[SerializeField] protected Material[] materials;

		[Header("_Options : 아래 중 하나를 채워 넣으세요.")]
		[SerializeField] private WBool switcher;
		[SerializeField] private WInt materialIndex;

		private Material[] originalMaterials;

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			originalMaterials = new Material[meshRenderers.Length];
			for (int i = 0; i < meshRenderers.Length; i++)
				originalMaterials[i] = meshRenderers[i].material;

			if (switcher != null)
				switcher.RegisterListener(this, nameof(UpdateMaterial));

			if (materialIndex != null)
				materialIndex.RegisterListener(this, nameof(UpdateMaterial));

			UpdateMaterial();
		}

		[ContextMenu(nameof(UpdateMaterial))]
		public virtual void UpdateMaterial()
		{
			WDebugLog(nameof(UpdateMaterial));

			if (meshRenderers == null || meshRenderers.Length == 0)
				return;

			if (materials == null || materials.Length == 0)
				return;

			if (materials == null || materials.Length == 0)
				return;

			for (int i = 0; i < meshRenderers.Length; i++)
			{
				Material newMaterial = originalMaterials[i];

				if (switcher)
					newMaterial = materials[switcher.Value ? 1 : 0];

				if (materialIndex)
				{
					if (materialIndex.Value >= 0 && materialIndex.Value < materials.Length)
						newMaterial = materials[materialIndex.Value];
				}

				Material[] curMaterials = meshRenderers[i].materials;
				curMaterials[rendererMaterialIndexes[i]] = newMaterial;
				meshRenderers[i].materials = curMaterials;
			}
		}
	}
}