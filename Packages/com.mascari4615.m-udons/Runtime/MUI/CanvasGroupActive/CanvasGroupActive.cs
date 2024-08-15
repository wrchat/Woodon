using UnityEngine;
using static Mascari4615.MUtil;

namespace Mascari4615
{
	// [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class CanvasGroupActive : MBase
	{
		[Header("_" + nameof(CanvasGroupActive))]
		[SerializeField] private CanvasGroup[] activeCanvasGroups;
		[SerializeField] private CanvasGroup[] disableCanvasGroups;
		[SerializeField] private bool defaultActive;

		[Header("_" + nameof(CanvasGroupActive) + " - Options")]
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

			foreach (CanvasGroup c in activeCanvasGroups)
				SetCanvasGroupActive(c, Active);

			foreach (CanvasGroup c in disableCanvasGroups)
				SetCanvasGroupActive(c, !Active);
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

		public void SetCustomBool(MBool mBool)
		{
			this.mBool = mBool;
			UpdateValue();
		}
	}
}