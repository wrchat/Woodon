using UdonSharp;
using UnityEngine;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class WPlayerBool : WBool
	{
		[field: Header("_" + nameof(WPlayerBool))]
		[field: SerializeField] public WPlayer WPlayer { get; private set; }

		protected override void Init()
		{
			WPlayer.RegisterListener(this, nameof(UpdateValue));
			UpdateValue();
		}

		public void UpdateValue()
		{
			SetValue(WPlayer.IsTargetPlayer());
		}
	}
}