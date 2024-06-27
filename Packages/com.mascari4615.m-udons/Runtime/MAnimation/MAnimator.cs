using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace Mascari4615
{
	public class MAnimator : MBase
	{
		[field: Header("_" + nameof(MAnimator))]
		[field: SerializeField] public Animator[] Animators { get; private set; }

		[SerializeField] private string boolName;
		[SerializeField] private string intName;
		[SerializeField] private bool defaultState;
		[SerializeField] private CustomBool customBool;

		[SerializeField] private string[] triggerNames;

		private void Start()
		{
			foreach (Animator animator in Animators)
				animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorStateOnDisable = true;
			// animator.keepAnimatorControllerStateOnDisable = true;

			// Can't Use keepAnimatorStateOnDisable on Unity 2019''

			SetBool(defaultState);
		}

		[ContextMenu(nameof(UpdateValue))]
		public void UpdateValue()
		{
			if (customBool)
				SetBool(customBool.Value);
		}

		public void SetBool(bool value)
		{
			if (string.IsNullOrEmpty(boolName))
				return;

			foreach (Animator animator in Animators)
				animator.SetBool(boolName, value);
		}
		
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

		[ContextMenu(nameof(SetTrigger_G0))]
		public void SetTrigger_G0() => SetTrigger_G(0);
		public void SetTrigger_G1() => SetTrigger_G(1);
		public void SetTrigger_G2() => SetTrigger_G(2);

		[ContextMenu(nameof(SetInt_L0))]
		public void SetInt_L0() => SetInt_L(0);
		public void SetInt_L1() => SetInt_L(1);
		public void SetInt_L2() => SetInt_L(2);

		[ContextMenu(nameof(SetInt_G0))]
		public void SetInt_G0() => SetInt_G(0);
		public void SetInt_G1() => SetInt_G(1);
		public void SetInt_G2() => SetInt_G(2);
		#endregion
	}
}