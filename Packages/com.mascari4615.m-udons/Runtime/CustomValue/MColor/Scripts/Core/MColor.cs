using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Mascari4615
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MColor : MEventSender
	{
		[Header("_" + nameof(MColor))]
		[SerializeField] private MeshRenderer[] targetMeshRenderers;
		[SerializeField] private Image[] targetImages;

		[UdonSynced] public Color OriginColor;

		[UdonSynced, FieldChangeCallback(nameof(Value))] private Color _value;
		public Color Value
		{
			get => _value;
			set
			{
				_value = value;
				OnValueChanged();
				SendEvents();
			}
		}

		public void SetColor(Color color)
		{
			SetOwner();
			Value = color;
			RequestSerialization();
		}

		private void OnValueChanged()
		{
			foreach (MeshRenderer target in targetMeshRenderers)
				target.material.color = _value;

			foreach (Image target in targetImages)
				target.color = _value;
		}
	}
}