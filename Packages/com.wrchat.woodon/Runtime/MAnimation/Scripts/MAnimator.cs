using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace WRC.Woodon
{
	public class MAnimator : MBase
	{
		public const string STATE_STRING = "STATE";

		[field: Header("_" + nameof(MAnimator))]
		[field: SerializeField] public Animator[] Animators { get; private set; }
		[SerializeField] private string boolName;
		[SerializeField] private string intName;
		[SerializeField] private string[] triggerNames;

		[Header("_" + nameof(MAnimator) + " - Options")]
		[SerializeField] private bool defaultState;
		[SerializeField] private MBool mBool;
		[SerializeField] private MValue mValue;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			foreach (Animator animator in Animators)
				animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorControllerStateOnDisable = true;

			// Can't Use keepAnimatorStateOnDisable on Unity 2019''

			SetBool(defaultState);

			if (mBool)
			{
				mBool.RegisterListener(this, nameof(UpdateValue));
				UpdateValue();
			}

			if (mValue)
			{
				mValue.RegisterListener(this, nameof(UpdateValue));
				UpdateValue();
			}
		}

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (mBool)
				SetBool(mBool.Value);

			if (mValue)
				SetInt_L(mValue.Value);
		}

		public void SetBool(string boolName, bool value)
		{
			if (string.IsNullOrEmpty(boolName))
				return;

			foreach (Animator animator in Animators)
				animator.SetBool(boolName, value);
		}

		public void SetBool(bool value) => SetBool(boolName, value);
		
		[ContextMenu(nameof(SetBoolTrue))]
		public void SetBoolTrue() => SetBool(true);
		
		[ContextMenu(nameof(SetBoolFalse))]
		public void SetBoolFalse() => SetBool(false);
		
		[ContextMenu(nameof(ToggleBool))]
		public void ToggleBool() => SetBool(!Animators[0].GetBool(boolName));

		public void SetTrigger(string triggerName)
		{
			MDebugLog(nameof(SetTrigger_L) + triggerName);
			foreach (Animator animator in Animators)
				animator.SetTrigger(triggerName);
		}

		public void SetTrigger_L(int index)
		{
			MDebugLog(nameof(SetTrigger_L) + index);
			foreach (Animator animator in Animators)
				animator.SetTrigger(triggerNames[index]);
		}

		public void SetTrigger_G(int index)
		{
			MDebugLog(nameof(SetTrigger_G) + index);
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetTrigger_L) + index);
		}

		public void SetInt_L(int value)
		{
			MDebugLog(nameof(SetInt_L) + value);
			foreach (Animator animator in Animators)
				animator.SetInteger(intName, value);
		}

		public void SetInt_G(int value)
		{
			MDebugLog(nameof(SetInt_G) + value);
			SendCustomNetworkEvent(NetworkEventTarget.All, nameof(SetInt_L) + value);
		}

		#region HorribleEvents
		
		[ContextMenu(nameof(SetTrigger_L0))]
		public void SetTrigger_L0() => SetTrigger_L(0);
		public void SetTrigger_L1() => SetTrigger_L(1);
		public void SetTrigger_L2() => SetTrigger_L(2);
		public void SetTrigger_L3() => SetTrigger_L(3);
		public void SetTrigger_L4() => SetTrigger_L(4);
		public void SetTrigger_L5() => SetTrigger_L(5);
		public void SetTrigger_L6() => SetTrigger_L(6);
		public void SetTrigger_L7() => SetTrigger_L(7);
		public void SetTrigger_L8() => SetTrigger_L(8);
		public void SetTrigger_L9() => SetTrigger_L(9);

		[ContextMenu(nameof(SetTrigger_G0))]
		public void SetTrigger_G0() => SetTrigger_G(0);
		public void SetTrigger_G1() => SetTrigger_G(1);
		public void SetTrigger_G2() => SetTrigger_G(2);
		public void SetTrigger_G3() => SetTrigger_G(3);
		public void SetTrigger_G4() => SetTrigger_G(4);
		public void SetTrigger_G5() => SetTrigger_G(5);
		public void SetTrigger_G6() => SetTrigger_G(6);
		public void SetTrigger_G7() => SetTrigger_G(7);
		public void SetTrigger_G8() => SetTrigger_G(8);
		public void SetTrigger_G9() => SetTrigger_G(9);

		[ContextMenu(nameof(SetInt_L0))]
		public void SetInt_L0() => SetInt_L(0);
		public void SetInt_L1() => SetInt_L(1);
		public void SetInt_L2() => SetInt_L(2);
		public void SetInt_L3() => SetInt_L(3);
		public void SetInt_L4() => SetInt_L(4);
		public void SetInt_L5() => SetInt_L(5);
		public void SetInt_L6() => SetInt_L(6);
		public void SetInt_L7() => SetInt_L(7);
		public void SetInt_L8() => SetInt_L(8);
		public void SetInt_L9() => SetInt_L(9);

		[ContextMenu(nameof(SetInt_G0))]
		public void SetInt_G0() => SetInt_G(0);
		public void SetInt_G1() => SetInt_G(1);
		public void SetInt_G2() => SetInt_G(2);
		public void SetInt_G3() => SetInt_G(3);
		public void SetInt_G4() => SetInt_G(4);
		public void SetInt_G5() => SetInt_G(5);
		public void SetInt_G6() => SetInt_G(6);
		public void SetInt_G7() => SetInt_G(7);
		public void SetInt_G8() => SetInt_G(8);
		public void SetInt_G9() => SetInt_G(9);
		#endregion
	}
}