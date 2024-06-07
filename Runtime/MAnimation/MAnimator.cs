
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Mascari4615
{
	public class MAnimator : MBase
	{
		[Header("_" + nameof(MAnimator))]
		[SerializeField] private Animator[] animators;
		public Animator[] Animators => animators;

		[SerializeField] private string boolName;
		[SerializeField] private string intName;
		[SerializeField] private bool defaultState;
		[SerializeField] private CustomBool customBool;

		[SerializeField] private string[] triggerNames;

		private bool boolState;
		public bool BoolState
		{
			get => boolState;
			private set
			{
				boolState = value;
				OnBoolStateChange();
			}
		}
		private void OnBoolStateChange()
		{
			MDebugLog(nameof(OnBoolStateChange));

			if (!string.IsNullOrEmpty(boolName))
				foreach (var animator in animators)
					animator.SetBool(boolName, BoolState);
		}

		private void Start()
		{
			foreach (var animator in animators)
				animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorControllerStateOnDisable = true;

			// Can't Use keepAnimatorStateOnDisable on Unity 2019

			if (!customBool)
			{
				BoolState = defaultState;
			}
			else
			{

			}

			OnBoolStateChange();
		}

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (customBool)
				SetBool(customBool.Value);
		}

		public void SetBool(bool value) => BoolState = value;
		
		[ContextMenu(nameof(SetBoolTrue))]
		public void SetBoolTrue() => SetBool(true);
		
		[ContextMenu(nameof(SetBoolFalse))]
		public void SetBoolFalse() => SetBool(false);
		
		[ContextMenu(nameof(ToggleBool))]
		public void ToggleBool() => SetBool(!BoolState);

		public void SetTrigger_L(int index)
		{
			MDebugLog(nameof(SetTrigger_L) + index);
			foreach (var animator in animators)
				animator.SetTrigger(triggerNames[index]);
		}

		[ContextMenu(nameof(SetTrigger_L0))]
		public void SetTrigger_L0() => SetTrigger_L(0);
		public void SetTrigger_L1() => SetTrigger_L(1);
		public void SetTrigger_L2() => SetTrigger_L(2);

		public void SetTrigger_G(int index)
		{
			MDebugLog(nameof(SetTrigger_G) + index);
			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetTrigger_L) + index);
		}

		[ContextMenu(nameof(SetTrigger_G0))]
		public void SetTrigger_G0() => SetTrigger_G(0);
		public void SetTrigger_G1() => SetTrigger_G(1);
		public void SetTrigger_G2() => SetTrigger_G(2);


		public void SetInt_L(int value)
		{
			MDebugLog(nameof(SetInt_L) + value);
			foreach (var animator in animators)
				animator.SetInteger(intName, value);
		}

		[ContextMenu(nameof(SetInt_L0))]
		public void SetInt_L0() => SetInt_L(0);
		public void SetInt_L1() => SetInt_L(1);
		public void SetInt_L2() => SetInt_L(2);

		public void SetInt_G(int value)
		{
			MDebugLog(nameof(SetInt_G) + value);
			SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetInt_L) + value);
		}

		[ContextMenu(nameof(SetInt_G0))]
		public void SetInt_G0() => SetInt_G(0);
		public void SetInt_G1() => SetInt_G(1);
		public void SetInt_G2() => SetInt_G(2);
	}
}