using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class VoiceSetter_Mike : VoiceAmplification
	{
		[Header("_" + nameof(VoiceSetter_Mike))]
		[SerializeField] private MMike[] mikes;
		[SerializeField] private Transform mikesParent;
		
		public override void Init(VoiceManager voiceManager)
		{
			base.Init(voiceManager);

			if (mikesParent == null)
				return;
			
			mikes = mikesParent.GetComponentsInChildren<MMike>(true);
		}

		protected override bool IsAmplification(VRCPlayerApi playerAPI)
		{
			bool isTarget = false;

			foreach (MMike mike in mikes)
			{
				if (mike.IsPlayerHoldingAndEnabled(playerAPI))
				{
					isTarget = true;
					break;
				}
			}
			
			MDebugLog(nameof(IsAmplification) + isTarget);
			return isTarget;
		}
	}
}