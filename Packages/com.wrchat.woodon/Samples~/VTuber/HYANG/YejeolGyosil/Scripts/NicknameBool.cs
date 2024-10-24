using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using WRC.Woodon;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	// HACK:
	[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
	public class NicknameBool : MBool
	{
		[SerializeField] private string[] targetNicknames;

		protected override void Start()
		{
			defaultValue = IsTarget();
			base.Start();
		}

		public bool IsTarget()
		{
			foreach (string targetNickname in targetNicknames)
			{
				if (Networking.LocalPlayer.displayName == targetNickname)
					return true;
			}

			return false;
		}
	}
}