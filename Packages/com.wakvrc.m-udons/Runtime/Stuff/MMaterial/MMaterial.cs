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
		[SerializeField] private MBool switcher;
		[SerializeField] private MValue materialIndex;

		protected virtual void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (switcher != null)
			{
				switcher.RegisterListener(this, nameof(UpdateMaterial));
				UpdateMaterial();
			}
		}

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
				newMaterial = materials[materialIndex.Value];

			for (int i = 0; i < meshRenderers.Length; i++)
			{
				Material[] materials = meshRenderers[i].materials;
				materials[rendererMaterialIndexes[i]] = newMaterial;
				meshRenderers[i].materials = materials;
			}
		}
	}
}