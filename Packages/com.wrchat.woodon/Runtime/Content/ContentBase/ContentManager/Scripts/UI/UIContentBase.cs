using TMPro;
using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[DefaultExecutionOrder(-5000)]
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class UIContentBase : UIContent
	{
		[Header("_" + nameof(UIContentBase))]
		[SerializeField] protected MAnimator[] mAnimatorsByGameState;

		public override void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)}");

			foreach (MAnimator mAnimator in mAnimatorsByGameState)
				mAnimator.SetInt_L(contentManager.ContentState);
		}
	}
}