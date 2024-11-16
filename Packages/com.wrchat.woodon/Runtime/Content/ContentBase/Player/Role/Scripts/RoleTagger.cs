using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WRC.Woodon
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class RoleTagger : MBase
	{
		[Header("_" + nameof(RoleTagger))]
		[SerializeField] private RoleTag roleTag;
		[SerializeField] private MBool mBool;
		[SerializeField] private MTarget[] mTargets;

		private void Start()
		{
			Init();
		}
		
		private void Init()
		{
			mBool.RegisterListener(this, nameof(UpdateTag));
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

			if (isTarget || mBool.Value)
				RoleUtil.SetPlayerTag(roleTag, Networking.LocalPlayer);
			else
				RoleUtil.SetPlayerTag(RoleTag.None, Networking.LocalPlayer);
		}
	}
}
