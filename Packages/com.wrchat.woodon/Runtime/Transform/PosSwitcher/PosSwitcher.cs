using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class PosSwitcher : MBase
	{
		[Header("_" + nameof(PosSwitcher))]
		[SerializeField] private GameObject[] targetObjects;
		[SerializeField] private Transform posA, posB;
		[SerializeField] private MBool isPosA;

		[Header("_" + nameof(PosSwitcher) + " - Options")]
		[SerializeField] private bool useLocalTransform = true;
		[SerializeField] private bool useScale = false;

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

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (isPosA != null)
				isPosA.RegisterListener(this, nameof(UpdateValue));
		}

		public void UpdateValue()
		{
			if (isPosA != null)
				IsOriginPos = isPosA.Value;
		}

		private void OnEnable()
		{
			UpdateValue();
			UpdatePos();
		}

		private void UpdatePos()
		{
			foreach (GameObject targetObject in targetObjects)
			{
				if (targetObject == null)
					continue;

				if (useLocalTransform)
				{
					targetObject.transform.localPosition = IsOriginPos ? posA.localPosition : posB.localPosition;
					targetObject.transform.localRotation = IsOriginPos ? posA.localRotation : posB.localRotation;
				}
				else
				{
					targetObject.transform.position = IsOriginPos ? posA.position : posB.position;
					targetObject.transform.rotation = IsOriginPos ? posA.rotation : posB.rotation;
				}

				if (useScale)
					targetObject.transform.localScale = IsOriginPos ? posA.localScale : posB.localScale;
			}
		}

		#region HorribleEvents
		public void TogglePos()
		{
			MDebugLog(nameof(TogglePos));
			IsOriginPos = !IsOriginPos;
		}
		public void SetPosOrigin() => IsOriginPos = true;
		public void SetPosTarget() => IsOriginPos = false;
		#endregion
	}
}