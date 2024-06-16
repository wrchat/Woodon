
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class MMaterial : MBase
	{
		[SerializeField] protected MeshRenderer[] meshRenderers;
		[SerializeField] private Material[] materials;
		[SerializeField] private SyncedBool syncedBool;
		[SerializeField] private MScore mScore;

		public virtual void UpdateMaterial()
		{
			if (meshRenderers == null || meshRenderers.Length == 0)
				return;

			Material newMaterial = materials[0];

			if (syncedBool)
				newMaterial = materials[syncedBool.SyncValue ? 1 : 0];

			if (mScore)
				newMaterial = materials[mScore.Score];

			foreach (var meshRenderer in meshRenderers)
				meshRenderer.material = newMaterial;
		}
	}
}