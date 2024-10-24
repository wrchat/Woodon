using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class PosChanger : MBase
	{
		[Header("_" + nameof(PosChanger))]
		// [UdonSynced, FieldChangeCallback(nameof(IsOriginPos))]
		[SerializeField] private GameObject targetObject;
		[SerializeField] private Transform posA;
		[SerializeField] private Transform posB;
		[SerializeField] private MBool mBool;
		
		private bool _isOriginPos;
		public bool IsOriginPos
		{
			get => _isOriginPos;
			set
			{
				_isOriginPos = value;
				UpdatePos();
			}
		}

		private void OnEnable()
		{
			UpdateValue();
			UpdatePos();
		}

		private void UpdatePos()
		{
			targetObject.transform.localPosition = IsOriginPos ? posA.position : posB.position;
			targetObject.transform.localRotation = IsOriginPos ? posA.rotation : posB.rotation;
			targetObject.transform.localScale = IsOriginPos ? posA.localScale : posB.localScale;
		}

		public void TogglePos()
		{
			MDebugLog(nameof(TogglePos));
			// SetOwner();
			IsOriginPos = !IsOriginPos;
			// RequestSerialization();
		}

		public void SetPosOrigin() => IsOriginPos = true;
		public void SetPosTarget() => IsOriginPos = false;

		public void UpdateValue()
		{
			if (mBool)
				IsOriginPos = mBool.Value;
		}
	}
}