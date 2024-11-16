using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class MColor : WEventPublisher
	{
		[Header("_" + nameof(MColor))]
		[SerializeField] private MeshRenderer[] targetMeshRenderers;
		[SerializeField] private Image[] targetImages;
		[field: SerializeField] public Color DefaultColor { get; private set; }

		[Header("_" + nameof(MColor) + " - Options")]
		[SerializeField] private bool useSync;

		[UdonSynced, FieldChangeCallback(nameof(SyncedValue))] private Color _syncedValue;
		public Color SyncedValue
		{
			get => _syncedValue;
			private set
			{
				_syncedValue = value;

				if (useSync)
					SetValue(_syncedValue, isReciever: true);
			}
		}

		private Color _value;
		public Color Value
		{
			get => _value;
			private set
			{
				_value = value;
				OnValueChange();
			}
		}

		private void OnValueChange()
		{
			MDebugLog(nameof(OnValueChange));

			foreach (MeshRenderer target in targetMeshRenderers)
				target.material.color = _value;

			foreach (Image target in targetImages)
				target.color = _value;

			SendEvents();
		}

		private void Start()
		{
			Init();
		}
		
		private void Init()
		{
			if (Networking.IsMaster)
			{
				Value = DefaultColor;
				RequestSerialization();
			}
			else
			{
				OnValueChange();
			}
		}

		public void SetValue(Color newValue, bool isReciever = false)
		{
			if (isReciever == false)
			{
				if (useSync && SyncedValue != newValue)
				{
					SetOwner();
					SyncedValue = newValue;
					RequestSerialization();

					return;
				}
			}

			Value = newValue;
		}
	}
}