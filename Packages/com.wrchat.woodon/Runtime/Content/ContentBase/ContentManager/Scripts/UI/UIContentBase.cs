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
		[SerializeField] protected WAnimator[] contentStateAnimators;

		public override void UpdateUI()
		{
			WDebugLog($"{nameof(UpdateUI)}");

			foreach (WAnimator animator in contentStateAnimators)
				animator.SetInt_L(contentManager.ContentState);
		}
	}
}