using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RoleTagger : WBase
	{
		[Header("_" + nameof(RoleTagger))]
		[SerializeField] private RoleTag roleTag;
		[SerializeField] private WBool wBool;
		[SerializeField] private MTarget[] mTargets;

		private void Start()
		{
			Init();
		}
		
		private void Init()
		{
			wBool.RegisterListener(this, nameof(UpdateTag));
			UpdateTag();
		}

		public void UpdateTag()
		{
			bool isTarget = false;
			foreach (MTarget mTarget in mTargets)
			{
				if (mTarget.IsTargetPlayer())
				{
					isTarget = true;
					break;
				}
			}

			if (isTarget || wBool.Value)
				RoleUtil.SetPlayerTag(roleTag, Networking.LocalPlayer);
			else
				RoleUtil.SetPlayerTag(RoleTag.None, Networking.LocalPlayer);
		}
	}
}
