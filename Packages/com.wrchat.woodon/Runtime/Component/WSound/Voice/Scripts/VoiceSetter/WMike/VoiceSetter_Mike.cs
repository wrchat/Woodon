using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_Mike : VoiceSetter
	{
		[Header("_" + nameof(VoiceSetter_Mike))]
		[SerializeField] private WMike[] mikes;
		[SerializeField] private Transform mikesParent;
		
		public override void Init(VoiceManager voiceManager)
		{
			base.Init(voiceManager);

			if (mikesParent == null)
				return;
			
			mikes = mikesParent.GetComponentsInChildren<WMike>(true);
		}

		protected override bool IsCondition(VRCPlayerApi playerAPI)
		{
			bool isTarget = false;

			foreach (WMike mike in mikes)
			{
				if (mike.IsPlayerHoldingAndEnabled(playerAPI))
				{
					isTarget = true;
					break;
				}
			}
			
			MDebugLog(nameof(IsCondition) + isTarget);
			return isTarget;
		}
	}
}