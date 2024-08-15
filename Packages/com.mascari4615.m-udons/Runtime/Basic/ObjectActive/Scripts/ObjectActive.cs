using UnityEngine;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ObjectActive : MBase
	{
		[Header("_" + nameof(ObjectActive))]
		[SerializeField] private GameObject[] activeObjects;
		[SerializeField] private GameObject[] disableObjects;
		[SerializeField] private bool defaultActive;

		[Header("_" + nameof(ObjectActive) + " - Options")]
		[SerializeField] private MBool mBool;

		private bool _active;
		public bool Active
		{
			get => _active;
			private set
			{
				_active = value;
				OnActiveChange();
			}
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			SetActive(defaultActive);
	
			if (mBool != null)
				mBool.RegisterListener(this, nameof(UpdateValue));

			OnActiveChange();
		}

		private void OnActiveChange()
		{
			MDebugLog($"{nameof(OnActiveChange)}");

			foreach (GameObject o in activeObjects)
				o.SetActive(Active);

			foreach (GameObject o in disableObjects)
				o.SetActive(!Active);
		}

		public void SetActive(bool targetActive)
		{
			MDebugLog($"{nameof(SetActive)}({targetActive})");
			Active = targetActive;
		}

		[ContextMenu(nameof(ToggleActive))]
		public void ToggleActive() => SetActive(!Active);

		[ContextMenu(nameof(SetActiveTrue))]
		public void SetActiveTrue() => SetActive(true);

		[ContextMenu(nameof(SetActiveFalse))]
		public void SetActiveFalse() => SetActive(false);

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (mBool)
				SetActive(mBool.Value);
		}

		public void SetMBool(MBool mBool)
		{
			this.mBool = mBool;
			UpdateValue();
		}
	}
}