using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public abstract class ActiveToggle : MBase
	{
		[Header("_" + nameof(ActiveToggle))]
		[SerializeField] private bool defaultActive;

		[Header("_" + nameof(ActiveToggle) + " - Options")]
		[SerializeField] private MBool mBool;

		private bool _active;
		public bool Active
		{
			get => _active;
			private set
			{
				MDebugLog($"{nameof(Active)} changed: {_active} -> {value}");
				_active = value;
				UpdateActive();
			}
		}

		private void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			if (mBool != null)
			{
				mBool.RegisterListener(this, nameof(UpdateValueByMBool));
				UpdateValueByMBool();
			}
			else
			{
				SetActive(defaultActive);
			}

			UpdateActive();
		}

		protected abstract void UpdateActive();

		public void SetActive(bool newActive)
		{
			MDebugLog($"{nameof(SetActive)}({newActive})");

			if (mBool != null)
			{
				mBool.SetValue(newActive);
			}
			else
			{
				Active = newActive;
			}
		}

		public void UpdateValueByMBool()
		{
			if (mBool != null)
				Active = mBool.Value;
		}

		public void SetMBool(MBool mBool)
		{
			this.mBool = mBool;
			UpdateValueByMBool();
		}

		#region HorribleEvents
		[ContextMenu(nameof(ToggleActive))]
		public void ToggleActive() => SetActive(!Active);

		[ContextMenu(nameof(SetActiveTrue))]
		public void SetActiveTrue() => SetActive(true);

		[ContextMenu(nameof(SetActiveFalse))]
		public void SetActiveFalse() => SetActive(false);
		#endregion
	}
}