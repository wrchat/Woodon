using UdonSharp;
using UnityEngine;
using WRC.Woodon;

namespace Mascari4615.Project.Wakta.VII.V2Game
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class V2GameSeat : MMaterial
	{
		[Header("_" + nameof(V2GameSeat))]
		[SerializeField] private string objectName;
		[SerializeField] private MeshRenderer localScreen;

		protected override void Start()
		{
			base.Start();

			GameObject g = GameObject.Find(objectName);
			meshRenderers = new MeshRenderer[2];
			meshRenderers[0] = g.GetComponent<MeshRenderer>();
			meshRenderers[1] = localScreen;

			UpdateMaterial();
		}

		public override void UpdateMaterial()
		{
			if (meshRenderers == null || meshRenderers.Length == 0)
				Start();

			base.UpdateMaterial();
		}
	}
}